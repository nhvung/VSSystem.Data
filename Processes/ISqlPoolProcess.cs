using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public interface ISqlPoolProcess
    {
        EDbProvider Provider { get; }
        string ConnectionString { get; }
        string Database { get; }
        object ExecuteScalar(string query);
        TResult ExecuteScalar<TResult>(string query);
        int ExecuteNonQuery(string query, System.Data.CommandType commandType = System.Data.CommandType.StoredProcedure, Action<Exception> errorLogAction = null);
        int ExecuteNonQuery(List<string> queries, System.Data.CommandType commandType, Action<Exception> errorLogAction = null);
        List<TResult> ExecuteReader<TResult>(string query);
        List<object> ExecuteReader(string query, Type dtoType);
        SqlDbResult ExecuteReader(string query);
        int ExecuteInsert<TDbRecord>(string tableName, string[] fields, TDbRecord record) where TDbRecord : SqlDbRecord;
        int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string, string>[] mappingFields, TDbRecord record) where TDbRecord : SqlDbRecord;
        int ExecuteInsert<TDbRecord>(string tableName, string[] fields, List<TDbRecord> records) where TDbRecord : SqlDbRecord;
        int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string, string>[] mappingFields, List<TDbRecord> records) where TDbRecord : SqlDbRecord;
        int ExecuteInsertBase(string tableName, string[] fields, Type dataType, params object[] records);
        int ExecuteUpdate<TDbRecord>(string tableName, string[] keyFields, string[] updateFields, List<TDbRecord> records, int updateBlockSize = 50) where TDbRecord : SqlDbRecord;
        int ExecuteUpdate<TDbRecord>(string tableName, string[] keyFields, string[] updateFields, TDbRecord record) where TDbRecord : SqlDbRecord;
        ISqlPoolProcess Clone();
    }
}
