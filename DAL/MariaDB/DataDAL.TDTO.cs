﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DTO;
using VSSystem.Data.Filters;

namespace VSSystem.Data.DAL
{
    public class DataDAL<TDTO> : ADataDAL<TDTO> where TDTO : DataDTO
    {
        public DataDAL(SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = "Data";
            _CreateTableStatements = string.Empty;
            _AutoCreateTable = false;
        }
        public DataDAL(ISqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = "Data";
            _CreateTableStatements = string.Empty;
            _AutoCreateTable = false;
        }
        public DataDAL(string tableName, SqlPoolProcess sqlProcess) : base(tableName, sqlProcess)
        {
            _CreateTableStatements = string.Empty;
            _AutoCreateTable = false;
        }
        public DataDAL(string tableName, ISqlPoolProcess sqlProcess) : base(tableName, sqlProcess)
        {
            _CreateTableStatements = string.Empty;
            _AutoCreateTable = false;
        }
        #region Insert
        public override int Insert(TDTO dbObj)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                List<string> fields = _GetTypeFields();
                int exec = _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), dbObj);
                return exec;
            }
            catch (Exception ex)
            {
                if (_IsTableExistsException(ex))
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
        public override int Insert(List<TDTO> dbObjs)
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                int exec = 0;
                List<string> fields = _GetTypeFields();
                var exeObjs = dbObjs.ToList();
                while(exeObjs.Count > 0)
                {
                    var tObjs = exeObjs.Take(_insertBlockSize).ToList();
                    exec += _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), tObjs);
                    exeObjs = exeObjs.Skip(_insertBlockSize).ToList();
                }
                return exec;
            }
            catch (Exception ex)
            {
                if (_IsTableExistsException(ex))
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

        protected override int InsertRetry(TDTO dbObj)
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
        protected override int InsertRetry(List<TDTO> dbObjs)
        {
            try
            {
                string getDbFieldQuery = _provider == EDbProvider.SqlServer
                       ? "select column_name from information_schema.columns where table_name = '" + _TableName + "' and table_catalog=db_name()"
                       : "select column_name from information_schema.columns where table_name = '" + _TableName + "' and table_schema = database()";
                List<string> fields = ExecuteReader<string>(getDbFieldQuery);
                int exec = 0;
               var exeObjs = dbObjs.ToList();
                while (exeObjs.Count > 0)
                {
                    var tObjs = exeObjs.Take(_insertBlockSize).ToList();
                    exec += _sqlProcess.ExecuteInsert(_TableName, fields.ToArray(), tObjs);
                    exeObjs = exeObjs.Skip(_insertBlockSize).ToList();
                }
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region Search
        public override List<TDTO> Search<TFilter>(TFilter filter)
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
                if (_IsTableExistsException(ex))
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
        public override List<TDTO> SearchWithOrder<TFilter>(TFilter filter, string[] orderFields)
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
                    int offset = filter.FromIndex - 1;
                    int limitSize = filter.ToIndex - filter.FromIndex + 1;
                    query += " limit " + limitSize + " offset " + offset;
                }
                List<TDTO> result = ExecuteReader<TDTO>(query);
                return result ?? new List<TDTO>();
            }
            catch (Exception ex)
            {
                if (_IsTableExistsException(ex))
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
        public override List<TDTO> SearchWithOrder<TFilter>(TFilter filter, List<KeyValuePair<string, string>> selectedFields, List<string> orderFields)
        {

            bool retryCreateTable = false;
        RETRY:
            try
            {
                string sFilter = filter.GetFilterQuery();

                List<string> tSelectedFields = new List<string>();
                if(selectedFields?.Count > 0)
                {
                    foreach(var field in selectedFields)
                    {
                        if(!string.IsNullOrWhiteSpace(field.Key))
                        {
                            if(!string.IsNullOrWhiteSpace(field.Value))
                            {
                                tSelectedFields.Add($"{field.Key} as {field.Value}");
                            }
                            else
                            {
                                tSelectedFields.Add($"{field.Key}");
                            }
                        }
                    }
                }
                
                string query = string.Format("select * from {0} {1} {2}", _TableName, filter.Alias, sFilter);
                if(tSelectedFields.Count > 0)
                {
                    string sFields = string.Join(", ", tSelectedFields);
                    query = string.Format("select {3} from {0} {1} {2}", _TableName, filter.Alias, sFilter, sFields);
                }

                if (orderFields?.Count > 0)
                {
                    List<string> orderStatements = new List<string>();
                    foreach (var orderField in orderFields)
                    {
                        if (!string.IsNullOrWhiteSpace(orderField))
                        {
                            orderStatements.Add(string.Format("{0}{1}", string.IsNullOrWhiteSpace(filter.Alias) ? "" : filter.Alias + ".", orderField));
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
                    int offset = filter.FromIndex - 1;
                    int limitSize = filter.ToIndex - filter.FromIndex + 1;
                    query += " limit " + limitSize + " offset " + offset;
                }
                List<TDTO> result = ExecuteReader<TDTO>(query);
                return result ?? new List<TDTO>();
            }
            catch (Exception ex)
            {
                if (_IsTableExistsException(ex))
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
        public override List<TResult> Search<TResult, TFilter>(TFilter filter)
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
                if (_IsTableExistsException(ex))
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
        public override List<TResult> SearchIDs<TResult, TFilter>(TFilter filter, string[] resultFields)
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
                if (_IsTableExistsException(ex))
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
        #endregion

        public override List<TDTO> GetAllData()
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
                if (_IsTableExistsException(ex))
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
        public override int Truncate()
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
                if (_IsTableExistsException(ex))
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

        #region Update
        public override int ExecuteUpdate(string[] keyFields, string[] updateFields, TDTO dbObj)
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
                if (_IsTableExistsException(ex))
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
        public override int ExecuteUpdate(string[] keyFields, string[] updateFields, List<TDTO> dbObjs, int updateBlockSize = 50)
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
                if (_IsTableExistsException(ex))
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
        #endregion

        #region Value
        public override TResult GetMaxValue<TResult>(string field)
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
                if (_IsTableExistsException(ex))
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
        public override TResult GetMinValue<TResult>(string field)
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
                if (_IsTableExistsException(ex))
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
        public override int GetTotal<TFilter>(TFilter filter)
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
                if (_IsTableExistsException(ex))
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
        #endregion
        
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
                if (_IsTableExistsException(ex))
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
                if (_IsTableExistsException(ex))
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
                if (_IsTableExistsException(ex))
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
                if (_IsTableExistsException(ex))
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
                if (_IsTableExistsException(ex))
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
                if (_IsTableExistsException(ex))
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

        #region Stat


        public override List<TStatResult> Calculate<TFilter, TStatResult>(TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
            //where TFilter : BaseFilter
        {
            bool retryCreateTable = false;
        RETRY:
            try
            {
                string sFilter = filter.GetFilterQuery();

                string selectedQuery = string.Join(", ", statFields.Select(ite => string.IsNullOrWhiteSpace(ite.Value) ? $"{ite.Key}" : $"{ite.Key} as '{ite.Value}'"));
                string calculateQuery = string.Join(", ", calculateFields.Select(ite => string.IsNullOrWhiteSpace(ite.Value) ? $"{ite.Key}" : $"{ite.Key} as '{ite.Value}'"));
                string groupQuery = string.Join(", ", statFields.Select(ite => ite.Key));
                string orderQuery = string.Empty;
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
                        orderQuery = "order by " + string.Join(", ", orderStatements);
                    }
                }

                string query = $"select {selectedQuery}, {calculateQuery} " +
                    $"from {_TableName} {filter.Alias} " +
                    $"{sFilter} " +
                    $"group by {groupQuery} " +
                   $"{orderQuery}";

                List<TStatResult> result = ExecuteReader<TStatResult>(query);
                return result ?? new List<TStatResult>();
            }
            catch (Exception ex)
            {
                if (_IsTableExistsException(ex))
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

        #endregion

        protected override bool _IsTableExistsException(Exception ex)
        {
            return ex.Message.IndexOf(string.Format("Table '{0}.{1}' doesn't exist", _sqlProcess.Database, _TableName), StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        
    }
}
