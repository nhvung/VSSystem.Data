using System;
using System.IO;

namespace VSSystem.Data
{
    public class Variables
    {
        public static SqlPoolProcess GetSqlProcessFromIniFile(Func<string, string, string, string> readIniValueFunc, Func<string, string> descryptFunc, string configSection = "database_info"
            , Action<string> debugLogAction = null, Action<Exception> errorLogAction = null)
        {
            try
            {
                string sqlHost = readIniValueFunc.Invoke(configSection, "server", "localhost");
                string sqlUsername = readIniValueFunc(configSection, "user", "eiq");
                string sqlPassword = readIniValueFunc(configSection, "password", string.Empty);
                if (descryptFunc != null)
                {
                    sqlPassword = descryptFunc(sqlPassword);
                }

                int port, sqlCommandTimeout, numberOfConnections;
                string sPort = readIniValueFunc(configSection, "port", "0");
                int.TryParse(sPort, out port);
                string sqlDatabase = readIniValueFunc(configSection, "database", "");
                string sqlDriver = readIniValueFunc(configSection, "driver", "mariadb odbc 3.1 driver");
                string sSqlCommandTimeout = readIniValueFunc(configSection, "cmd_timeout", "120");
                int.TryParse(sSqlCommandTimeout, out sqlCommandTimeout);
                string sNumberOfConnections = readIniValueFunc(configSection, "number_connection", "1");
                int.TryParse(sNumberOfConnections, out numberOfConnections);
                if (numberOfConnections <= 0)
                {
                    numberOfConnections = 1;
                }

                var sqlProcess = new SqlPoolProcess(sqlHost, sqlUsername, sqlPassword, port, sqlDatabase, 120, sqlCommandTimeout, sqlDriver, numberOfConnections, numberOfConnections, debugLogAction);
                return sqlProcess;
            }
            catch (Exception ex)
            {
                errorLogAction?.Invoke(ex);
            }
            return null;
        }

        public static SqlPoolProcess GetSqlProcess(string sqlHost, string sqlUsername, string sqlPassword, int port, string sqlDatabase, string sqlDriver, int sqlCommandTimeout = 120, int numberOfConnections = 1)
        {
            try
            {
                var sqlProcess = new SqlPoolProcess(sqlHost, sqlUsername, sqlPassword, port, sqlDatabase, 120, sqlCommandTimeout, sqlDriver, numberOfConnections, numberOfConnections);
                return sqlProcess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SqlitePoolProcess GetSqliteProcessFromIniFile(DirectoryInfo folder, Func<string, string, string, string> readIniValueFunc, Func<string, string> descryptFunc, string configSection = "database_info")
        {
            try
            {
                string dbFileName = readIniValueFunc.Invoke(configSection, "db_file", folder.FullName + "/main.s3db");
                string sqlPassword = readIniValueFunc(configSection, "password", string.Empty);
                if (descryptFunc != null)
                {
                    sqlPassword = descryptFunc(sqlPassword);
                }
                int sqlCommandTimeout, numberOfConnections;
                string sSqlCommandTimeout = readIniValueFunc(configSection, "cmd_timeout", "120");
                int.TryParse(sSqlCommandTimeout, out sqlCommandTimeout);
                string sNumberOfConnections = readIniValueFunc(configSection, "number_connection", "1");
                int.TryParse(sNumberOfConnections, out numberOfConnections);
                if (numberOfConnections <= 0)
                {
                    numberOfConnections = 1;
                }

                SqlitePoolProcess sqlProcess = null;
            RETRY_INIT:
                try
                {
                    sqlProcess = new SqlitePoolProcess(dbFileName, sqlPassword, sqlCommandTimeout, numberOfConnections);
                }
                catch (Exception ex)
                {
                    if (ex.Message?.IndexOf("You need to call SQLitePCL.raw.SetProvider().", StringComparison.InvariantCultureIgnoreCase) >= 0
                        || ex.InnerException?.Message?.IndexOf("You need to call SQLitePCL.raw.SetProvider().", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        goto RETRY_INIT;
                    }
                }
                return sqlProcess;
            }
            catch// (Exception ex)
            {

            }
            return null;
        }

        public static SqlitePoolProcess GetSqliteProcess(string dbFileName, string password, int sqlCommandTimeout = 120, int numberOfConnections = 1)
        {
            try
            {
                var sqlProcess = new SqlitePoolProcess(dbFileName, password, sqlCommandTimeout, numberOfConnections);
                return sqlProcess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
