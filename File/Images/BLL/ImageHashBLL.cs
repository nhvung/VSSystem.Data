using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using VSSystem.Data.BLL;
using VSSystem.Data.File;
using VSSystem.Data.File.Extensions;
using VSSystem.Data.File.Images.DAL;
using VSSystem.Data.File.Images.DTO;
using VSSystem.Data.File.Images.IO;

namespace VSSystem.Data.File.Images.BLL
{
    public class ImageHashBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
        where TDAL : IImageHashDAL<TDTO>
        where TDTO : ImageHashDTO
    {

        static TDTO GetHashImage(string tableName, byte[] sha1Key)
        {
            return GetDAL(tableName).GetHashImage(sha1Key);
        }
        static TInfoDTO GetHashImageInfo<TInfoDTO>(string tableName, long image_ID)
            where TInfoDTO : ImageHashInfoDTO
        {
            return GetDAL(tableName).GetHashImageInfo<TInfoDTO>(image_ID);
        }
        static List<TImageHashInfoDTO> GetHashImageInfo<TImageHashInfoDTO>(string tableName, List<long> image_IDs)
            where TImageHashInfoDTO : ImageHashInfoDTO
        {
            return GetDAL(tableName).GetHashImageInfo<TImageHashInfoDTO>(image_IDs);
        }

        public static byte[] GetImageData(string tableName, long image_ID, string fileDataFolderPath)
        {
            var imgDbInfo = GetHashImageInfo<ImageHashInfoDTO>(tableName, image_ID);
            DirectoryInfo rootFolder = new DirectoryInfo(fileDataFolderPath);
            if (rootFolder.Exists)
            {
                ImagePositionInfo imgPosInfo = new ImagePositionInfo(imgDbInfo.Image_ID, rootFolder.FullName + "/" + imgDbInfo.Path, imgDbInfo.Position);
                var imgInfo = ImageStreamData.GetImage(imgPosInfo);
                return imgInfo.Data;
            }
            return null;
        }
        public static ImageInfo GetImageInfo(string tableName, long image_ID, string fileDataFolderPath)
        {
            var imgDbInfo = GetHashImageInfo<ImageHashInfoDTO>(tableName, image_ID);
            if (imgDbInfo != null)
            {
                DirectoryInfo rootFolder = new DirectoryInfo(fileDataFolderPath);
                if (rootFolder.Exists)
                {
                    ImagePositionInfo imgPosInfo = new ImagePositionInfo(imgDbInfo.Image_ID, rootFolder.FullName + "/" + imgDbInfo.Path, imgDbInfo.Position);
                    var imgInfo = ImageStreamData.GetImage(imgPosInfo);
                    return imgInfo;
                }
            }

            return null;
        }

        static object _lockWriteObj;
        public static TDTO WriteImage(string tableName, string templateName, string fileDataFolderPath, byte[] imageBytes, long image_ID, int importType
            , Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            try
            {
                if (_lockWriteObj == null)
                {
                    _lockWriteObj = new object();
                }
                long lUtcNow = Generator.ToInt64(DateTime.UtcNow);
                var sha1Key = imageBytes.GetSha1Hash();
                debugLogAction?.Invoke("Check exist image in database: 0x" + BitConverter.ToString(sha1Key).Replace("-", ""));
                TDTO pHashObj = null;

                pHashObj = GetHashImage(tableName, sha1Key);
            STEP1:
                if (pHashObj == null)
                {
                    if (image_ID <= 0)
                    {
                        image_ID = Generator.GenerateInt64ID();
                    }
                    debugLogAction?.Invoke("Create new image");
                    pHashObj = Activator.CreateInstance<TDTO>();
                    pHashObj.Sha1 = sha1Key;
                    pHashObj.Image_ID = image_ID;
                    pHashObj.CreatedDateTime = lUtcNow;

                    try
                    {
                        int width, height;
                        ImageProcess.GetImageDimension(imageBytes, out width, out height);
                        pHashObj.Width = width;
                        pHashObj.Height = height;
                    }
                    catch (Exception ex)
                    {
                        errorLogAction?.Invoke(ex);
                    }

                    try
                    {
                        lock (_lockWriteObj)
                        {
                        CREATE_FILE:
                            debugLogAction?.Invoke("Write bytes to file");
                            var lastFile = ImageHashFileBLL.GetLastFile(tableName + "File", templateName, importType);
                            pHashObj.File_ID = lastFile.File_ID;
                            pHashObj.ImportType = importType;
                            FileInfo pFile = new FileInfo(fileDataFolderPath + "/" + lastFile.Path);
                            long maxFileLength = File.Define.FileGlobalValues.MAX_FILE_LENGTH;
                            if (pFile.Exists && pFile.Length >= maxFileLength)
                            {
                                ImageHashFileBLL.CloseFile(tableName + "File", lastFile.File_ID);
                                goto CREATE_FILE;
                            }
                            if (!pFile.Directory.Exists)
                            {
                                pFile.Directory.Create();
                            }

                            pHashObj.Position = DataFile.Write(pFile, imageBytes); 
                        }
                    }
                    catch (Exception ex)
                    {
                        errorLogAction?.Invoke(ex);
                    }

                    try
                    {
                        debugLogAction?.Invoke("Insert record to mapping table");
                        ImageHashBLL.Insert(tableName, pHashObj);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("Duplicate entry", StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            Thread.Sleep(500);
                            pHashObj = GetHashImage(tableName, sha1Key);
                            if (pHashObj == null)
                            {
                                goto STEP1;
                            }
                        }
                        else
                        {
                            errorLogAction?.Invoke(ex);
                        }
                    }
                    debugLogAction?.Invoke("Done process.");
                }

                debugLogAction?.Invoke("Return");
                return pHashObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TDTO WriteImage(string tableName, string templateName, string fileDataFolderPath, FileInfo imageFile, long image_ID, int importType
            , Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            if (imageFile?.Exists ?? false)
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imageFile.FullName);
                return WriteImage(tableName, templateName, fileDataFolderPath, imageBytes, image_ID, importType, debugLogAction, errorLogAction);
            }
            else
            {
                throw new Exception("Image file not found");
            }
        }
        public static TDTO WriteImage(string tableName, string templateName, string fileDataFolderPath, byte[] imageBytes, int importType
            , Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            return WriteImageV2(tableName, templateName, fileDataFolderPath, imageBytes, 0, importType, debugLogAction, errorLogAction);
        }
        public static TDTO WriteImage(string tableName, string templateName, string fileDataFolderPath, FileInfo imageFile, int importType
            , Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            if (imageFile?.Exists ?? false)
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(imageFile.FullName);
                return WriteImage(tableName, templateName, fileDataFolderPath, imageBytes, 0, importType, debugLogAction, errorLogAction);
            }
            else
            {
                throw new Exception("Image file not found");
            }
        }

