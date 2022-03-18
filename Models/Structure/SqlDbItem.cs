using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbItem
    {
        protected string _name;
        public string Name { get { return _name; } }
        public virtual string[] ToSqlArrayString(EDbProvider provider) { return new string[0]; }
        public virtual string ToSqlString(EDbProvider provider) { return string.Empty; }
        public override string ToString()
        {
            return _name;
        }
    }
}
