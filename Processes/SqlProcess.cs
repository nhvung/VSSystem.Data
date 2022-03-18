using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using VSSystem.Data.Query;

namespace VSSystem.Data
{
    public class SqlProcess
    {
        protected string _driver;
        protected int _commandTimeout;
        protected EDbProvider _provider;
        public EDbProvider Provider { get { return _provider; } }
        protected IDbConnection _connection;
        protected string _connectionString;
        public string ConnectionString { get { return _connectionString; } }
        protected string _database;
        public string Database { get { return _database; } }
        protected SqlProcess() { }
        protected SqlProcess(int commandTimeout) { }
        public override string ToString()
        {
            return _connectionString;
        }
        SqlProcess(string server, string username, string password, int port, int connectionTimeout, int commandTimeout, string driver, bool usePool = false, int minPoolSize = 0, int maxPoolSize = 100)
        {

            try
            {
                _driver = driver;
                DbConnectionStringBuilder dbCSBuilder = new DbConnectionStringBuilder();
                dbCSBuilder.Add("server", server);
                dbCSBuilder.Add("uid", username);
                dbCSBuilder.Add("pwd", password);
                if (connectionTimeout > 0)
                {
                    dbCSBuilder.Add("Connection Timeout", connectionTimeout);
                }
                dbCSBuilder.Add("pwd", password);
                if (port > 0)
                {
                    dbCSBuilder.Add("port", port);
                }
                if (usePool)
                {
                    dbCSBuilder.Add("Pooling", "True");
                    dbCSBuilder.Add("Min Pool Size", minPoolSize);
                    dbCSBuilder.Add("Max Pool Size", maxPoolSize);
                }
                _connectionString = dbCSBuilder.ConnectionString;
                if (driver.IndexOf("mysql", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.Mysql;
                    _connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        _connectionString += string.Format("; driver={0}", driver);
                        _connection = new OdbcConnection(_connectionString);
                    }
                }
                else if (driver.IndexOf("mariadb", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.MariaDB;
                    _connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        _connectionString += string.Format("; driver={0}", driver);
                        _connection = new OdbcConnection(_connectionString);
                    }
                }
                else
                {
                    _provider = EDbProvider.SqlServer;
                    _connection = new SqlConnection(_connectionString);
                }
                _commandTimeout = commandTimeout;
                _database = "";
            }
            catch (Exception ex)
            {
                throw new Exception("driver: " + _driver, ex);
            }
        }

