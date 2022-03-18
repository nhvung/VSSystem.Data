using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSSystem.Data
{
    public partial class SqlDbProcess : SqlProcess
    {

        public List<string> GetAllDatabases()
        {
            try
            {
                var databases = ExecuteReader<string>(_provider == EDbProvider.SqlServer ? "select name from sys.databases where name not in ('master', 'model', 'tempdb', 'msdb', 'resource')" : "show databases where `database` not in ('mysql', 'performance_schema', 'information_schema')");
                return databases;
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
                var qTables = Clone<SqlDbProcess>().ExecuteReader<SqlDbTableStruture>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_TABLES_MYSQL : _GET_TABLES_SQLSERVER).GroupBy(ite => ite.table_name, StringComparer.InvariantCultureIgnoreCase).ToDictionary(ite => ite.Key, ite => ite.ToArray(), StringComparer.InvariantCultureIgnoreCase);
                
                
                var qIndexes = Clone<SqlDbProcess>().ExecuteReader<SqlDbTableIndex>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_INDEXES_MYSQL : _GET_INDEXES_SQLSERVER).GroupBy(ite => ite.table_name, StringComparer.InvariantCultureIgnoreCase).ToDictionary(ite => ite.Key, ite => ite.ToArray(), StringComparer.InvariantCultureIgnoreCase);

                List<SqlDbTable> tables = new List<SqlDbTable>(qTables.Count);
                foreach (var qTable in qTables)
                {
                    SqlDbColumn[] columns = qTable.Value.Select(ite => new SqlDbColumn(ite.column_name, ite.data_type, ite.character_set_name, ite.character_maximum_length, ite.numeric_precision, ite.numeric_scale, ite.is_nullable, ite.column_key, ite.is_identity, _provider)).ToArray();
                    SqlDbIndex[] indexes = new SqlDbIndex[0];
                    if (qIndexes.ContainsKey(qTable.Key))
                    {
                        indexes = qIndexes[qTable.Key].GroupBy(ite => new { ite.index_name, ite.is_unique }).Select(ite => new SqlDbIndex(ite.Select(ite1 => ite1.column_name).ToArray(), ite.Key.is_unique == 1)).ToArray();
                    }
                    SqlDbTable table = new SqlDbTable(qTable.Key, columns, indexes);
                    tables.Add(table);
                }
                return tables;
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
                var qTables = ExecuteReader<SqlDbTableStruture>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_TABLES_MYSQL : _GET_TABLES_SQLSERVER).GroupBy(ite => ite.table_name, StringComparer.InvariantCultureIgnoreCase).ToDictionary(ite => ite.Key, ite => ite.ToArray(), StringComparer.InvariantCultureIgnoreCase);
                var qIndexes = ExecuteReader<SqlDbTableIndex>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_INDEXES_MYSQL : _GET_INDEXES_SQLSERVER).GroupBy(ite => ite.table_name, StringComparer.InvariantCultureIgnoreCase).ToDictionary(ite => ite.Key, ite => ite.ToArray(), StringComparer.InvariantCultureIgnoreCase);

                foreach (var qTable in qTables)
                {
                    if (qTable.Key.Equals(tableName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        SqlDbColumn[] columns = qTable.Value.Select(ite => new SqlDbColumn(ite.column_name, ite.data_type, ite.character_set_name, ite.character_maximum_length, ite.numeric_precision, ite.numeric_scale, ite.is_nullable, ite.column_key, ite.is_identity, _provider)).ToArray();
                        SqlDbIndex[] indexes = new SqlDbIndex[0];
                        if (qIndexes.ContainsKey(qTable.Key))
                        {
                            indexes = qIndexes[qTable.Key].GroupBy(ite => new { ite.index_name, ite.is_unique }).Select(ite => new SqlDbIndex(ite.Select(ite1 => ite1.column_name).ToArray(), ite.Key.is_unique == 1)).ToArray();
                        }
                        SqlDbTable table = new SqlDbTable(qTable.Key, columns, indexes);
                        return table;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GenerateDTOClass(string query, string className, string nameSpace = "")
        {
            try
            {
                Dictionary<string, string> fields = GetFieldAndType(query);
                string nameSpaceDefine = string.IsNullOrEmpty(nameSpace) ? "using System;\r\n{0}" : string.Format("using System;\r\nnamespace {0} {{{{\r\n{{0}}\r\n}}}}", nameSpace);
                string classDefine = string.IsNullOrEmpty(className) ? "" : string.Format("public class {0} {{{{\r\n{{0}}\r\n}}}}", className);
                string[] lines = fields.Select(ite => string.Format("{0} _{1};", ite.Value, ite.Key) + Environment.NewLine + string.Format("public {0} {1} {{ get {{ return _{1}; }} set {{ _{1} = value; }}}}", ite.Value, ite.Key)).ToArray();
                string properties = string.Join(Environment.NewLine + Environment.NewLine, lines);
                classDefine = string.Format(classDefine, properties);
                string contents = string.Format(nameSpaceDefine, classDefine);
                return contents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GenerateTableDTOClass(string tableName, string className, string nameSpace = "")
        {
            try
            {
                Dictionary<string, string> fields = GetFieldAndType(_provider == EDbProvider.Mysql ? "select * from " + tableName + " limit 0" : "select top 0 * from " + tableName);
                string nameSpaceDefine = string.IsNullOrEmpty(nameSpace) ? "using System;\r\n{0}" : string.Format("using System;\r\nnamespace {0} {{{{\r\n{{0}}\r\n}}}}", nameSpace);
                string classDefine = string.IsNullOrEmpty(className) ? "" : string.Format("public class {0} {{{{\r\n{{0}}\r\n}}}}", className);
                string[] lines = fields.Select(ite => string.Format("{0} _{1};", ite.Value, ite.Key) + Environment.NewLine + string.Format("public {0} {1} {{ get {{ return _{1}; }} set {{ _{1} = value; }}}}", ite.Value, ite.Key)).ToArray();
                string properties = string.Join(Environment.NewLine + Environment.NewLine, lines);
                classDefine = string.Format(classDefine, properties);
                string contents = string.Format(nameSpaceDefine, classDefine);
                return contents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DuplicateTable(string oldTableName, string newTableName, bool includeData = true)
        {

            try
            {
                string query = _provider == EDbProvider.MariaDB || _provider == EDbProvider.Mysql ? string.Format("create table {0} select * from {1}", newTableName, oldTableName) : string.Format("select * into {0} from {1}", newTableName, oldTableName);
                if (!includeData) query += " where 1 = 2";
                ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public List<SqlDbStatement> GetAllDatabaseStatements()
        {

            try
            {
                List<SqlDbStatement> statements = new List<SqlDbStatement>();

                var tables = GetTables();
                if (tables?.Count > 0)
                {
                    foreach (var table in tables)
                    {
                        string createStatement = table.ToSqlString(_provider);
                        SqlDbStatement statement = new SqlDbStatement(table.Name, createStatement.Split(new char[] { '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).ToList(), SqlDbStatement.Type.Table);
                        statements.Add(statement);
                    }
                }
                List<string> procedures = Clone().ExecuteReader<string>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_PROCDURE_NAMES_MYSQL : _GET_PROCDURE_NAMES_SQLSERVER);
                foreach (string procedure in procedures)
                {
                    var res = _provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? GetDefinition_MYSQL(procedure, "procedure") : GetDefinition_SQLSERVER(procedure, "procedure");
                    SqlDbStatement statement = new SqlDbStatement(procedure, res, SqlDbStatement.Type.Procedure);
                    statements.Add(statement);
                }
                List<string> triggers = Clone().ExecuteReader<string>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_TRIGGER_NAMES_MYSQL : _GET_TRIGGER_NAMES_SQLSERVER);
                foreach (string trigger in triggers)
                {
                    var res = _provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? GetDefinition_MYSQL(trigger, "trigger") : GetDefinition_SQLSERVER(trigger, "trigger");
                    SqlDbStatement statement = new SqlDbStatement(trigger, res, SqlDbStatement.Type.Trigger);
                    statements.Add(statement);
                }

                return statements;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        List<string> GetDefinition_SQLSERVER(string objectName, string type)
        {
            string query = "sp_helptext " + objectName;
            string shortType = type.Equals("procedure", StringComparison.InvariantCultureIgnoreCase) ? "P" : "TR";
            List<string> qRes = Clone().ExecuteReader<string>(query);
            List<string> result = new List<string>() { string.Format("if object_id('{0}', '{2}') is not null drop {1} {0};", objectName, type, shortType) };
            List<string> strs = new List<string>();
            foreach (string qr in qRes)
            {
                string s = string.Join(" ", qr.Trim(' ').Split(new char[] { '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                if (string.IsNullOrEmpty(s)) continue;
                if (s.StartsWith("--")) continue;
                strs.Add(s);
            }

            if (strs?.Count > 0)
            {
                string createQuery = string.Join(" ", strs).Trim();
                if (!createQuery.EndsWith(";")) createQuery += ";";
                result.Add(createQuery.Trim(' '));
            }

            return result;
        }
        List<string> GetDefinition_MYSQL(string objectName, string type)
        {
            string query = string.Format("show create {0} {1}", type, objectName);
            var qRes = Clone().ExecuteReader(query);
            string[] smLines = qRes.Values[0][2].ToString().Split(new char[] { '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> result = new List<string>() { string.Format("drop {1} if exists `{0}`;", objectName, type) };
            List<string> strs = new List<string>();
            foreach (string line in smLines)
            {
                if (line == "" || line.Trim().StartsWith("--")) continue;
                strs.Add(line);
            }
            if (strs?.Count > 0)
            {
                string createQuery = string.Join(" ", strs).Trim();
                if (!createQuery.EndsWith(";")) createQuery += ";";
                result.Add(createQuery.Trim(' '));
            }
            return result;
        }

        public List<string> GetAllTableNames()
        {

            try
            {
                var qTables = Clone().ExecuteReader<string>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_TABLE_NAMES_MYSQL : _GET_TABLE_NAMES_SQLSERVER);
                return qTables;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get size of database with MB unit
        /// </summary>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        public float GetDatabaseSize(string databaseName = null)
        {
            try
            {
                string database = string.IsNullOrEmpty(databaseName) ? _provider == EDbProvider.SqlServer ? "db_name()" : "database()" : "'" + databaseName + "'";
                string query = _provider == EDbProvider.SqlServer
                    ? "select sum(sysfile.size) * 8 / 1024 from sys.databases sysdb join sys.master_files sysfile on sysdb.database_id = sysfile.database_id and sysdb.name = " + database
                    : "select sum(data_length + index_length) / 1024 / 1024 from information_schema.tables where table_schema = " + database;
                float result = ExecuteScalar<float>(query);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<string> GetAllProcedureNames()
        {

            try
            {
                var result = Clone().ExecuteReader<string>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_PROCDURE_NAMES_MYSQL : _GET_PROCDURE_NAMES_SQLSERVER);
                return result;
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
                var result = Clone().ExecuteReader<string>(_provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? _GET_TRIGGER_NAMES_MYSQL : _GET_TRIGGER_NAMES_SQLSERVER);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int DeleteDatabase(string databaseName)
        {
            try
            {
                string query = _provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? string.Format("drop database if exists {0};", databaseName) : string.Format("if object_id('{0}') is not null drop database {0};", databaseName);
                return Clone().ExecuteNonQuery(query);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CreateDatabase(string databaseName, bool overwrite = false)
        {
            try
            {
                string query = "";
                if (overwrite) DeleteDatabase(databaseName);
                query = _provider == EDbProvider.Mysql || _provider == EDbProvider.MariaDB ? string.Format("create database if not exists {0};", databaseName) : string.Format("if object_id('{0}') is null create database {0};", databaseName);
                return Clone().ExecuteNonQuery(query);
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
                string query = _provider == EDbProvider.SqlServer ? "select top 0 * from " + tableName : "select * from " + tableName + " limit 0";
                var result = GetFieldAndType(query)?.Keys.ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int ExecuteInsert(string tableName, string[] fields, Type dataType, params object[] records)
        {
            try
            {
                if (records?.Length > 0)
                {
                    if (fields == null || fields.Length == 0)
                    {
                        fields = GetTableFields(tableName).ToArray();
                    }
                    else
                    {
                        string[] dataFields = GetTableFields(tableName).ToArray();
                        fields = fields.Intersect(dataFields).Distinct().ToArray();
                    }
                    string insertFields = _provider == EDbProvider.SqlServer ? string.Join(",", fields.Select(ite => "[" + ite + "]")) : string.Join(",", fields.Select(ite => "`" + ite + "`"));
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
        public int TruncateTable(string tableName)
        {
            try
            {
                string query = "truncate table " + tableName;
                var result = ExecuteNonQuery(query);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
