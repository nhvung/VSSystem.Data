using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSSystem.Data.Query
{
    public class SqlDbTableQuery : SqlDbQuery
    {
        List<string> _SelectedFields;
        public List<string> SelectedFields { get { return _SelectedFields; } set { _SelectedFields = value; } }

        List<SqlDbOrderField> _OrderFields;
        public List<SqlDbOrderField> OrderFields { get { return _OrderFields; } set { _OrderFields = value; } }

        List<SqlDbFilterField> _FilterFields;
        public List<SqlDbFilterField> FilterFields { get { return _FilterFields; } set { _FilterFields = value; } }
        public SqlDbTableQuery(string name)
        {
            _Alias = "";
            _Name = name;
            _SelectedFields = new List<string>() { "*" };
            _OrderFields = new List<SqlDbOrderField>();
            _FilterFields = new List<SqlDbFilterField>();
        }

        public override string GetQuery()
        {
            return base.GetQuery();
        }

        public string GetFilters()
        {
            try
            {
                List<string> filters = new List<string>();
                if (_FilterFields?.Count > 0)
                {
                    _FilterFields.ForEach(ite => ite.Alias = _Alias);
                    filters = _FilterFields.Select(ite => ite.GetQuery()).ToList();
                }
                string result = filters.Count == 0 ? "" : "(" + string.Join(" AND ", filters) + ")";
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
