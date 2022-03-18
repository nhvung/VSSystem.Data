using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSSystem.Data.Query
{
    public class SqlDbFilterField : SqlDbQuery
    {
        public class SqlDbCompareType
        {
            public const string NOTEQUALTO = "<>";
            public const string EQUALS = "=";
            public const string LESSTHAN = "<";
            public const string LESSTHANOREQUALTO = "<=";
            public const string GREATERTHAN = ">";
            public const string GREATERTHANOREQUALTO = ">=";

            public const string LEFTLIKE = "LEFTLIKE";
            public const string RIGHTLIKE = "RIGHTLIKE";
            public const string FULLLIKE = "FULLLIKE";
        }
        List<object> _Values;
        public List<object> Values { get { return _Values; } set { _Values = value; } }

        string _CompareType;
        public string CompareType { get { return _CompareType; } set { _CompareType = value; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Field Name</param>
        /// <param name="compareType">Operator for comparing or FULLLIKE (%Value%), LEFTLIKE (%Value), RIGHTLIKE (Value%) </param>
        /// <param name="values"></param>
        public SqlDbFilterField(string name, string compareType, List<object> values)
        {
            _Name = name;
            _CompareType = compareType.Trim(' ');
            _Values = values;
        }

        public override string GetQuery()
        {
            try
            {
                List<string> filters = GetQueries();
                string result = filters.Count == 0 ? "" : "(" + string.Join(" OR ", filters) + ")";
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override List<string> GetQueries()
        {
            try
            {
                string alias = string.IsNullOrEmpty(_Alias) ? "" : _Alias + ".";
                List<string> result = new List<string>();
                string nullFilter = _Values.Contains(null) ? alias + _Name + " IS NULL" : "";

                if (nullFilter != "") result.Add(nullFilter);

                string valueFormat = _CompareType.Equals(SqlDbCompareType.FULLLIKE, StringComparison.InvariantCultureIgnoreCase) ? "'%{0}%'"
                    : _CompareType.Equals(SqlDbCompareType.LEFTLIKE, StringComparison.InvariantCultureIgnoreCase) ? "'%{0}'"
                    : _CompareType.Equals(SqlDbCompareType.RIGHTLIKE, StringComparison.InvariantCultureIgnoreCase) ? "'{0}%'"
                    : "'{0}'";

                string[] values = _Values.Where(ite => ite != null).Select(ite => string.Format(valueFormat, ite.ToString().Replace("\\", "\\\\").Replace("'", "''"))).ToArray();

                if (values.Length > 0)
                {
                    if (_CompareType.Equals(SqlDbCompareType.EQUALS, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (values.Length == 1)
                        {
                            result.Add(string.Format("{0}{1} = {2}", alias, _Name, values[0]));
                        }
                        else
                        {
                            result.Add(string.Format("{0}{1} in ({2})", alias, _Name, string.Join(", ", values)));
                        }
                    }
                    else
                    {
                        string compareType = _CompareType.IndexOf("like", StringComparison.InvariantCultureIgnoreCase) >= 0 ? " LIKE " : _CompareType;
                        string[] fValues = values.Select(ite => string.Format("{0}{1} {2} {3}", alias, _Name, compareType, ite)).ToArray();
                        result.AddRange(fValues);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
