using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Images.Cache
{
    public interface ICacheImage
    {
        void Load(Action<string> debugLogAction = null, Action<Exception> errorLogAction = null);
        void Push(long id, byte[] data, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null);
        byte[] Get(long id, Action<string> debugLogAction = null, Action<Exception> errorLogAction = null);
        string GetName();
        void Delete(long id);
    }
}
