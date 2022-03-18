using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Monitoring.Define
{
    public enum EComponentType : byte
    {
        Undefine = 0,
        Service = 1,
        Web = 2,
        Tool = 4,
        Host = 8,
    }
}
