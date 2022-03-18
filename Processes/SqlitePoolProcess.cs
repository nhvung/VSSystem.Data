using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlitePoolProcess : ISqlPoolProcess
    {
        public SqlitePoolProcess()
        {
        }
        public SqlitePoolProcess(string dbFileName, string password, int commandTimeout, int poolSize = 10)
        {
            _instanceProcess = new SqliteProcess(dbFileName, password, commandTimeout, poolSize);
        }
        protected SqliteProcess _instanceProcess;
        public EDbProvider Provider { get { return _instanceProcess.Provider; } }
        public string ConnectionString { get { return _instanceProcess?.ConnectionString; } }
        public string Database { get { return _instanceProcess.Database; } }
        public object ExecuteScalar(string query)
        {
            try
            {
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
                int value = sqlP.ExecuteInsert(tableName, fields, record);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string, string>[] mappingFields, TDbRecord record) where TDbRecord : SqlDbRecord
        {
            try
            {
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
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
                var sqlP = _instanceProcess.Clone();
                int value = sqlP.ExecuteUpdate(tableName, keyFields, updateFields, record);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ISqlPoolProcess Clone()
        {
            return Clone<SqlitePoolProcess>();
        }
        public TSqlPoolProcess Clone<TSqlPoolProcess>() where TSqlPoolProcess : SqlitePoolProcess
        {

            try
            {
                TSqlPoolProcess result = Activator.CreateInstance<TSqlPoolProcess>();
                result._instanceProcess = _instanceProcess.Clone();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
