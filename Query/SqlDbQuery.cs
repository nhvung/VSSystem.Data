using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.Query
{
    public class SqlDbQuery
    {
        protected string _Name;
        public string Name { get { return _Name; } set { _Name = value; } }
        protected string _Alias;
        public string Alias { get { return _Alias; } set { _Alias = value; } }
        public virtual string GetQuery() { return string.Empty; }
        public virtual List<string> GetQueries() { return new List<string>(); }
    }
}
