using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSSystem.Data.Images.Cache
{
    public class MemoryCacheImage : CacheImage
    {
        protected Dictionary<long, long> _cacheByID;
        protected MemoryStream _stream;
        public MemoryCacheImage(string name, int limitSize = 1000, long limitLength = 1024)
            : base(name, limitSize, limitLength)
        {
            _stream = new MemoryStream();
            _lockObj = new object();
        }
        object _lockObj;
        public override byte[] Get(long id, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            byte[] result = null;

            try
            {
                if (_stream?.Length > 0)
                {
                    if ((_cacheByID?.ContainsKey(id) ?? false))
                    {
                        var pos = _cacheByID[id];
                        lock (_lockObj)
                        {
                            _stream.Seek(pos, SeekOrigin.Begin);
                            using (var br = new File.BinaryReader(_stream, Encoding.UTF8, true))
                            {
                                result = br.ReadBytes();
                                br.Close();
                                br.Dispose();
                            }
                        }
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
            _stream = new MemoryStream();
        }

        public override void Push(long id, byte[] data, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            if (id <= 0 || data == null || data.Length == 0 || (_cacheByID?.ContainsKey(id) ?? false))
            {
                return;
            }
            if (_cacheByID == null || _cacheByID.Count >= _limitSize || _stream.Length > _limitLength)
            {
                debugLogAction?.Invoke("Cache exceed.");
                _cacheByID = new Dictionary<long, long>();
                _stream.Close();
                _stream.Dispose();
                _stream = new MemoryStream();
            }

            try
            {
                long pos = -1;
                _stream.Seek(0, SeekOrigin.End);
                pos = _stream.Position;
                using (var bw = new File.BinaryWriter(_stream, Encoding.UTF8, true))
                {
                    bw.Write(data);
                    bw.Close();
                    bw.Dispose();
                }
                if (pos > -1)
                {
                    _cacheByID[id] = pos;
                }
            }
            catch { }
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
