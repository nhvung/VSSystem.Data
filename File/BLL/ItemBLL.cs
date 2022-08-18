using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VSSystem.Data.BLL;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.Define;
using VSSystem.Data.File.DTO;
using VSSystem.Data.File.Extensions;

namespace VSSystem.Data.File.BLL
{
    public class ItemBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
        where TDAL : IItemDAL<TDTO>
        where TDTO : ItemDTO
    {
        static TDTO GetItemByHash(string tableName, byte[] sha1Bytes)
        {
            return GetDAL(tableName).GetItemByHash(sha1Bytes);
        }
        static TInfoDTO GetItemInfoByID<TInfoDTO>(string tableName, long id)
            where TInfoDTO : ItemInfoDTO
        {
            return GetDAL(tableName).GetItemInfoByID<TInfoDTO>(id);
        }
        static List<TInfoDTO> GetItemInfoByID<TInfoDTO>(string tableName, List<long> ids)
            where TInfoDTO : ItemInfoDTO
        {
            return GetDAL(tableName).GetItemInfoByID<TInfoDTO>(ids);
        }
        public static byte[] GetItemData(string tableName, long id, string fileDataFolderPath)
        {
            try
            {
                byte[] result = null;
                var imgDbInfo = GetItemInfoByID<ItemInfoDTO>(tableName, id);
                DirectoryInfo rootFolder = new DirectoryInfo(fileDataFolderPath);
                if (rootFolder.Exists)
                {
                    FileInfo file = new FileInfo(rootFolder.FullName + "/" + imgDbInfo.Path);
                    if (file.Exists)
                    {
                        using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            stream.Seek(imgDbInfo.Position, SeekOrigin.Begin);
                            using (var br = new BinaryReader(stream))
                            {
                                result = br.ReadBytes();
                                br.Close();
                                br.Dispose();
                            }
                            stream.Close();
                            stream.Dispose();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static Stream GetItemStream(string tableName, long id, string fileDataFolderPath)
        {
            try
            {
                Stream result = null;
                var itemInfoObj = GetItemInfoByID<ItemInfoDTO>(tableName, id);
                if (itemInfoObj != null)
                {
                    DirectoryInfo rootFolder = new DirectoryInfo(fileDataFolderPath);
                    if (rootFolder.Exists)
                    {
                        FileInfo file = new FileInfo(rootFolder.FullName + "/" + itemInfoObj.Path);
                        if (file.Exists)
                        {
                            var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            stream.Seek(itemInfoObj.Position, SeekOrigin.Begin);
                            result = new BinaryReader(stream).ReadStream();
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        static object _lockWriteObj;

        public static TDTO Write(string tableName, string templateName, string fileDataFolderPath, Stream itemStream, long id, ESavingOptions savingOption = ESavingOptions.AllInOne)
        {
            try
            {
                if (_lockWriteObj == null)
                {
                    _lockWriteObj = new object();
                }
                long lUtcNow = Generator.ToInt64(DateTime.UtcNow);
                var sha1Key = itemStream.GetSha1Hash();
                TDTO pObj = null;

                lock (_lockWriteObj)
                {
                    pObj = GetItemByHash(tableName, sha1Key);
                STEP1:
                    if (pObj == null)
                    {
                        if (id <= 0)
                        {
                            id = Generator.GenerateInt64ID();
                        }
                        pObj = Activator.CreateInstance<TDTO>();
                        pObj.Sha1 = sha1Key;
                        pObj.ID = id;
                        pObj.CreatedDateTime = lUtcNow;

                        try
                        {
                        CREATE_FILE:
                            var lastFile = ItemFileBLL.GetLastFile(tableName + "File", templateName);
                            pObj.File_ID = lastFile.File_ID;
                            FileInfo pFile = new FileInfo(fileDataFolderPath + "/" + lastFile.Path);
                            long maxFileLength = Define.FileGlobalValues.MAX_FILE_LENGTH;
                            if (pFile.Exists && pFile.Length >= maxFileLength)
                            {
                                ItemFileBLL.CloseFile(tableName + "File", lastFile.File_ID);
                                goto CREATE_FILE;
                            }
                            if (!pFile.Directory.Exists)
                            {
                                pFile.Directory.Create();
                            }

                            pObj.Position = DataFile.Write(pFile, itemStream);

                            if (savingOption == ESavingOptions.SideBySide)
                            {
                                ItemFileBLL.CloseFile(tableName + "File", lastFile.File_ID);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            Insert(tableName, pObj);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.IndexOf("Duplicate entry", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                Thread.Sleep(500);
                                pObj = GetItemByHash(tableName, sha1Key);
                                if (pObj == null)
                                {
                                    goto STEP1;
                                }
                            }
                        }
                    }
                }
                return pObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TDTO Write(string tableName, string templateName, string fileDataFolderPath, byte[] itemBytes, long id, ESavingOptions savingOption = ESavingOptions.AllInOne)
        {
            try
            {
                if (_lockWriteObj == null)
                {
                    _lockWriteObj = new object();
                }
                long lUtcNow = Generator.ToInt64(DateTime.UtcNow);
                var sha1Key = itemBytes.GetSha1Hash();
                TDTO pObj = null;

                lock (_lockWriteObj)
                {
                    pObj = GetItemByHash(tableName, sha1Key);
                STEP1:
                    if (pObj == null)
                    {
                        if (id <= 0)
                        {
                            id = Generator.GenerateInt64ID();
                        }
                        pObj = Activator.CreateInstance<TDTO>();
                        pObj.Sha1 = sha1Key;
                        pObj.ID = id;
                        pObj.CreatedDateTime = lUtcNow;

                        try
                        {
                        CREATE_FILE:
                            var lastFile = ItemFileBLL.GetLastFile(tableName + "File", templateName);
                            pObj.File_ID = lastFile.File_ID;
                            FileInfo pFile = new FileInfo(fileDataFolderPath + "/" + lastFile.Path);
                            long maxFileLength = Define.FileGlobalValues.MAX_FILE_LENGTH;
                            if (pFile.Exists && pFile.Length >= maxFileLength)
                            {
                                ItemFileBLL.CloseFile(tableName + "File", lastFile.File_ID);
                                goto CREATE_FILE;
                            }
                            if (!pFile.Directory.Exists)
                            {
                                pFile.Directory.Create();
                            }

                            pObj.Position = DataFile.Write(pFile, itemBytes);
                            if (savingOption == ESavingOptions.SideBySide)
                            {
                                ItemFileBLL.CloseFile(tableName + "File", lastFile.File_ID);
                            }
                        }
                        catch
                        {
                        }

                        try
                        {
                            Insert(tableName, pObj);
                        }
                        catch (Exception ex)
                        {
                            if (ex.Message.IndexOf("Duplicate entry", StringComparison.InvariantCultureIgnoreCase) >= 0)
                            {
                                Thread.Sleep(500);
                                pObj = GetItemByHash(tableName, sha1Key);
                                if (pObj == null)
                                {
                                    goto STEP1;
                                }
                            }
                        }
                    }
                }

                return pObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static TDTO Write(string tableName, string templateName, string fileDataFolderPath, FileInfo itemFile, long id, ESavingOptions savingOption = ESavingOptions.AllInOne)
        {
            if (itemFile?.Exists ?? false)
            {
                byte[] itemBytes = System.IO.File.ReadAllBytes(itemFile.FullName);
                return Write(tableName, templateName, fileDataFolderPath, itemBytes, id, savingOption);
            }
            else
            {
                throw new Exception("Item file not found");
            }
        }
        public static TDTO Write(string tableName, string templateName, string fileDataFolderPath, byte[] itemBytes, ESavingOptions savingOption = ESavingOptions.AllInOne)
        {
            return Write(tableName, templateName, fileDataFolderPath, itemBytes, 0, savingOption);
        }
        public static TDTO Write(string tableName, string templateName, string fileDataFolderPath, FileInfo itemFile, ESavingOptions savingOption = ESavingOptions.AllInOne)
        {
            return Write(tableName, templateName, fileDataFolderPath, itemFile, 0, savingOption);
        }
    }

    public class ItemBLL<TDAL> : ItemBLL<TDAL, ItemDTO>
       where TDAL : IItemDAL<ItemDTO>
    {

    }
    public class ItemBLL : ItemBLL<IItemDAL<ItemDTO>>
    {

    }
}
