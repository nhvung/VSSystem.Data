using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Images.Cache
{
    public abstract class CacheImage : ICacheImage
    {
        protected string _name;
        public string Name { get { return _name; } }
        protected int _limitSize;
        protected long _limitLength;
        public CacheImage(string name, int limitSize, long limitLength)
        {
            _name = name;
            _limitLength = limitLength * 1024 * 1024;
            _limitSize = limitSize;
        }
        public abstract byte[] Get(long id, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null);

        public abstract void Load(Action<string> debugLogAction = null, Action<Exception> errorLogAction = null);

        public abstract void Push(long id, byte[] data, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null);
        public string GetName()
        {
            return _name;
        }
        public abstract void Delete(long id);
    }
}
