using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.Filters;

namespace VSSystem.Data.DAL
{
    public abstract class ADataDAL
    {
        protected EDbProvider _provider;
        protected ISqlPoolProcess _sqlProcess;
        public EDbProvider Provider { get { return _provider; } }
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
        protected virtual int ExecuteNonQuery(string query, System.Data.CommandType commandType)
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
        protected virtual int ExecuteNonQuery(List<string> queries, System.Data.CommandType commandType)
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
        protected int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, System.Data.CommandType.Text);
        }
        protected int ExecuteNonQuery(List<string> queries)
        {
            return ExecuteNonQuery(queries, System.Data.CommandType.Text);
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
        public virtual List<TStatResult> Calculate<TFilter, TStatResult>(TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            return new List<TStatResult>();
        }
        protected ADataDAL(ISqlPoolProcess sqlProcess)
        {
            _sqlProcess = sqlProcess;
            _provider = sqlProcess?.Provider ?? EDbProvider.NotDefine;
        }
    }
}
