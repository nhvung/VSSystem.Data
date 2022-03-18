using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DTO;
using VSSystem.Data.Filters;

namespace VSSystem.Data.DAL
{
    public class DBDataDAL<TDTO> : DBDataDAL where TDTO : DataDTO
    {
        protected string _TableName;
        protected string _CreateTableStatements = string.Empty;
        protected bool _AutoCreateTable;
        public DBDataDAL() : base(null)
        {
            _TableName = "Data";
            _CreateTableStatements = string.Empty;
            _AutoCreateTable = false;
        }
        public DBDataDAL(SqlPoolDbProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = "Data";
            _CreateTableStatements = string.Empty;
            _AutoCreateTable = false;
        }
        List<string> _getTypeFields()
        {
            Type type = typeof(TDTO);
            return type.GetProperties().Select(ite => ite.Name).ToList();
        }
        public virtual int Insert(TDTO dbObj)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                List<string> fields = _getTypeFields();
                int exec = _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), dbObj);
                return exec;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                else if (ex.Message.IndexOf("Invalid column name", StringComparison.InvariantCultureIgnoreCase) >= 0 || ex.Message.IndexOf("Unknown column ", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    return InsertRetry(dbObj);
                throw ex;
            }
        }
        public virtual int Insert(List<TDTO> dbObjs)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                List<string> fields = _getTypeFields();
                int exec = _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), dbObjs);
                return exec;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                else if (ex.Message.IndexOf("Invalid column name", StringComparison.InvariantCultureIgnoreCase) >= 0 || ex.Message.IndexOf("Unknown column ", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    return InsertRetry(dbObjs);
                throw ex;
            }
        }

        int InsertRetry(TDTO dbObj)
        {
            try
            {
                string getDbFieldQuery = _provider == EDbProvider.SqlServer
                    ? "select column_name from information_schema.columns where table_name = '" + _TableName + "' and table_catalog=db_name()"
                    : "select column_name from information_schema.columns where table_name = '" + _TableName + "' and table_schema = database()";
                List<string> fields = ExecuteReader<string>(getDbFieldQuery);
                int exec = _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), dbObj);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        int InsertRetry(List<TDTO> dbObjs)
        {
            try
            {
                string getDbFieldQuery = _provider == EDbProvider.SqlServer
                       ? "select column_name from information_schema.columns where table_name = '" + _TableName + "' and table_catalog=db_name()"
                       : "select column_name from information_schema.columns where table_name = '" + _TableName + "' and table_schema = database()";
                List<string> fields = ExecuteReader<string>(getDbFieldQuery);
                int exec = _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), dbObjs);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TDTO> Search<TFilter>(TFilter filter) where TFilter : BaseFilter
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string sFilter = filter.GetFilterQuery();
                string query = string.Format("select * from {0} {1} {2}", _TableName, filter.Alias, sFilter);

                if (filter.FromIndex > 0 && filter.ToIndex > 0)
                {
                    if (_provider == EDbProvider.MariaDB || _provider == EDbProvider.Mysql)
                    {
                        int offset = filter.FromIndex - 1;
                        int limitSize = filter.ToIndex - filter.FromIndex + 1;
                        query += " limit " + limitSize + " offset " + offset;
                    }

                }

                List<TDTO> result = ExecuteReader<TDTO>(query);
                return result ?? new List<TDTO>();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }
        public virtual List<TDTO> SearchWithOrder<TFilter>(TFilter filter, string[] orderFields) where TFilter : BaseFilter
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string sFilter = filter.GetFilterQuery();
                string query = string.Format("select * from {0} {1} {2}", _TableName, filter.Alias, sFilter);

                if (orderFields?.Length > 0)
                {
                    List<string> orderStatements = new List<string>();
                    foreach (var orderField in orderFields)
                    {
                        if (!string.IsNullOrEmpty(orderField))
                        {
                            orderStatements.Add(string.Format("{0}{1}", string.IsNullOrEmpty(filter.Alias) ? "" : filter.Alias + ".", orderField));
                        }
                    }
                    if (orderStatements?.Count > 0)
                    {
                        string orderQuery = " order by " + string.Join(", ", orderStatements);
                        query += orderQuery;
                    }
                }

                if (filter.FromIndex > 0 && filter.ToIndex > 0)
                {
                    if (_provider == EDbProvider.MariaDB || _provider == EDbProvider.Mysql)
                    {
                        int offset = filter.FromIndex - 1;
                        int limitSize = filter.ToIndex - filter.FromIndex + 1;
                        query += " limit " + limitSize + " offset " + offset;
                    }
                }


                List<TDTO> result = ExecuteReader<TDTO>(query);
                return result ?? new List<TDTO>();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }
        public virtual List<TResult> Search<TResult, TFilter>(TFilter filter)
            where TFilter : BaseFilter
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string sFilter = filter.GetFilterQuery();
                string query = string.Format("select * from {0} {1} {2}", _TableName, filter.Alias, sFilter);
                if (filter.FromIndex > 0 && filter.ToIndex > 0)
                {
                    if (_provider == EDbProvider.MariaDB || _provider == EDbProvider.Mysql)
                    {
                        int offset = filter.FromIndex - 1;
                        int limitSize = filter.ToIndex - filter.FromIndex + 1;
                        query += " limit " + limitSize + " offset " + offset;
                    }
                }
                List<TResult> result = ExecuteReader<TResult>(query);
                return result ?? new List<TResult>();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }

        public virtual List<TResult> SearchIDs<TResult, TFilter>(TFilter filter, string[] resultFields)
           where TFilter : BaseFilter
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string selectFields = resultFields?.Length > 0 ? string.Join(", ", resultFields.Select(f => string.Format("{0}{1}",
                    string.IsNullOrEmpty(filter.Alias) ? "" : filter.Alias + ".",
                    f))) : "*";
                string sFilter = filter.GetFilterQuery();
                string query = string.Format("select {0} from {1} {2} {3}", selectFields, _TableName, filter.Alias, sFilter);
                if (filter.FromIndex > 0 && filter.ToIndex > 0)
                {
                    if (_provider == EDbProvider.MariaDB || _provider == EDbProvider.Mysql)
                    {
                        int offset = filter.FromIndex - 1;
                        int limitSize = filter.ToIndex - filter.FromIndex + 1;
                        query += " limit " + limitSize + " offset " + offset;
                    }
                }
                List<TResult> result = ExecuteReader<TResult>(query);
                return result ?? new List<TResult>();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }

        public virtual List<TDTO> GetAllData()
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string query = "select * from " + _TableName;
                List<TDTO> result = ExecuteReader<TDTO>(query);
                return result ?? new List<TDTO>();
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }

        public virtual int Truncate()
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string query = "truncate table " + _TableName;
                int exec = ExecuteNonQuery(query);
                return exec;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }


        public int ExecuteUpdate(string[] keyFields, string[] updateFields, TDTO dbObj)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                int exec = _sqlProcess.ExecuteUpdate(_TableName, keyFields, updateFields, dbObj);
                return exec;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }
        public int ExecuteUpdate(string[] keyFields, string[] updateFields, List<TDTO> dbObjs, int updateBlockSize = 50)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                int exec = _sqlProcess.ExecuteUpdate(_TableName, keyFields, updateFields, dbObjs, updateBlockSize);
                return exec;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }

        public TResult GetMaxValue<TResult>(string field)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string query = string.Format("select max({0}) from {1}", field, _TableName);
                TResult result = ExecuteScalar<TResult>(query);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }

        public TResult GetMinValue<TResult>(string field)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string query = string.Format("select min({0}) from {1}", field, _TableName);
                TResult result = ExecuteScalar<TResult>(query);
                return result;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }

        public virtual int GetTotal<TFilter>(TFilter filter) where TFilter : BaseFilter
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string searchFt = filter.GetFilterQuery();
                string query = string.Format("select count(1) from {0} {1} {2}", _TableName, filter.Alias, searchFt);
                int total = ExecuteScalar<int>(query);
                return total;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }
        public int CreateTable()
        {
            try
            {
                if (!string.IsNullOrEmpty(_CreateTableStatements))
                {
                    string query = string.Format(_CreateTableStatements, _TableName);
                    return ExecuteNonQuery(query);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override List<TResult> ExecuteReader<TResult>(string query)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                return base.ExecuteReader<TResult>(query);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }
        }
        protected override List<object> ExecuteReader(string query, Type dtoType)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                return base.ExecuteReader(query, dtoType);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }

        }
        protected override int ExecuteNonQuery(List<string> queries, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                return base.ExecuteNonQuery(queries, commandType);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }

        }
        protected override int ExecuteNonQuery(string query, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                return base.ExecuteNonQuery(query, commandType);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }

        }
        protected override object ExecuteScalar(string query)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                return base.ExecuteScalar(query);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }

        }
        protected override TResult ExecuteScalar<TResult>(string query)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                return base.ExecuteScalar<TResult>(query);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    if (_AutoCreateTable && !retryCreateTable)
                    {
                        CreateTable();
                        retryCreateTable = true;
                        goto RETRY;
                    }
                }
                throw ex;
            }

        }
    }
}