        public static TDTO WriteImageV2(string tableName, string templateName, string fileDataFolderPath, byte[] imageBytes, long image_ID, int importType
            , Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            try
            {
                if (_lockWriteObj == null)
                {
                    _lockWriteObj = new object();
                }
                long lUtcNow = Generator.ToInt64(DateTime.UtcNow);
                var sha1Key = imageBytes.GetSha1Hash();
                debugLogAction?.Invoke("Check exist image in database: 0x" + BitConverter.ToString(sha1Key).Replace("-", ""));
                TDTO pHashObj = null;

                lock (_lockWriteObj)
                {
                    pHashObj = GetHashImage(tableName, sha1Key);
                STEP1:
                    if (pHashObj == null)
                    {
                        if (image_ID <= 0)
                        {
                            image_ID = Generator.GenerateInt64ID();
                        }
                        debugLogAction?.Invoke("Create new image");
                        pHashObj = Activator.CreateInstance<TDTO>();
                        pHashObj.Sha1 = sha1Key;
                        pHashObj.Image_ID = image_ID;
                        pHashObj.CreatedDateTime = lUtcNow;

                        try
                        {
                            int width, height;
                            ImageProcess.GetImageDimension(imageBytes, out width, out height);
                            pHashObj.Width = width;
                            pHashObj.Height = height;
                        }
                        catch (Exception ex)
                        {
                            errorLogAction?.Invoke(ex);
                        }

                        try
                        {
                        CREATE_FILE:
                            debugLogAction?.Invoke("Write bytes to file");
                            var lastFile = ImageHashFileBLL.GetLastFile(tableName + "File", templateName, importType);
                            
                            FileInfo pFile = new FileInfo(fileDataFolderPath + "/" + lastFile.Path);
                            long maxFileLength = File.Define.FileGlobalValues.MAX_FILE_LENGTH;

                            bool closeFileImperative = false;
                        CLOSE_FILE:                            
                            if (pFile.Exists && pFile.Length >= maxFileLength || closeFileImperative)
                            {
                                ImageHashFileBLL.CloseFile(tableName + "File", lastFile.File_ID);
                                goto CREATE_FILE;
                            }
                            if (!pFile.Directory.Exists)
                            {
                                pFile.Directory.Create();
                            }

                            pHashObj.File_ID = lastFile.File_ID;
                            pHashObj.ImportType = importType;

                            long position = -1;
                            try
                            {
                                position = DataFile.Write(pFile, imageBytes, errorLogAction);
                            }
                            catch { }
                            if (position == -1)
                            {
                                closeFileImperative = true;
                                goto CLOSE_FILE;
                            }

                            pHashObj.Position = position;
                        }
                        catch (Exception ex)
                        {
                            errorLogAction?.Invoke(ex);
                        }

                        try
                        {
                            debugLogAction?.Invoke("Insert record to mapping table");
                            ImageHashBLL.Insert(tableName, pHashObj);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.IndexOf("Duplicate entry", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                Thread.Sleep(500);
                                pHashObj = GetHashImage(tableName, sha1Key);
                                if (pHashObj == null)
                                {
                                    goto STEP1;
                                }
                            }
                            else
                            {
                                errorLogAction?.Invoke(ex);
                            }
                        }
                        debugLogAction?.Invoke("Done process.");
                    }
                }
            

                debugLogAction?.Invoke("Return");
                return pHashObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class ImageHashBLL<TImageHashDAL> : ImageHashBLL<TImageHashDAL, ImageHashDTO>
        where TImageHashDAL : IImageHashDAL<ImageHashDTO>
    {

    }
    public class ImageHashBLL : ImageHashBLL<IImageHashDAL<ImageHashDTO>>
    {

    }
}
