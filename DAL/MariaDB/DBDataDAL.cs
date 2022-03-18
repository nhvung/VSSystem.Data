using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.DAL
{
    public class DBDataDAL
    {
        protected SqlPoolDbProcess _sqlProcess;
        protected EDbProvider _provider;
        protected DBDataDAL(SqlPoolDbProcess sqlProcess)
        {
            _sqlProcess = sqlProcess;
            _provider = sqlProcess.Provider;
        }
        protected virtual List<TResult> ExecuteReader<TResult>(string query)
        {
            try
            {
                List<TResult> result = _sqlProcess.ExecuteReader<TResult>(query);
                return result ?? new List<TResult>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual List<object> ExecuteReader(string query, Type dtoType)
        {
            try
            {
                List<object> result = _sqlProcess.ExecuteReader(query, dtoType);
                return result ?? new List<object>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual int ExecuteNonQuery(string query, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {

            try
            {
                int result = _sqlProcess.ExecuteNonQuery(query, commandType);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual int ExecuteNonQuery(List<string> queries, System.Data.CommandType commandType = System.Data.CommandType.Text)
        {

            try
            {
                int result = _sqlProcess.ExecuteNonQuery(queries, commandType);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual object ExecuteScalar(string query)
        {

            try
            {
                object result = _sqlProcess.ExecuteScalar(query);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual TResult ExecuteScalar<TResult>(string query)
        {

            try
            {
                TResult result = _sqlProcess.ExecuteScalar<TResult>(query);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected virtual int ExecuteInsert(string tableName, string[] fields, Type dataType, object[] records)
        {
            try
            {
                int exec = _sqlProcess.ExecuteInsertBase(tableName, fields, dataType, records);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected virtual int TruncateTable(string tableName)
        {
            try
            {
                int exec = _sqlProcess.TruncateTable(tableName);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
