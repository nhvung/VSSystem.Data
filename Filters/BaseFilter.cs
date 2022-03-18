using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSSystem.Data.Filters
{
    public class BaseFilter
    {
        protected const string EQUAL_TO = "=";
        protected const string NOT_EQUAL_TO = "<>";
        protected const string GREATER_THAN = ">";
        protected const string GREATER_THAN_OR_EQUAL_TO = ">=";
        protected const string LESS_THAN = "<";
        protected const string LESS_THAN_OR_EQUAL_TO = "<=";

        protected List<string> _arrayFilters;
        protected DateTime _BeginTime;
        public DateTime BeginTime { get { return _BeginTime; } set { _BeginTime = value; } }

        protected DateTime _EndTime;
        public DateTime EndTime { get { return _EndTime; } set { _EndTime = value; } }

        protected int _FromIndex;
        /// <summary>
        /// Begin at 1.
        /// </summary>
        public int FromIndex { get { return _FromIndex < 0 ? 0 : _FromIndex; } set { _FromIndex = value; } }

        protected int _ToIndex;
        public int ToIndex { get { return _ToIndex < _FromIndex ? _FromIndex : _ToIndex; } set { _ToIndex = value; } }

        protected int _LastRecord;
        public int LastRecord { get { return _LastRecord; } set { _LastRecord = value; } }

        protected int _FirstRecord;
        public int FirstRecord { get { return _FirstRecord; } set { _FirstRecord = value; } }

        protected string _Alias;
        public string Alias { get { return _Alias; } set { _Alias = value; } }
        //2020-05-11 @Vung

        protected long _LongBeginDateTime;
        public long LongBeginDateTime { get { return _LongBeginDateTime; } set { _LongBeginDateTime = value; } }

        protected long _LongEndDateTime;
        public long LongEndDateTime { get { return _LongEndDateTime; } set { _LongEndDateTime = value; } }

        protected int _IntBeginDate;
        public int IntBeginDate { get { return _IntBeginDate; } set { _IntBeginDate = value; } }

        protected int _IntEndDate;
        public int IntEndDate { get { return _IntEndDate; } set { _IntEndDate = value; } }
        // <=
        public BaseFilter()
        {
            _arrayFilters = new List<string>();
            _FromIndex = 0;
            _ToIndex = 0;
            _BeginTime = DateTime.MinValue;
            _EndTime = DateTime.MinValue;
            _LongBeginDateTime = 0;
            _LongEndDateTime = 0;
            _IntBeginDate = 0;
            _IntEndDate = 0;
            _LastRecord = 0;
            _FirstRecord = 0;

        }
        protected virtual void InitFilters()
        {
            _arrayFilters = new List<string>();
        }
        public virtual string GetFilterQuery()
        {

            try
            {
                InitFilters();
                string query = "";
                _arrayFilters = _arrayFilters.Where(ite => !string.IsNullOrEmpty(ite)).ToList();
                if (_arrayFilters != null && _arrayFilters.Count > 0)
                {
                    query = string.Format(" WHERE {0}", string.Join(" AND ", _arrayFilters));
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetFilter(string fieldName, params string[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues.Select(ite => "'" + ite.Replace("'", "''").Replace("\\", "\\\\") + "'"));
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilterLike(string fieldName, params string[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string[] values = filterValues.Select(ite => "'%" + ite.Replace("'", "''").Replace("\\", "\\\\") + "%'").ToArray();
            string result = "(" + string.Join(" or ", values.Select(ite => string.Format("{0} like {1}", fieldName, ite))) + ")";
            return result;
        }
        public static string GetFilterLike(string alias, string fieldName, params string[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string[] values = filterValues.Select(ite => "'%" + ite.Replace("'", "''").Replace("\\", "\\\\") + "%'").ToArray();
            string result = "(" + string.Join(" or ", values.Select(ite => string.IsNullOrEmpty(alias)
            ? string.Format("{0} like {1}", fieldName, ite)
            : string.Format("{0}.{1} like {2}", alias, fieldName, ite))) + ")";
            return result;
        }
        public static string GetFilter(string fieldName, params Guid[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues.Select(ite => "'" + ite + "'"));
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params byte[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params sbyte[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params short[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params ushort[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params int[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params uint[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params long[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params ulong[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params float[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params double[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params decimal[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        public static string GetFilter(string fieldName, params DateTime[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues.Select(ite => "'" + ite.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"));
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = '{0:yyyy-MM-dd HH:mm:ss.fff}'", filterValues[0]) : string.Format(" in ({0})", values));
            return result;
        }
        #region Alias
        public static string GetFilter(string alias, string fieldName, params string[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues.Select(ite => "'" + ite.Replace("'", "''").Replace("\\", "\\\\") + "'"));
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0].Replace("'", "''").Replace("\\", "\\\\")) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0].Replace("'", "''").Replace("\\", "\\\\")) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params Guid[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues.Select(ite => "'" + ite + "'"));
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params byte[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params sbyte[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params short[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params ushort[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params int[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params uint[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params long[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params ulong[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params float[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params double[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params decimal[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues);
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = {0}", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        public static string GetFilter(string alias, string fieldName, params DateTime[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            string values = string.Join(", ", filterValues.Select(ite => "'" + ite.ToString("yyyy-MM-dd HH:mm:ss.fff") + "'"));
            string result = string.Format("{0}{1}", fieldName, filterValues.Length == 1 ? string.Format(" = '{0:yyyy-MM-dd HH:mm:ss.fff}'", filterValues[0]) : string.Format(" in ({0})", values));
            if (!string.IsNullOrEmpty(alias))
            {
                result = string.Format("{0}.{1}{2}", alias, fieldName, filterValues.Length == 1 ? string.Format(" = '{0}'", filterValues[0]) : string.Format(" in ({0})", values));
            }
            return result;
        }
        #endregion

        #region Alias + Compare
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params string[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, "'" + ite.Replace("'", "''").Replace("\\", "\\\\") + "'")).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, "'" + ite.Replace("'", "''").Replace("\\", "\\\\") + "'")).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params Guid[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, "'" + ite.ToString() + "'")).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, "'" + ite.ToString() + "'")).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params byte[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params sbyte[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params short[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params ushort[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params int[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params uint[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params long[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params ulong[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params float[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params double[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params decimal[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} {2})", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} {3})", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        public static string GetFilterCompare(string alias, string fieldName, string compareString, params DateTime[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            List<string> ftValues = string.IsNullOrEmpty(alias)
                ? filterValues.Select(ite => string.Format("({0} {1} '{2:yyyy-MM-dd HH:mm:ss}')", fieldName, compareString, ite)).ToList()
                : filterValues.Select(ite => string.Format("({0}.{1} {2} '{3:yyyy-MM-dd HH:mm:ss}')", alias, fieldName, compareString, ite)).ToList();
            string result = "(" + string.Join(" or ", ftValues) + ")";
            return result;
        }
        #endregion
        public static string GetFilter<TSource>(string fieldName, params TSource[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            Type srcType = typeof(TSource);
            string result = "";
            if (srcType == typeof(string))
            {
                string[] newFtValues = filterValues.Select(ite => Convert.ToString(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(Guid))
            {
                Guid[] newFtValues = filterValues.Select(ite => Guid.Parse(ite.ToString())).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(byte))
            {
                byte[] newFtValues = filterValues.Select(ite => Convert.ToByte(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(sbyte))
            {
                sbyte[] newFtValues = filterValues.Select(ite => Convert.ToSByte(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(short))
            {
                short[] newFtValues = filterValues.Select(ite => Convert.ToInt16(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(ushort))
            {
                ushort[] newFtValues = filterValues.Select(ite => Convert.ToUInt16(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(int))
            {
                int[] newFtValues = filterValues.Select(ite => Convert.ToInt32(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(uint))
            {
                uint[] newFtValues = filterValues.Select(ite => Convert.ToUInt32(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(long))
            {
                long[] newFtValues = filterValues.Select(ite => Convert.ToInt64(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(ulong))
            {
                ulong[] newFtValues = filterValues.Select(ite => Convert.ToUInt64(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(float))
            {
                float[] newFtValues = filterValues.Select(ite => Convert.ToSingle(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(double))
            {
                double[] newFtValues = filterValues.Select(ite => Convert.ToDouble(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(decimal))
            {
                decimal[] newFtValues = filterValues.Select(ite => Convert.ToDecimal(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            else if (srcType == typeof(DateTime))
            {
                DateTime[] newFtValues = filterValues.Select(ite => Convert.ToDateTime(ite)).ToArray();
                result = GetFilter(fieldName, newFtValues);
            }
            return result;
        }

        public static string GetFilter<TSource>(string alias, string fieldName, params TSource[] filterValues)
        {
            if (filterValues == null || filterValues.Length == 0) return "";
            Type srcType = typeof(TSource);
            string result = "";
            if (srcType == typeof(string))
            {
                string[] newFtValues = filterValues.Select(ite => Convert.ToString(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(Guid))
            {
                Guid[] newFtValues = filterValues.Select(ite => Guid.Parse(ite.ToString())).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(byte))
            {
                byte[] newFtValues = filterValues.Select(ite => Convert.ToByte(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(short))
            {
                short[] newFtValues = filterValues.Select(ite => Convert.ToInt16(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(ushort))
            {
                ushort[] newFtValues = filterValues.Select(ite => Convert.ToUInt16(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(int))
            {
                int[] newFtValues = filterValues.Select(ite => Convert.ToInt32(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(uint))
            {
                uint[] newFtValues = filterValues.Select(ite => Convert.ToUInt32(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(long))
            {
                long[] newFtValues = filterValues.Select(ite => Convert.ToInt64(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(ulong))
            {
                ulong[] newFtValues = filterValues.Select(ite => Convert.ToUInt64(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(float))
            {
                float[] newFtValues = filterValues.Select(ite => Convert.ToSingle(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(double))
            {
                double[] newFtValues = filterValues.Select(ite => Convert.ToDouble(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(decimal))
            {
                decimal[] newFtValues = filterValues.Select(ite => Convert.ToDecimal(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(DateTime))
            {
                DateTime[] newFtValues = filterValues.Select(ite => Convert.ToDateTime(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            else if (srcType == typeof(sbyte))
            {
                sbyte[] newFtValues = filterValues.Select(ite => Convert.ToSByte(ite)).ToArray();
                result = GetFilter(alias, fieldName, newFtValues);
            }
            return result;
        }
    }
}
