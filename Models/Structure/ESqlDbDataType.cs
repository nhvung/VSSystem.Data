using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public enum ESqlDbDataType : int
    {
        Undefine = -1,
        Byte,
        Int16,
        Int32,
        Int64,
        Float,
        Double,
        Decimal,

        Date,
        DateTime,
        TimeStamp,

        Guid,

        String,
        UnicodeString,
        VString,
        VUnicodeString,
        Text,
        UnicodeText,

        Bytes,
        VBytes
    }
}
