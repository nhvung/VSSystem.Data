using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbScript
    {
        public static string GetCreateIndex(string tableName, string indexName, string[] ascColumns, string[] descColumns, bool isUnique, EDbProvider provider)
        {
            try
            {
                string query = "";
                List<string> columns = new List<string>();
                if (ascColumns?.Length > 0) columns.AddRange(ascColumns.Select(ite => ite + " asc"));
                if (descColumns?.Length > 0) columns.AddRange(descColumns.Select(ite => ite + " desc"));

                if (columns.Count > 0)
                {
                    if (provider == EDbProvider.MariaDB || provider == EDbProvider.Mysql)
                        query = string.Format("if exists((select 1 from information_schema.statistics where table_schema= database() and table_name = '{1}' and index_name = '{0}')) then drop index {0} on {1}; end if; ", indexName, tableName);
                    else if (provider == EDbProvider.SqlServer)
                        query = string.Format("if exists (select 1 from sys.indexes where name='{0}' and object_id = object_id('{1}')) begin drop index {0} on {1} end;", indexName, tableName);
                    query += Environment.NewLine + string.Format("create" + (isUnique ? " unique" : "") + " index {0} on {1}({2});", indexName, tableName, string.Join(", ", columns));
                }
                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetAddColumn_NormalTable(string tableName, SqlDbColumn column, EDbProvider provider)
        {

            try
            {
                string query = "";
                if (column != null)
                {
                    string columnString = column.ToSqlString(provider);
                    if (provider == EDbProvider.MariaDB || provider == EDbProvider.Mysql)
                        query = string.Format("if exists (select * from information_schema.tables where table_name = '{1}' and table_schema = database()) then if not exists (select * from information_schema.columns where column_name='{0}' and table_name = '{1}' and table_schema = database()) then alter table {1} add {2}; end if; end if;", column.Name, tableName, columnString);
                    else if (provider == EDbProvider.SqlServer)
                        query = string.Format("if exists (select * from information_schema.tables where table_name = '{1}' and table_catalog = db_name()) begin if not exists (select * from information_schema.columns where column_name='{0}' and table_name = '{1}' and table_catalog = db_name()) begin alter table {1} add {2}; end; end;", column.Name, tableName, columnString);
                }

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryGetTableName">Like: select TableName from PartitionTable [where ?] [order by ?]</param>
        /// <param name="columns"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static string GetAddColumns_PartitionTable(string queryGetTableName, string alias, List<SqlDbColumn> columns, EDbProvider provider)
        {

            try
            {
                string query = "";

                if (columns?.Count > 0)
                {
                    if (provider == EDbProvider.MariaDB || provider == EDbProvider.Mysql)
                    {
                        query += "declare done int default false;" + Environment.NewLine;
                        query += "declare tname varchar(100);" + Environment.NewLine;
                        query += string.Format("declare {0}_curs cursor for {1};" + Environment.NewLine, alias, queryGetTableName);
                        query += "declare continue handler for not found set done = true;" + Environment.NewLine;
                        query += "set done = false;" + Environment.NewLine;
                        query += string.Format("open {0}_curs;" + Environment.NewLine, alias);
                        query += "iLoop: loop" + Environment.NewLine;
                        query += string.Format("fetch {0}_curs into tname;" + Environment.NewLine, alias);
                        query += "if(done) then leave iLoop;" + Environment.NewLine;
                        query += "end if;" + Environment.NewLine; ;
                        query += "set @execsql = '';" + Environment.NewLine;
                        foreach (var column in columns)
                        {
                            string columnString = column.ToSqlString(provider);
                            //if(provider == EDbProvider.MariaDB) query += string.Format("set @execsql = concat('if not exists (select * from information_schema.columns where column_name=''{0}'' and table_name = ''', tname, ''') then alter table ', tname,' add {1}; end if;');" + Environment.NewLine, column.Name, columnString);
                            //query += string.Format("set @execsql = (select if(count(1) = 0, concat('select ''', tname, ' not exists'' as Message'), (select if(count(1) = 0, concat('alter table ', TName, ' add {1};'), concat('select ''', TName ,'.{0} existed.'' as Message')) from information_schema.columns where table_schema = database() and column_name = '{0}' and table_name = TName)) from information_schema.tables where table_schema = database() and table_name = tname);" + Environment.NewLine, column.Name, columnString);
                            query += string.Format("set @execsql = (select if(count(1) = 0, null, (select if(count(1) = 0, concat('alter table ', TName, ' add {1};'), null) from information_schema.columns where table_schema = database() and column_name = '{0}' and table_name = TName)) from information_schema.tables where table_schema = database() and table_name = tname);" + Environment.NewLine, column.Name, columnString);
                            //set @execsql = (select if(count(1) = 0, concat('alter table ', TName, ' add CasingNumber int;'), concat('select ''', TName ,'.CasingNumber existed.''')) from information_schema.columns where table_schema = database() and column_name = 'CasingNumber' and table_name = TName);
                            query += "if @execsql is not null then prepare stmt from @execsql; execute stmt; end if;" + Environment.NewLine;
                            //query += "prepare stmt from @execsql;" + Environment.NewLine;
                            //query += "execute stmt;" + Environment.NewLine;
                            //if @execsql is not null then prepare stmt from @execsql; execute stmt; end if;
                        }

                        query += "end loop;" + Environment.NewLine;
                        query += string.Format("close {0}_curs;" + Environment.NewLine, alias);
                    }
                    else if (provider == EDbProvider.SqlServer)
                    {
                        query += "declare @dbname nvarchar(100);" + Environment.NewLine;
                        query += "set @dbname = db_name();" + Environment.NewLine;
                        query += "exec sp_removedbreplication @dbname;" + Environment.NewLine;

                        query += "declare @name nvarchar(100);" + Environment.NewLine;
                        query += "declare @execsql nvarchar(max) = N'';" + Environment.NewLine;

                        query += string.Format("declare {0}_curs cursor for {1};" + Environment.NewLine, alias, queryGetTableName);
                        query += string.Format("open {0}_curs;" + Environment.NewLine, alias);
                        query += string.Format("fetch next from {0}_curs into @name" + Environment.NewLine, alias);


                        query += "while @@fetch_status = 0" + Environment.NewLine;
                        query += "begin" + Environment.NewLine;
                        foreach (var column in columns)
                        {
                            string columnString = column.ToSqlString(provider);
                            query += string.Format("set @execsql = (select N'if exists (select * from information_schema.tables where table_name = ''' + @name + ''' and table_catalog = db_name()) begin if not exists (select * from information_schema.columns where column_name=''{0}'' and table_name = ''' + @name + ''' and table_catalog = db_name()) begin alter table ' + @name + N' add {1}; end; else select ''Table ' + @name + ' not exists'' as Message end;');" + Environment.NewLine, column.Name, columnString);
                            //query += string.Format("set @execsql = (select if(count(1) = 0, concat('alter table ', TName, ' add {1};'), concat('select ''', TName ,'.{0} existed.''')) from information_schema.columns where table_schema = database() and column_name = '{0}' and table_name = TName);" + Environment.NewLine, column.Name, columnString);
                            query += "exec sp_executesql @execsql;" + Environment.NewLine;
                        }
                        query += string.Format("fetch next from {0}_curs into @name;" + Environment.NewLine, alias);
                        query += "end" + Environment.NewLine;
                        query += string.Format("close {0}_curs;" + Environment.NewLine, alias);
                        query += string.Format("deallocate {0}_curs;" + Environment.NewLine, alias);

                    }
                }

                return query;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string GetContentBetweenBeginEnd(string[] lines, ref int tIdx)
        {
            try
            {
                string line = lines[tIdx].Trim();
                if (line.EndsWith("end", StringComparison.InvariantCultureIgnoreCase)) { tIdx++; return line; }
                if (line.EndsWith("end;", StringComparison.InvariantCultureIgnoreCase)) { tIdx++; return line; }
                string content = line;
                tIdx++;
                while (tIdx < lines.Length)
                {
                    line = lines[tIdx].Trim();
                    if (line == "" || line.StartsWith("--")) { tIdx++; continue; }
                    if (line.StartsWith("begin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string beginEndStatement = GetContentBetweenBeginEnd(lines, ref tIdx);
                        content += " " + beginEndStatement;

                    }
                    else if (line.EndsWith("end", StringComparison.InvariantCultureIgnoreCase))
                    {
                        content += " " + line.Replace("//", "");
                        tIdx++;
                        break;
                    }
                    else if (line.EndsWith("end;", StringComparison.InvariantCultureIgnoreCase))
                    {
                        content += " " + line.Replace("//", "");
                        tIdx++;
                        break;
                    }
                    else
                    {
                        content += " " + line.Replace("//", "");
                        tIdx++;
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string GetProduceDefinition(string[] lines, ref int tIdx)
        {

            try
            {
                string line = lines[tIdx].Trim();
                if (line.EndsWith("end", StringComparison.InvariantCultureIgnoreCase)) return line;
                string content = line;
                tIdx++;
                while (tIdx < lines.Length)
                {
                    line = lines[tIdx].Trim();
                    if (line == "" || line.StartsWith("--")) { tIdx++; continue; }
                    if (line.StartsWith("begin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string beginEndStatement = GetContentBetweenBeginEnd(lines, ref tIdx);
                        content += " " + beginEndStatement;
                        //tIdx++;
                        break;

                    }
                    else if (line.StartsWith("as", StringComparison.InvariantCultureIgnoreCase))
                    {
                        content += " " + line;
                        tIdx++;
                    }
                    else
                    {
                        content += " " + line;
                        tIdx++;
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static string GetTriggerDefinition(string[] lines, ref int tIdx)
        {

            try
            {
                string line = lines[tIdx].Trim();
                if (line.EndsWith("end", StringComparison.InvariantCultureIgnoreCase)) return line;
                string content = line;
                tIdx++;
                while (tIdx < lines.Length)
                {
                    line = lines[tIdx].Trim();
                    if (line == "" || line.StartsWith("--")) { tIdx++; continue; }
                    if (line.StartsWith("begin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string beginEndStatement = GetContentBetweenBeginEnd(lines, ref tIdx);
                        content += " " + beginEndStatement;
                        //tIdx++;
                        break;

                    }
                    else if (line.StartsWith("as", StringComparison.InvariantCultureIgnoreCase))
                    {
                        content += " " + line;
                        tIdx++;
                    }
                    else
                    {
                        content += " " + line;
                        tIdx++;
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static string PreProcessContent(string fileContent)
        {

            try
            {
                int fromIdx = 0;
                int beginIdx = fileContent.IndexOf("begin", fromIdx, StringComparison.InvariantCultureIgnoreCase);
                while (beginIdx != -1)
                {
                    string subString1 = fileContent.Substring(0, beginIdx);
                    string subString2 = Environment.NewLine + fileContent.Substring(beginIdx);
                    fileContent = subString1 + subString2;
                    fromIdx = beginIdx + 5;
                    beginIdx = fileContent.IndexOf("begin", fromIdx, StringComparison.InvariantCultureIgnoreCase);
                }

                fromIdx = 0;
                int endQuotIdx = fileContent.IndexOf("end;", fromIdx, StringComparison.InvariantCultureIgnoreCase);
                while (endQuotIdx != -1)
                {
                    string subString1 = fileContent.Substring(0, endQuotIdx + 4);
                    string subString2 = Environment.NewLine + fileContent.Substring(endQuotIdx + 4);
                    fileContent = subString1 + subString2;
                    fromIdx = endQuotIdx + 4;
                    endQuotIdx = fileContent.IndexOf("end;", fromIdx, StringComparison.InvariantCultureIgnoreCase);
                }
                fromIdx = 0;
                int delimiterIdx = fileContent.IndexOf("//", fromIdx, StringComparison.InvariantCultureIgnoreCase);
                while (delimiterIdx != -1)
                {
                    string subString1 = fileContent.Substring(0, delimiterIdx);
                    string subString2 = Environment.NewLine + fileContent.Substring(delimiterIdx);
                    fileContent = subString1 + subString2;
                    fromIdx = delimiterIdx + 4;
                    delimiterIdx = fileContent.IndexOf("//", fromIdx, StringComparison.InvariantCultureIgnoreCase);
                }
                return fileContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<string> GetContentScriptInFile(string fileContent, string server = "%", string user = "root", EDbProvider provider = EDbProvider.SqlServer)
        {

            try
            {
                fileContent = PreProcessContent(fileContent);
                List<string> lines = new List<string>();
                string[] tLines = fileContent.Split(new char[] { '\t', '\r', '\n', '$' }, StringSplitOptions.RemoveEmptyEntries);
                int lIdx = 0;
                while (lIdx < tLines.Length)
                {
                    string line = tLines[lIdx].Trim();
                    if (line == "" || line.StartsWith("--") || line.StartsWith("//") || line.StartsWith("$$")) { lIdx++; continue; }
                    if (line.StartsWith("go", StringComparison.InvariantCultureIgnoreCase) || line.StartsWith("delimiter", StringComparison.InvariantCultureIgnoreCase)) { lIdx++; continue; };
                    if (line.StartsWith("CREATE PROCEDURE", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string procdureStatement = GetProduceDefinition(tLines, ref lIdx);
                        lines.Add(procdureStatement);
                    }
                    else if (line.StartsWith("CREATE trigger", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string triggerStatement = GetTriggerDefinition(tLines, ref lIdx);
                        string definer = "";
                        if (provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB)
                        {
                            definer = string.Format("Create DEFINER=`{0}`@`{1}` trigger", user, server);
                            triggerStatement = definer + " " + triggerStatement.Substring("CREATE trigger".Length);
                        }
                        lines.Add(triggerStatement);
                    }
                    else if (line.StartsWith("begin", StringComparison.InvariantCultureIgnoreCase))
                    {
                        string beginEndStatement = GetContentBetweenBeginEnd(tLines, ref lIdx);
                        lines[lines.Count - 1] += " " + beginEndStatement;
                    }
                    else if (line.StartsWith("as", StringComparison.InvariantCultureIgnoreCase))
                    {
                        lines[lines.Count - 1] += " " + line;
                        lIdx++;
                    }
                    else
                    {
                        lines.Add(line);
                        lIdx++;
                    }
                }
                return lines;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
