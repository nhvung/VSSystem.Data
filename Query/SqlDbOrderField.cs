using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Query
{
    public class SqlDbOrderField
    {
        string _Field;
        public string Field { get { return _Field; } set { _Field = value; } }

        bool _IsASC;
        public bool IsASC { get { return _IsASC; } set { _IsASC = value; } }

    }
}
