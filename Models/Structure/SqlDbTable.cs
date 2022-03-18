using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbTable : SqlDbItem
    {
        public SqlDbTable(string name, SqlDbColumn[] columns, SqlDbIndex[] indexes)
        {
            _name = name;
            _columns = columns.ToList();
            _indexes = indexes.ToList();
        }
        public SqlDbTable(string name)
        {
            _name = name;
            _columns = new List<SqlDbColumn>();
            _indexes = new List<SqlDbIndex>();
        }
        List<SqlDbColumn> _columns;
        public List<SqlDbColumn> Columns { get { return _columns; } }
        List<SqlDbIndex> _indexes;
        public void AddColumn(string columnName, ESqlDbDataType eType, long length, bool isPrimaryKey = false, bool isNullable = true, bool isIdentity = false)
        {
            SqlDbColumn column = new SqlDbColumn(columnName, eType, length, isPrimaryKey, isNullable, isIdentity);
            _columns.Add(column);
        }
        public void AddColumn(string columnName, ESqlDbDataType eType, long nPrecision, long nScale, bool isPrimaryKey = false, bool isNullable = true, bool isIdentity = false)
        {
            SqlDbColumn column = new SqlDbColumn(columnName, eType, nPrecision, nScale, isPrimaryKey, isNullable, isIdentity);
            _columns.Add(column);
        }
        public void AddIndex(string[] columnNames, bool isUnique = false)
        {
            if(_indexes == null)
            {
                _indexes = new List<SqlDbIndex>();
            }
            _indexes.Add(new SqlDbIndex(columnNames, isUnique));
        }
        public override string ToSqlString(EDbProvider provider)
        {
            string[] arrStr = ToSqlArrayString(provider);
            string result = string.Join(" ", arrStr);
            return result;
        }
        public override string[] ToSqlArrayString(EDbProvider provider)
        {
            return ToSqlString(provider, false);
        }
        public string[] ToSqlArrayString(EDbProvider provider, bool overwrite)
        {
            return ToSqlString(provider, overwrite);
        }
        public string[] ToSqlString(EDbProvider provider, bool overwrite)
        {
            List<string> result = new List<string>();

            result.AddRange(overwrite 
                ? _overwriteTableSQLString(provider) 
                : _notoverwriteTableSQLString(provider));
            result.Add("(");
            List<string> columnStrings = _columns.Select(ite => ite.ToSqlString(provider)).ToList();
            string[] keyColumns = _columns.Where(ite => ite.IsPrimaryKey).Select(ite => ite.Name).ToArray();
            string keySql = keyColumns.Length > 0 ? "constraint pk_" + _name + " primary key (" + string.Join(", ", keyColumns) + ")" : "";
            if (!string.IsNullOrEmpty(keySql)) columnStrings.Add(keySql);
            result.AddRange(columnStrings.Select((ite, idx) => { return idx < columnStrings.Count - 1 ? ite + "," : ite; }));
            result.Add(");");
            if (_indexes.Count > 0)
            {
                result.Add(Environment.NewLine);
                result.AddRange(_indexes.Select((ite, idx) => ite.ToSqlString(_name, idx) + Environment.NewLine));
            }
            return result.ToArray();
        }

        public string[] ToClass(string className = "", string nameSpace = "", bool includeUsingDefine = true, string parentClass = "")
        {
            try
            {
                string propertyLevel = "\t", contentLevel = "\t\t", classLevel = "";

                List<string> lines = new List<string>();

                if (includeUsingDefine) lines.Add("using System;");
                if (!string.IsNullOrEmpty(nameSpace)) { lines.AddRange(new string[] { "namespace " + nameSpace, "{" }); classLevel += "\t"; propertyLevel += "\t"; contentLevel += "\t"; }
                lines.Add(classLevel + "public class " + (!string.IsNullOrEmpty(className) ? className : _name) + (string.IsNullOrEmpty(parentClass) ? "" : " : " + parentClass));
                lines.Add(classLevel + "{");
                List<string> initLines = new List<string>() { propertyLevel + "void InitializeValues()", propertyLevel + "{" }, propertyLines = new List<string>();
                foreach (SqlDbColumn column in _columns)
                {
                    var fields = column.ToClassField();
                    propertyLines.Add(propertyLevel + fields[SqlDbColumn.LineType.PRIVATELINE]);
                    propertyLines.Add(propertyLevel + fields[SqlDbColumn.LineType.PUBLICLINE]);
                    initLines.Add(contentLevel + fields[SqlDbColumn.LineType.INITLINE]);
                }
                initLines.Add(propertyLevel + "}");
                lines.AddRange(propertyLines);
                lines.AddRange(initLines);

                lines.Add(string.Format(propertyLevel + "public {0}() {{ InitializeValues(); }}", !string.IsNullOrEmpty(className) ? className : _name));

                lines.Add(classLevel + "}");
                if (!string.IsNullOrEmpty(nameSpace)) lines.Add("}");
                return lines.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override string ToString()
        {
            return _name;
        }
        string[] _overwriteTableSQLString(EDbProvider provider)
        {
            string[] result = provider == EDbProvider.SqlServer ? new string[] { string.Format("if object_id('{0}') is not null drop table {0};", _name), string.Format("create table {0}", _name) } : new string[] { string.Format("drop table if exists {0};", _name), string.Format("create table {0}", _name) };
            return result;
        }

        string[] _notoverwriteTableSQLString(EDbProvider provider)
        {
            string[] result = provider == EDbProvider.SqlServer ? new string[] { string.Format("if object_id('{0}') is null create table {0}", _name) } : new string[] { string.Format("create table if not exists {0}", _name) };
            return result;
        }

    }
}
