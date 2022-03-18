using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using VSSystem.Data.File.Images.Define;

namespace VSSystem.Data.File.Images
{
    public class ImageProcess
    {
        public const int THUMB_WIDTH = 180, MEDIUM_WIDTH = 300, LARGE_WIDTH = 580;
        public static void GetImageDimension(byte[] binImage, out int width, out int height)
        {
            width = 0;
            height = 0;
            try
            {
                var imgType = GetImageType(binImage);
                if (imgType == EImageFormat.Png)
                {
                    GetPngDimension(binImage, out width, out height);
                }
                else if (imgType == EImageFormat.Bmp)
                {
                    GetBmpDimension(binImage, out width, out height);
                }
                else
                {
                    GetDefaultDimension(binImage, out width, out height);
                }
            }
            catch { }
        }
        static void GetPngDimension(byte[] binImage, out int width, out int height)
        {
            width = 0;
            height = 0;

            try
            {
                using (var stream = new MemoryStream(binImage))
                {
                    stream.Seek(16, SeekOrigin.Begin);

                    byte[] buff = new byte[8];
                    stream.Read(buff, 0, buff.Length);
                    buff = buff.Reverse().ToArray();
                    height = BitConverter.ToInt32(buff, 0);
                    width = BitConverter.ToInt32(buff, 4);
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch { }
        }
        
        static void GetDefaultDimension(byte[] binImage, out int width, out int height)
        {
            width = 0;
            height = 0;

            try
            {
                using (var stream = new MemoryStream(binImage))
                {
                    try
                    {
                        using (var img = System.Drawing.Image.FromStream(stream))
                        {
                            width = img.Width;
                            height = img.Height;
                            img.Dispose();
                        }
                    }
                    catch { }
                    stream.Close();
                    stream.Dispose();
                }
            }
            catch { }
        }
        static void GetBmpDimension(byte[] binImage, out int width, out int height)
        {
            width = 0;
            height = 0;

            try
            {
                using (var stream = new MemoryStream(binImage))
                {
                    stream.Seek(18, SeekOrigin.Begin);

                    byte[] buff = new byte[8];
                    stream.Read(buff, 0, buff.Length);
                    width = BitConverter.ToInt32(buff, 0);
                    height = BitConverter.ToInt32(buff, 4);

                    stream.Close();
                    stream.Dispose();
                }
            }
            catch { }
        }

        public static EImageFormat GetImageType(byte[] imageData)
        {
            try
            {
                var headerCode = GetBinHeaderInfo(imageData);
                return GetHeaderFormat(headerCode);
            }
            catch (Exception)
            {

                throw;
            }
        }
        static byte[] GetBinHeaderInfo(byte[] imageData)
        {
            try
            {
                byte[] result = imageData.Take(8).ToArray();
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        static EImageFormat GetHeaderFormat(byte[] headerCode)
        {
            Dictionary<byte[], EImageFormat> mFormat = new Dictionary<byte[], EImageFormat>()
            {
                { Encoding.ASCII.GetBytes("BM"), EImageFormat.Bmp },
                { Encoding.ASCII.GetBytes("GIF"), EImageFormat.Gif },
                { Encoding.ASCII.GetBytes("RIFF"), EImageFormat.Riff },
                { new byte[]{ 0xff, 0xd8 }, EImageFormat.Jpeg },
                { new byte[]{ 0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a }, EImageFormat.Png },
                { new byte[]{ 0x49, 0x49, 0x2a  }, EImageFormat.Tiff },
                { new byte[]{ 0x4d, 0x4d, 0x2a }, EImageFormat.Tiff },
            };

            foreach (var mf in mFormat)
            {
                if (mf.Key.SequenceEqual(headerCode.Take(mf.Key.Length)))
                {
                    return mf.Value;
                }
            }

            return EImageFormat.Jpeg;
        }

        public static byte[] ResizeImage(byte[] imageData, int targetWidth, int targetHeight, bool convertToJpeg = false)
        {
            try
            {
                int newWidth, newHeight;
                byte[] result = ResizeImage(imageData, targetWidth, targetHeight, out newWidth, out newHeight, convertToJpeg);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("ResizeImageToThumbnail Exception: " + ex.Message);
            }

        }

        public static byte[] ResizeImage(byte[] imageData, int targetWidth, int targetHeight, out int newWidth, out int newHeight, bool convertToJpeg = false)
        {
            newWidth = 0;
            newHeight = 0;
            try
            {
                byte[] result = imageData;
                if (targetWidth > 0 && targetHeight <= 0)
                {
                    targetHeight = targetWidth;
                }
                if (targetHeight > 0 && targetWidth <= 0)
                {
                    targetWidth = targetHeight;
                }

                if (targetHeight > 0 && targetWidth > 0)
                {
                    System.Drawing.Image originalImage = null;
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        try
                        {
                            originalImage = System.Drawing.Image.FromStream(ms);
                        }
                        catch
                        {
                        }
                        ms.Close();
                        ms.Dispose();

                    }
                    if (originalImage != null)
                    {
                        int width = originalImage.Width;
                        int height = originalImage.Height;

                        double imageRatio = width * 1.0 / height;


                        newWidth = Convert.ToInt32(Math.Floor(imageRatio * targetHeight));
                        newHeight = Convert.ToInt32(Math.Floor(targetWidth / imageRatio));

                        if (newWidth > targetWidth)
                        {
                            newWidth = targetWidth;
                        }
                        else
                        {
                            newHeight = targetHeight;
                        }

                        using (var thumbImage = new Bitmap(newWidth, newHeight))
                        {
                            using (Graphics g = Graphics.FromImage(thumbImage))
                            {
                                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                                g.DrawImage(originalImage, 0, 0, newWidth, newHeight);
                                g.Dispose();
                            }
                            using (var ms = new MemoryStream())
                            {
                                if (convertToJpeg)
                                {
                                    thumbImage.Save(ms, ImageFormat.Jpeg);
                                }
                                else
                                {
                                    thumbImage.Save(ms, ImageFormat.Png);
                                }

                                result = ms.ToArray();
                                ms.Close();
                                ms.Dispose();
                            }
                            thumbImage.Dispose();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("ResizeImageToThumbnail Exception: " + ex.Message);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="imageFormat">PNG, JPEG, BMP</param>
        /// <returns></returns>
        public static byte[] ConvertImage(byte[] input, string imageFormat)
        {
            byte[] result = input;
            try
            {
                using (var srcStream = new MemoryStream(input))
                {
                    using (var outStream = new MemoryStream())
                    {
                        using (var img = System.Drawing.Image.FromStream(srcStream))
                        {
                            if (imageFormat.Equals("Jpeg", StringComparison.InvariantCultureIgnoreCase)
                                || imageFormat.Equals("Jpg", StringComparison.InvariantCultureIgnoreCase))
                            {
                                img.Save(outStream, ImageFormat.Jpeg);
                            }
                            else if (imageFormat.Equals("Png", StringComparison.InvariantCultureIgnoreCase))
                            {
                                img.Save(outStream, ImageFormat.Png);
                            }
                            else if (imageFormat.Equals("Bmp", StringComparison.InvariantCultureIgnoreCase))
                            {
                                img.Save(outStream, ImageFormat.Bmp);
                            }

                            img.Dispose();
                        }
                        outStream.Close();
                        outStream.Dispose();
                        result = outStream.ToArray();
                    }
                    srcStream.Close();
                    srcStream.Dispose();
                }
            }
            catch { }
            return result;
        }

        public static byte[] ConvertImage(byte[] input, EImageFormat imageFormat)
        {
            byte[] result = input;
            try
            {
                using (var srcStream = new MemoryStream(input))
                {
                    using (var outStream = new MemoryStream())
                    {
                        using (var img = System.Drawing.Image.FromStream(srcStream))
                        {
                            if (imageFormat == EImageFormat.Jpeg)
                            {
                                img.Save(outStream, ImageFormat.Jpeg);
                            }
                            else if (imageFormat == EImageFormat.Png)
                            {
                                img.Save(outStream, ImageFormat.Png);
                            }
                            else if (imageFormat == EImageFormat.Bmp)
                            {
                                img.Save(outStream, ImageFormat.Bmp);
                            }
                            else if (imageFormat == EImageFormat.Gif)
                            {
                                img.Save(outStream, ImageFormat.Gif);
                            }
                            else if (imageFormat == EImageFormat.Tiff)
                            {
                                img.Save(outStream, ImageFormat.Tiff);
                            }
                            img.Dispose();
                        }
                        outStream.Close();
                        outStream.Dispose();
                        result = outStream.ToArray();
                    }
                    srcStream.Close();
                    srcStream.Dispose();
                }
            }
            catch { }
            return result;
        }
    }
}
