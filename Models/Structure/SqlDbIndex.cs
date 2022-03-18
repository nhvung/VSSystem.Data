using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbIndex : SqlDbItem
    {
        string[] _columnNames;
        bool _isUnique;
        public SqlDbIndex(string[] columnNames, bool isUnique)
        {
            _columnNames = columnNames;
            _isUnique = isUnique;
        }

        public string ToSqlString(string tableName, int idx = 0)
        {
            string result = string.Empty;
            if (_columnNames.Length > 0)
            {
                try
                {
                    if (_isUnique)
                        result = "create unique index uidx_" + tableName + "_" + idx + " on " + tableName + " (";
                    else
                        result = "create index idx_" + tableName + "_" + idx + " on " + tableName + " (";
                    result += string.Join(",", _columnNames);
                    result += ");" + Environment.NewLine;
                }
                catch { }
            }
            return result;
        }
    }
}