        SqlProcess(string server, string username, string password, int port, string database, int connectionTimeout, int commandTimeout, string driver, bool usePool = false, int minPoolSize = 0, int maxPoolSize = 100)
        {

            try
            {
                _driver = driver;
                _database = database;

                DbConnectionStringBuilder dbCSBuilder = new DbConnectionStringBuilder();
                dbCSBuilder.Add("server", server);
                dbCSBuilder.Add("uid", username);
                dbCSBuilder.Add("pwd", password);
                if (connectionTimeout > 0)
                {
                    dbCSBuilder.Add("Connection Timeout", connectionTimeout);
                }
                if (port > 0)
                {
                    dbCSBuilder.Add("port", port);
                }
                if (!string.IsNullOrEmpty(database))
                {
                    dbCSBuilder.Add("database", database);
                }
                if (usePool)
                {
                    dbCSBuilder.Add("Pooling", "True");
                    dbCSBuilder.Add("Min Pool Size", minPoolSize);
                    dbCSBuilder.Add("Max Pool Size", maxPoolSize);
                }

                _connectionString = dbCSBuilder.ConnectionString;
                if (driver.IndexOf("mysql", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.Mysql;
                    _connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        _connectionString += string.Format("; driver={0}", driver);
                        _connection = new OdbcConnection(_connectionString);
                    }
                }
                else if (driver.IndexOf("mariadb", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.MariaDB;
                    _connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        _connectionString += string.Format("; driver={0}", driver);
                        _connection = new OdbcConnection(_connectionString);
                    }
                }
                else
                {
                    _provider = EDbProvider.SqlServer;
                    _connection = new SqlConnection(_connectionString);
                }
                _commandTimeout = commandTimeout;
            }
            catch (Exception ex)
            {
                throw new Exception("driver: " + _driver, ex);
            }
        }
        public SqlProcess Clone()
        {
            try
            {
                SqlProcess result = new SqlProcess();
                if (_driver.IndexOf("mysql", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.Mysql;
                    result._connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        result._connection = new OdbcConnection(_connectionString);
                    }
                }
                else if (_driver.IndexOf("mariadb", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.MariaDB;
                    result._connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        result._connection = new OdbcConnection(_connectionString);
                    }
                }
                else
                {
                    _provider = EDbProvider.SqlServer;
                    result._connection = new SqlConnection(_connectionString);
                }
                result._connectionString = _connectionString;
                result._provider = _provider;
                result._driver = _driver;
                result._database = _database;
                result._commandTimeout = _commandTimeout;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TSqlProcess Clone<TSqlProcess>(Action<string> debugLogAction = null) where TSqlProcess : SqlProcess
        {
            try
            {
                TSqlProcess result = Activator.CreateInstance<TSqlProcess>();
                if (_driver.IndexOf("mysql", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.Mysql;
                    result._connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        result._connection = new OdbcConnection(_connectionString);
                    }
                }
                else if (_driver.IndexOf("mariadb", StringComparison.InvariantCultureIgnoreCase) >= 0)
                {
                    _provider = EDbProvider.MariaDB;
                    result._connection = new MySqlConnector.MySqlConnection(_connectionString);
                    if (Environment.OSVersion.VersionString?.IndexOf("windows", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        result._connection = new OdbcConnection(_connectionString);
                    }
                }
                else
                {
                    _provider = EDbProvider.SqlServer;
                    result._connection = new SqlConnection(_connectionString);
                }
                result._provider = _provider;
                result._commandTimeout = _commandTimeout;
                result._connectionString = _connectionString;
                result._database = _database;
                result._driver = _driver;
                debugLogAction?.Invoke(_connectionString);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region with port
        public static SqlProcess CreateInstance(string server, string username, string password, int port, int connectionTimeout, int commandTimeout, string driver, bool usePool = false, int minPoolSize = 10, int maxPoolSize = 10)
        {
            SqlProcess ins = new SqlProcess(server, username, password, port, connectionTimeout, commandTimeout, driver, usePool, minPoolSize, maxPoolSize);
            return ins;
        }
        public static SqlProcess CreateInstance(string server, string username, string password, int port, string database, int connectionTimeout, int commandTimeout, string driver, bool usePool = false, int minPoolSize = 10, int maxPoolSize = 10)
        {
            SqlProcess ins = new SqlProcess(server, username, password, port, database, connectionTimeout, commandTimeout, driver, usePool, minPoolSize, maxPoolSize);
            return ins;
        }

        public static TSqlProcess CreateInstance<TSqlProcess>(string server, string username, string password, int port, int connectionTimeout, int commandTimeout, string driver, bool usePool = false, int minPoolSize = 10, int maxPoolSize = 10) where TSqlProcess : SqlProcess
        {
            TSqlProcess ins = new SqlProcess(server, username, password, port, connectionTimeout, commandTimeout, driver, usePool, minPoolSize, maxPoolSize).Clone<TSqlProcess>();
            return ins;
        }

        public static TSqlProcess CreateInstance<TSqlProcess>(string server, string username, string password, int port, string database, int connectionTimeout, int commandTimeout, string driver, bool usePool = false, int minPoolSize = 10, int maxPoolSize = 10
            , Action<string> debugLogAction = null) 
            where TSqlProcess : SqlProcess
        {
            TSqlProcess ins = new SqlProcess(server, username, password, port, database, connectionTimeout, commandTimeout, driver, usePool, minPoolSize, maxPoolSize)
                .Clone<TSqlProcess>(debugLogAction);
            return ins;
        }
        #endregion

        public void ChangeDatabase(string databaseName)
        {
            try
            {
                _database = databaseName;
                _connection.ChangeDatabase(databaseName);
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
                if (string.IsNullOrEmpty(query)) return null;
                _connection.Open();
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                object value = cmd.ExecuteScalar();
                cmd.Dispose();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public TResult ExecuteScalar<TResult>(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return default(TResult);
                _connection.Open();
                Type rType = typeof(TResult);
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                object value = cmd.ExecuteScalar();
                if (value != null && value != DBNull.Value)
                {
                    TResult result = (TResult)Convert.ChangeType(value.ToString(), rType);
                    return result;
                }
                cmd.Dispose();
                return default(TResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public virtual int ExecuteNonQuery(string query, System.Data.CommandType commandType = System.Data.CommandType.Text, Action<Exception> errorLogAction = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return 0;
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _commandTimeout;
                int value = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public void ExecuteNonQuery(string query, System.Data.CommandType commandType)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return;
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                IDbTransaction trans = _connection.BeginTransaction();
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _commandTimeout;
                cmd.Transaction = trans;
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                trans.Commit();
                trans.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
        public int ExecuteNonQuery(List<string> queries, System.Data.CommandType commandType, Action<Exception> errorLogAction = null)
        {
            int exec = 0;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _commandTimeout;
                List<Exception> exps = new List<Exception>();
                foreach (string query in queries)
                {
                    if (string.IsNullOrEmpty(query)) continue;
                    cmd.CommandText = query;
                    try
                    {
                        exec += cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        exps.Add(ex);
                    }
                }
                cmd.Dispose();
                if (exps.Count > 0)
                {
                    if (errorLogAction != null)
                        foreach (Exception exp in exps)
                        {
                            errorLogAction.Invoke(exp);
                        }
                    else
                    {
                        throw new Exception("Execute script error.");
                    }
                }
            }
            catch (Exception ex)
            {
                if (errorLogAction != null) errorLogAction.Invoke(ex);
                else throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
            return exec;
        }
        public int ExecuteNonQuery_UseTransaction(List<string> queries, System.Data.CommandType commandType)
        {
            int exec = 0;
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                IDbTransaction trans = _connection.BeginTransaction();
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _commandTimeout;
                cmd.Transaction = trans;
                foreach (string query in queries)
                {
                    if (string.IsNullOrEmpty(query)) continue;
                    cmd.CommandText = query;
                    exec = cmd.ExecuteNonQuery();
                }

                trans.Commit();
                trans.Dispose();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
            return exec;
        }

        public SqlDbResult ExecuteReader(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return null;
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                IDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

                SqlDbResult result = null;
                result = _ExecuteReader(reader);
                reader.Close();
                cmd.Dispose();

                return result;
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }
        public SqlDbResult ExecuteReader_UseTransaction(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return null;
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                IDbTransaction trans = _connection.BeginTransaction();
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                cmd.Transaction = trans;
                IDataReader reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                SqlDbResult result = null;
                result = _ExecuteReader(reader);
                reader.Close();
                trans.Commit();
                trans.Dispose();
                cmd.Dispose();

                return result;
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }


        public SqlDbResult ExecuteReaderWithIndex(SqlDbTableQuery table, long fromIndex, long toIndex)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                string query = "";
                string alias = string.IsNullOrEmpty(table.Alias) ? "" : table.Alias + ".";
                string selectFields = string.Join(", ", table.SelectedFields.Select(ite => string.Format("{0}{1}", alias, ite)));
                string orderFields = table.OrderFields?.Count > 0 ? string.Join(", ", table.OrderFields.Select(ite => string.Format("{0}{1} {2}", alias, ite.Field, ite.IsASC ? "ASC" : "DESC"))) : "(SELECT 1)";
                string filter = table.GetFilters();
                if (_provider == EDbProvider.SqlServer)
                {
                    string innerQuery = string.Format("{0} FROM {1} {2} {3}", selectFields, table.Name, table.Alias, filter == "" ? "" : " WHERE " + filter);
                    string rowIndexQuery = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS RIDX, {1}", orderFields, innerQuery);
                    query = string.Format("WITH RES AS ({0}) SELECT * FROM RES WHERE RIDX BETWEEN {1} AND {2}", rowIndexQuery, fromIndex, toIndex);
                }
                else if (_provider == EDbProvider.MariaDB)
                {
                    string innerQuery = string.Format("SELECT ROW_NUMBER() OVER (ORDER BY {0}) AS RIDX, {1} FROM {2} {3} {4}", orderFields, selectFields, table.Name, table.Alias, filter == "" ? "" : " WHERE " + filter);
                    query = string.Format("{0} limit {2}, {3}", innerQuery, orderFields, fromIndex - 1, toIndex - fromIndex + 1);
                }
                else if (_provider == EDbProvider.Mysql)
                {
                    string innerQuery = string.Format("SELECT {0} FROM {1} {2} {3}", selectFields, table.Name, table.Alias, filter == "" ? "" : " WHERE " + filter);
                    query = string.Format("{0} ORDER BY {1} limit {2}, {3}", innerQuery, orderFields, fromIndex - 1, toIndex - fromIndex + 1);
                }

                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                IDataReader reader = cmd.ExecuteReader();
                SqlDbResult result = null;
                result = new SqlDbResult(reader);
                reader.Close();
                cmd.Dispose();
                return result;
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public List<TResult> ExecuteReader<TResult>(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return new List<TResult>();
                SqlDbResult dbRes = ExecuteReader(query);
                List<TResult> results = dbRes?.Cast<TResult>();
                return results ?? new List<TResult>();
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }
        public List<object> ExecuteReader(string query, Type dtoType)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return new List<object>();
                SqlDbResult dbRes = ExecuteReader(query);
                List<object> results = dbRes?.Cast(dtoType);
                return results ?? new List<object>();
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        public void ExecuteReaderBlock<TResult>(string query, Action<List<TResult>> action, int blockSize = 100, Func<bool> stopAction = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return;
                _connection.Open();
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                IDataReader reader = cmd.ExecuteReader();
                List<TResult> records = new List<TResult>(blockSize);
                while (true)
                {
                    bool _stop = stopAction != null && stopAction.Invoke();
                    if (_stop)
                    {
                        cmd.Cancel();
                        break;
                    }
                    records = new List<TResult>(blockSize);
                    DateTime bDt = DateTime.Now;
                    for (int i = 0; i < blockSize && reader.Read(); i++)
                    {
                        TResult result = Parse<TResult>(reader);
                        records.Add(result);
                    }
                    DateTime eDt = DateTime.Now;
                    var ts = eDt - bDt;
                    action(records);
                    if (records.Count == 0) break;
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public void ExecuteReaderBlock(string query, Action<string[]> headerAction, Action<List<object[]>> recordsAction, int blockSize = 100, Func<bool> stopAction = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return;
                _connection.Open();
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                IDataReader reader = cmd.ExecuteReader();
                List<object[]> records = new List<object[]>(blockSize);
                string[] headers = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++) headers[i] = reader.GetName(i);
                headerAction?.Invoke(headers);
                while (true)
                {
                    bool _stop = stopAction != null && stopAction.Invoke();
                    if (_stop)
                    {
                        cmd.Cancel();
                        break;
                    }
                    records = new List<object[]>(blockSize);
                    DateTime bDt = DateTime.Now;
                    for (int i = 0; i < blockSize && reader.Read(); i++)
                    {
                        object[] values = new object[headers.Length];
                        reader.GetValues(values);
                        records.Add(values);
                    }
                    DateTime eDt = DateTime.Now;
                    var ts = eDt - bDt;
                    recordsAction(records);
                    if (records.Count == 0) break;
                }
                reader.Close();
                cmd.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, string[] fields, TDbRecord record) 
            where TDbRecord : SqlDbRecord
        {
            try
            {
                if (fields == null || fields.Length == 0)
                    fields = record.GetDbFields();
                else
                {
                    string[] dataFields = record.GetDbFields();
                    fields = fields.Intersect(dataFields).ToArray();
                }
                string insertFields = _provider == EDbProvider.SqlServer || _provider == EDbProvider.Sqlite
                    ? string.Join(",", fields.Select(ite => "[" + ite + "]"))
                    : string.Join(",", fields.Select(ite => "`" + ite + "`"));
                string dbValue = _provider == EDbProvider.Sqlite ? record.ToSqliteDbValues() : record.ToDbValues(fields);
                string query = string.Format("insert into {0}({1}) values {2}", tableName, insertFields, dbValue);
                int exec = ExecuteNonQuery(query);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string,string>[] mappingFields, TDbRecord record)
            where TDbRecord : SqlDbRecord
        {
            try
            {
                
                var insertFields = _provider == EDbProvider.SqlServer || _provider == EDbProvider.Sqlite
                    ? string.Join(",", mappingFields.Select(ite => "[" + ite.Key + "]"))
                    : string.Join(",", mappingFields.Select(ite => "`" + ite.Key + "`"));
                var valueFields = mappingFields.Select(ite => ite.Value).ToArray();
                string dbValue = _provider == EDbProvider.Sqlite ? record.ToSqliteDbValues() : record.ToDbValues(valueFields);
                string query = string.Format("insert into {0}({1}) values {2}", tableName, insertFields, dbValue);
                int exec = ExecuteNonQuery(query);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, string[] fields, List<TDbRecord> records) 
            where TDbRecord : SqlDbRecord
        {
            try
            {
                TDbRecord record = Activator.CreateInstance<TDbRecord>();
                if (fields == null || fields.Length == 0)
                    fields = record.GetDbFields();
                else
                {
                    string[] dataFields = record.GetDbFields();
                    fields = fields.Intersect(dataFields).Distinct().ToArray();
                }
                string insertFields = _provider == EDbProvider.SqlServer
                    ? string.Join(",", fields.Select(ite => "[" + ite + "]"))
                    : _provider == EDbProvider.Sqlite ? string.Join(",", fields.Select(ite => ite))
                    : string.Join(",", fields.Select(ite => "`" + ite + "`"));
                string dbValue = _provider == EDbProvider.Sqlite
                    ? string.Join(",", records.Select(ite => ite.ToSqliteDbValues(fields)))
                    : string.Join(",", records.Select(ite => ite.ToDbValues(fields)));
                string query = string.Format("insert into {0}({1}) values {2}", tableName, insertFields, dbValue);
                int exec = ExecuteNonQuery(query);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int ExecuteInsert<TDbRecord>(string tableName, KeyValuePair<string, string>[] mappingFields, List<TDbRecord> records)
            where TDbRecord : SqlDbRecord
        {
            try
            {
                TDbRecord record = Activator.CreateInstance<TDbRecord>();
                var insertFields = _provider == EDbProvider.SqlServer || _provider == EDbProvider.Sqlite
                    ? string.Join(",", mappingFields.Select(ite => "[" + ite.Key + "]"))
                    : string.Join(",", mappingFields.Select(ite => "`" + ite.Key + "`"));
                var valueFields = mappingFields.Select(ite => ite.Value).ToArray();
                string dbValue = _provider == EDbProvider.Sqlite
                    ? string.Join(",", records.Select(ite => ite.ToSqliteDbValues(valueFields)))
                    : string.Join(",", records.Select(ite => ite.ToDbValues(valueFields)));
                string query = string.Format("insert into {0}({1}) values {2}", tableName, insertFields, dbValue);
                int exec = ExecuteNonQuery(query);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public int ExecuteUpdate<TDbRecord>(string tableName, string[] keyFields, string[] fields, TDbRecord record) where TDbRecord : SqlDbRecord
        {
            try
            {
                if (fields == null || fields.Length == 0)
                    fields = record.GetDbFields();
                else
                {
                    string[] dataFields = record.GetDbFields();
                    fields = fields.Intersect(dataFields, StringComparer.InvariantCultureIgnoreCase).Distinct().ToArray();
                }

                if (keyFields == null || keyFields.Length == 0)
                {
                    keyFields = new string[] { fields.FirstOrDefault() };

                }
                fields = fields.Except(keyFields, StringComparer.InvariantCultureIgnoreCase).ToArray();
                string dbUpdateValues = _provider == EDbProvider.Sqlite ? record.ToUpdateSqliteDbValues(", ", fields) : record.ToUpdateDbValues(", ", fields);
                string keyValues = record.ToUpdateDbValues(" and ", keyFields);
                string query = string.Format("update {0} set {1} where {2}", tableName, dbUpdateValues, keyValues);
                int exec = ExecuteNonQuery(query);
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteUpdate<TDbRecord>(string tableName, string[] keyFields, string[] fields, List<TDbRecord> records, int updateBlockSize = 50) where TDbRecord : SqlDbRecord
        {
            _connection.Open();
            try
            {
                TDbRecord tRecord = Activator.CreateInstance<TDbRecord>();
                if (fields == null || fields.Length == 0)
                    fields = tRecord.GetDbFields();
                else
                {
                    string[] dataFields = tRecord.GetDbFields();
                    fields = fields.Intersect(dataFields).Distinct().ToArray();
                }
                if (keyFields == null || keyFields.Length == 0)
                {
                    keyFields = new string[] { fields.FirstOrDefault() };
                }
                fields = fields.Except(keyFields, StringComparer.InvariantCultureIgnoreCase).ToArray();
                int exec = 0;
                List<TDbRecord> exe_records;
                do
                {
                    string query = "";
                    exe_records = records.Take(updateBlockSize).ToList();
                    foreach (TDbRecord record in exe_records)
                    {
                        string dbUpdateValues = _provider == EDbProvider.Sqlite ? record.ToUpdateSqliteDbValues(", ", fields) : record.ToUpdateDbValues(", ", fields);
                        string keyValues = record.ToUpdateDbValues(" and ", keyFields);
                        query = string.Format("update {0} set {1} where {2};", tableName, dbUpdateValues, keyValues) + Environment.NewLine;
                        if (query != "")
                        {
                            exec += ExecuteNonQueryKeepConnection(query);
                        }
                    }
                    records = records.Skip(updateBlockSize).ToList();
                }
                while (records.Count > 0);


                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _connection.Close();
                _connection.Dispose();
            }
        }

        TResult Parse<TResult>(IDataReader record)
        {
            try
            {
                Type rType = typeof(TResult);
                TResult result = default(TResult);
                if (rType == typeof(byte[]) || rType.IsValueType || rType == typeof(string))
                {
                    result = (TResult)record[0];
                }
                else
                {
                    result = Activator.CreateInstance<TResult>();
                    PropertyInfo[] fields = rType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    object value;
                    foreach (PropertyInfo field in fields)
                    {
                        try { value = record[field.Name]; }
                        catch { value = null; }
                        value = value == DBNull.Value || value == null
                                ? field.PropertyType == typeof(string) ? Activator.CreateInstance(field.PropertyType, string.Empty.ToArray())
                                : Activator.CreateInstance(field.PropertyType)
                                : value;
                        if (field.PropertyType.IsEnum) field.SetValue(result, Enum.ToObject(field.PropertyType, value), null);
                        else if (field.PropertyType == typeof(Guid))
                        {
                            Guid gValue;
                            if (value.GetType() == typeof(byte[]))
                                gValue = new Guid((byte[])value);
                            else
                                gValue = new Guid(value.ToString());
                            field.SetValue(result, gValue, null);
                        }
                        else if (field.PropertyType == typeof(byte[])) field.SetValue(result, value, null);
                        else field.SetValue(result, Convert.ChangeType(value.ToString(), field.PropertyType), null);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected Dictionary<string, string> GetFieldAndType(string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return new Dictionary<string, string>();
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();
                }
                Dictionary<string, string> fields = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = _commandTimeout;
                var csProvider = new Microsoft.CSharp.CSharpCodeProvider();
                IDataReader reader = cmd.ExecuteReader();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string name = reader.GetName(i);
                    Type type = reader.GetFieldType(i);
                    string sType = csProvider.GetTypeOutput(new System.CodeDom.CodeTypeReference(type));
                    fields[name] = sType;
                }

                reader.Close();
                cmd.Dispose();
                return fields;
            }
            catch (DbException ex)
            {
                throw ex;
            }
        }

        SqlDbResult _ExecuteReader(IDataReader reader)
        {
            try
            {
                SqlDbResult result = new SqlDbResult();
                result.Fields = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++) result.Fields[i] = reader.GetName(i);
                object[] objs;
                result.Values = new List<object[]>();
                while (reader.Read())
                {
                    objs = new object[result.Fields.Length];


                    try
                    {
                        reader.GetValues(objs);
                    }
                    catch
                    {
                        for (int i = 0; i < objs.Length; i++)
                        {
                            try
                            {
                                objs[i] = reader[i];
                            }
                            catch { }
                        }
                    }

                    result.Values.Add(objs);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual int ExecuteNonQueryKeepConnection(string query, System.Data.CommandType commandType = System.Data.CommandType.Text, Action<Exception> errorLogAction = null)
        {
            try
            {
                if (string.IsNullOrEmpty(query)) return 0;

                IDbCommand cmd = _connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = commandType;
                cmd.CommandTimeout = _commandTimeout;
                int value = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
