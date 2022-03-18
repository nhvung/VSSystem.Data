
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VSSystem.Data
{
    public class SqliteProcess : SqlProcess
    {
        string _dbFileName;
        protected int _poolSize;
        protected string _password;
        public SqliteProcess(string dbFileName, string password, int commandTimeout, int poolSize = 10) : base(commandTimeout)
        {
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();            
            _dbFileName = dbFileName;
            if (string.IsNullOrEmpty(dbFileName))
            {
                builder.DataSource = ":memory:";
            }
            else
            {
                FileInfo dbFile = new FileInfo(dbFileName);
                if(!dbFile.Directory.Exists)
                {
                    dbFile.Directory.Create();
                }
                if(!dbFile.Exists)
                {
                    System.IO.File.WriteAllBytes(dbFile.FullName, new byte[0]);
                }
                builder.DataSource = dbFile.FullName;
            }
            _connectionString = builder.ToString();
            _connection = new SqliteConnection(_connectionString);
            _database = _connection.Database;
            _provider = EDbProvider.Sqlite;
            _poolSize = poolSize;

        }

        public new SqliteProcess Clone()
        {
            return new SqliteProcess(_dbFileName, _password, _commandTimeout, _poolSize);
        }
        public int ExecuteInsert(string tableName, string[] fields, Type dataType, params object[] records)
        {
            try
            {
                if (records?.Length > 0)
                {
                    if (fields == null || fields.Length == 0)
                    {
                        fields = _GetTableFields(tableName).ToArray();
                    }
                    else
                    {
                        string[] dataFields = _GetTableFields(tableName).ToArray();
                        fields = fields.Intersect(dataFields).Distinct().ToArray();
                    }
                    string insertFields = string.Join(",", fields.Select(ite => "[" + ite + "]"));
                    string dbValue = string.Join(",", SqlDbRecord.ToDbValues(fields, dataType, records));
                    string query = string.Format("insert into {0}({1}) values {2}", tableName, insertFields, dbValue);
                    int exec = ExecuteNonQuery(query);
                    return exec;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<string> _GetTableFields(string tableName)
        {
            try
            {
                string query = _provider == EDbProvider.SqlServer ? "select top 0 * from " + tableName : "select * from " + tableName + " limit 0";
                var result = GetFieldAndType(query)?.Keys.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
