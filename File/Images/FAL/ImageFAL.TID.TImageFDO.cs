using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using VSSystem.Data.File.Images.DTO;

namespace VSSystem.Data.File.Images.FAL
{
    public class ImageFAL<TID, TImageFDO> : IDataFAL<TID, PositionObjectInFileDTO<TID>, TImageFDO>
    {
        protected ImageFAL()
        {
            _ConvertStreamToImage = null;
            _ProcessImagesInFile = null;
        }
        protected Action<Stream, TImageFDO> _ConvertStreamToImage;
        public override TImageFDO GetObject(TID id, string shareDataFileFolderPath = "")
        {
            try
            {
                TImageFDO result = Activator.CreateInstance<TImageFDO>();
                var posObj = GetLocation(id);
                byte[] buff = GetDataInFile(posObj, shareDataFileFolderPath);
                using (MemoryStream ms = new MemoryStream(buff))
                {
                    _ConvertStreamToImage?.Invoke(ms, result);
                    ms.Close();
                    ms.Dispose();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override List<TImageFDO> GetObjects(List<TID> ids, string shareDataFileFolderPath = "")
        {
            try
            {
                List<TImageFDO> result = new List<TImageFDO>();
                var posObjs = GetLocations(ids);
                var buffObjs = GetDataInFile(posObjs, shareDataFileFolderPath);
                foreach (var buff in buffObjs)
                {
                    TImageFDO imgObj = Activator.CreateInstance<TImageFDO>();
                    using (MemoryStream ms = new MemoryStream(buff))
                    {

                        try
                        {
                            _ConvertStreamToImage?.Invoke(ms, imgObj);
                            result.Add(imgObj);
                        }
                        catch//(Exception ex)
                        {
                        }
                        ms.Close();
                        ms.Dispose();
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected Action<string, Action<TImageFDO>, CancellationToken> _ProcessImagesInFile;
        public void ProcessImagesInFile(string dataFilePath, Action<TImageFDO> processImageAction, CancellationToken cancellationToken = default)
        {
            _ProcessImagesInFile?.Invoke(dataFilePath, processImageAction, cancellationToken);
        }
    }
}
