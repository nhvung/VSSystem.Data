using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.File
{
    public class Variables : Data.Variables
    {
        static EDbProvider _Provider;
        public static EDbProvider Provider { get { return _Provider; } }
        public static SqlPoolProcess SqlPoolProcess;

        public static void InitFromIniFile(Func<string, string, string, string> readIniValueFunc, Func<string, string> descryptFunc, string configSection = "database_file_info")
        {
            SqlPoolProcess = GetSqlProcessFromIniFile(readIniValueFunc, descryptFunc, configSection);
            _Provider = SqlPoolProcess.Provider;
        }
        public static void Init(string sqlHost, string sqlUsername, string sqlPassword, int port, string sqlDatabase, string sqlDriver, int sqlCommandTimeout = 120, int numberOfConnections = 1)
        {
            SqlPoolProcess = GetSqlProcess(sqlHost, sqlUsername, sqlPassword, port, sqlDatabase, sqlDriver, sqlCommandTimeout, numberOfConnections);
            _Provider = SqlPoolProcess.Provider;
        }
    }
}
