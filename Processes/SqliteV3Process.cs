using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace VSSystem.Data
{
    public class SqliteV3Process : SqlProcess
    {
        string _dbFileName;
        protected int _poolSize;
        protected string _password;
        public SqliteV3Process(string dbFileName, string password, int commandTimeout, int poolSize = 10) : base(commandTimeout)
        {
            
            SQLiteConnectionStringBuilder builder = new SQLiteConnectionStringBuilder();
            builder.Version = 3;
            _dbFileName = dbFileName;
            if (string.IsNullOrEmpty(dbFileName))
            {
                builder.DataSource = ":memory:";
            }
            else
            {
                FileInfo dbFile = new FileInfo(dbFileName);
                if (!dbFile.Directory.Exists)
                {
                    dbFile.Directory.Create();
                }
                if (!dbFile.Exists)
                {
                    System.IO.File.WriteAllBytes(dbFile.FullName, new byte[0]);
                }
                builder.DataSource = dbFile.FullName;
            }

            //builder.Cache = SqliteCacheMode.Default;
            _password = password;
            if (!string.IsNullOrWhiteSpace(password))
            {
                //builder.Password = password;
            }
            if (poolSize > 0)
            {
                builder.Pooling = true;

            }

            _connectionString = builder.ToString();
            _connection = new SQLiteConnection(_connectionString);
            _database = _connection.Database;
            _provider = EDbProvider.Sqlite;
            _poolSize = poolSize;

        }

        public new SqliteV3Process Clone()
        {
            return new SqliteV3Process(_dbFileName, _password, _commandTimeout, _poolSize);
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
