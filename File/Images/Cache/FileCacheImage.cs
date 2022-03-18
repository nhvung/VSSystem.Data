using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSSystem.Data.Images.Cache
{
    public class FileCacheImage : CacheImage
    {
        protected Dictionary<long, long> _cacheByID;
        string _positionFilePath, _dataFilePath;

        public FileCacheImage(string cacheFolderPath, string name, int limitSize = 1000, long limitLength = 1024)
            : base(name, limitSize, limitLength)
        {
            _positionFilePath = cacheFolderPath + "/" + name + ".pf";
            _dataFilePath = cacheFolderPath + "/" + name + ".dtf";
        }

        public override void Push(long id, byte[] data, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            if (id <= 0 || data == null || data.Length == 0 || (_cacheByID?.ContainsKey(id) ?? false))
            {
                return;
            }
            var positionFile = new FileInfo(_positionFilePath);
            var dataFile = new FileInfo(_dataFilePath);

            debugLogAction?.Invoke(string.Format("Data file path: ", _dataFilePath));

            if (_cacheByID == null || _cacheByID.Count >= _limitSize || ((dataFile?.Exists ?? false) && dataFile?.Length >= _limitLength))
            {
                debugLogAction?.Invoke(string.Format("Cache exceed. Size: {0}, Length: {1}", _cacheByID.Count, dataFile?.Length));
                _cacheByID = new Dictionary<long, long>();

                try
                {
                    positionFile.Delete();
                }
                catch { }
                try
                {
                    dataFile.Delete();
                }
                catch { }
            }

            try
            {
                if (!positionFile.Directory.Exists)
                {
                    positionFile.Directory.Create();
                }

                long pos = -1;
                using (var dataStream = dataFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    dataStream.Seek(0, SeekOrigin.End);
                    pos = dataStream.Position;
                    using (var bw = new File.BinaryWriter(dataStream))
                    {
                        bw.Write(data);
                        bw.Close();
                        bw.Dispose();
                    }
                    dataStream.Close();
                    dataStream.Dispose();
                }
                if (pos > -1)
                {
                    using (var positionStream = positionFile.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                    {
                        positionStream.Seek(0, SeekOrigin.End);
                        using (var bw = new File.BinaryWriter(positionStream))
                        {
                            bw.Write(id);
                            bw.Write(pos);
                            bw.Close();
                            bw.Dispose();
                        }

                        positionStream.Close();
                        positionStream.Dispose();
                    }
                    _cacheByID[id] = pos;
                }
            }
            catch { }
        }

        public override byte[] Get(long id, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            byte[] result = null;

            try
            {
                var dataFile = new FileInfo(_dataFilePath);
                if (!dataFile.Exists)
                {
                    _cacheByID = new Dictionary<long, long>();
                }
                if ((_cacheByID?.ContainsKey(id) ?? false))
                {
                    var pos = _cacheByID[id];
                    using (var dataStream = dataFile.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        dataStream.Seek(pos, SeekOrigin.Begin);
                        using (var br = new File.BinaryReader(dataStream))
                        {
                            result = br.ReadBytes();
                            br.Close();
                            br.Dispose();
                        }
                        dataStream.Close();
                        dataStream.Dispose();
                    }
                }
            }
            catch { }
            return result;
        }

        public override void Load(Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            if (_cacheByID == null)
            {
                _cacheByID = new Dictionary<long, long>();
            }
            var positionFile = new FileInfo(_positionFilePath);
            if (positionFile?.Exists ?? false)
            {
                using (var positionStream = positionFile.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    positionStream.Seek(0, SeekOrigin.Begin);

                    using (var br = new File.BinaryReader(positionStream))
                    {
                        while (positionStream.Position < positionStream.Length)
                        {
                            long id = br.ReadInt64();
                            long pos = br.ReadInt64();
                            _cacheByID[id] = pos;
                        }

                        br.Close();
                        br.Dispose();
                    }
                    positionStream.Close();
                    positionStream.Dispose();
                }
            }
        }

        public override void Delete(long id)
        {

            try
            {
                if ((_cacheByID?.ContainsKey(id) ?? false))
                {
                    _cacheByID.Remove(id);
                }
            }
            catch { }
        }
    }
}
