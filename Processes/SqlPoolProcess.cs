using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlPoolProcess : ISqlPoolProcess
    {
        protected SqlDbProcess _instanceProcess;
        public EDbProvider Provider { get { return _instanceProcess.Provider; } }

        protected string _SqlServer;
        public string SqlServer { get { return _SqlServer; } set { _SqlServer = value; } }

        protected string _SqlUser;
        public string SqlUser { get { return _SqlUser; } set { _SqlUser = value; } }
        public string Database { get { return _instanceProcess.Database; } }

        string _Password;
        public string Password { get { return _Password; } set { _Password = value; } }

        public SqlPoolProcess(string server, string username, string password, int port, string database, int connectionTimeout, int commandTimeout, string driver, int minPoolSize = 10, int maxPoolSize = 10
            , Action<string> debugLogAction = null)
        {
            _SqlServer = server;
            _SqlUser = username;
            _Password = password;
            _instanceProcess = SqlProcess.CreateInstance<SqlDbProcess>(server, username, password, port, database, connectionTimeout, commandTimeout, driver, true, minPoolSize, maxPoolSize, debugLogAction);
        }
        public SqlPoolProcess() { }

        public override string ToString()
        {
            return _instanceProcess?.ToString() ?? "No Connection";
        }


        public string ConnectionString { get { return _instanceProcess?.ConnectionString; } }

        public TSqlPoolProcess Clone<TSqlPoolProcess>() where TSqlPoolProcess : SqlPoolProcess
        {

            try
            {
                TSqlPoolProcess result = Activator.CreateInstance<TSqlPoolProcess>();
                result._instanceProcess = _instanceProcess.Clone<SqlDbProcess>();
                result.Password = _Password;
                result.SqlUser = _SqlUser;
                result.SqlServer = _SqlServer;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public object ExecuteScalar(string query)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                object value = sqlP.ExecuteScalar(query);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public TResult ExecuteScalar<TResult>(string query)
        {

            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                TResult value = sqlP.ExecuteScalar<TResult>(query);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteNonQuery(string query, System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure, Action<Exception> errorLogAction = null)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteNonQuery(query, commandType, errorLogAction);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteNonQuery(List<string> queries, System.Data.CommandType commandType, Action<Exception> errorLogAction = null)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteNonQuery(queries, commandType, errorLogAction);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TResult> ExecuteReader<TResult>(string query)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                List<TResult> values = sqlP.ExecuteReader<TResult>(query);
                return values;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<object> ExecuteReader(string query, Type dtoType)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                List<object> values = sqlP.ExecuteReader(query, dtoType);
                return values;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public SqlDbResult ExecuteReader(string query)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                SqlDbResult values = sqlP.ExecuteReader(query);
                return values;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, string[] fields, TDbRecord record) where TDbRecord : SqlDbRecord
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteInsert(tableName, fields, record);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string,string>[] mappingFields, TDbRecord record) where TDbRecord : SqlDbRecord
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteInsert(tableName, mappingFields, record);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, string[] fields, List<TDbRecord> records) where TDbRecord : SqlDbRecord
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteInsert(tableName, fields, records);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string, string>[] mappingFields, List<TDbRecord> records) where TDbRecord : SqlDbRecord
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteInsert(tableName, mappingFields, records);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public int ExecuteInsertBase(string tableName, string[] fields, Type dataType, params object[] records)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteInsert(tableName, fields, dataType, records);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int ExecuteUpdate<TDbRecord>(string tableName, string[] keyFields, string[] updateFields, List<TDbRecord> records, int updateBlockSize = 50) where TDbRecord : SqlDbRecord
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteUpdate(tableName, keyFields, updateFields, records, updateBlockSize);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteUpdate<TDbRecord>(string tableName, string[] keyFields, string[] updateFields, TDbRecord record) where TDbRecord : SqlDbRecord
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                int value = sqlP.ExecuteUpdate(tableName, keyFields, updateFields, record);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SqlDbStatement> GetAllDatabaseStatements()
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetAllDatabaseStatements();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SqlDbTable> GetTables()
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetTables();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ISqlPoolProcess Clone()
        {
            return Clone<SqlPoolProcess>();
        }
    }
}
