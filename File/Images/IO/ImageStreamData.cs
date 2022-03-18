using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSSystem.Data.File;
using VSSystem.Data.File.Define;
using VSSystem.Data.File.Images.Define;

namespace VSSystem.Data.File.Images.IO
{
    class ImageStreamData
    {
        #region Write
        static long WriteImage(Stream stream, byte[] binaryImageData)
        {
            long position = -1;
            try
            {
                using (var bw = new File.BinaryWriter(stream))
                {
                    position = stream.Position;
                    bw.Write((byte)EBinaryType.Bytes);
                    bw.Write(binaryImageData.Length);
                    bw.Write(binaryImageData);
                    bw.Close();
                    bw.Dispose();
                }

            }
            catch// (Exception ex)
            {
                //throw ex;
            }
            return position;
        }
        public static long WriteImage(FileInfo file, byte[] binaryImageData)
        {
            long position = -1;
            using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
            {
                position = WriteImage(fs, binaryImageData);
                fs.Close();
                fs.Dispose();
            }
            return position;
        }
        public static long WriteImage(string fileName, byte[] binaryImageData)
        {
            return WriteImage(new FileInfo(fileName), binaryImageData);
        }
        #endregion

        #region Read
        static ImageInfo GetImage(Stream stream, long id, long position)
        {
            ImageInfo result = new ImageInfo(id);
            try
            {
                stream.Seek(position, SeekOrigin.Begin);
                using (var br = new File.BinaryReader(stream))
                {
                    result.Data = br.ReadBytes();

                    br.Close();
                    br.Dispose();
                }
            }
            catch //(Exception ex)
            {
            }
            return result;
        }
        static public ImageInfo GetImage(ImagePositionInfo imagePositionInfoObj)
        {
            ImageInfo result = new ImageInfo();
            try
            {
                FileInfo file = new FileInfo(imagePositionInfoObj.Path);
                if (file.Exists)
                {
                    using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        result = GetImage(fs, imagePositionInfoObj.Image_ID, imagePositionInfoObj.Position);
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public List<ImageInfo> GetImages(List<ImagePositionInfo> imagePositionInfoObjs, string rootFolderPath, int nThreads = 1)
        {
            List<ImageInfo> result = new List<ImageInfo>();
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion
    }
}
