using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Monitoring.Define
{
    public enum ELogType : byte
    {
        None = 0,
        Info = 1,
        Debug = 2,
        Warning = 4,
        Error = 8,
    }
}
