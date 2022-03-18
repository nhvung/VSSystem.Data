using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlPoolDbProcess : SqlPoolProcess
    {
        public SqlPoolDbProcess(string server, string username, string password, int port, string database, int connectionTimeout, int commandTimeout, string driver, int minPoolSize = 10, int maxPoolSize = 10)
            : base(server, username, password, port, database, connectionTimeout, commandTimeout, driver, minPoolSize, maxPoolSize)
        {

        }
        public SqlPoolDbProcess() : base() { }
        public override string ToString()
        {
            return _instanceProcess?.ToString() ?? "No Connection";
        }

        public List<string> GetAllProcedureNames()
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetAllProcedureNames();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetAllTriggerNames()
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetAllTriggerNames();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<string> GetAllTableNames()
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetAllTableNames();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDbTable GetTable(string tableName)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetTable(tableName);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetTableFields(string tableName)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.GetTableFields(tableName);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int TruncateTable(string tableName)
        {
            try
            {
                SqlDbProcess sqlP = _instanceProcess.Clone<SqlDbProcess>();
                var value = sqlP.TruncateTable(tableName);
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
