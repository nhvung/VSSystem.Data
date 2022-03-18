using System;
using System.Collections.Generic;

namespace VSSystem.Data.Monitoring
{
    public class Variables : Data.Variables
    {
        static EDbProvider _Provider;
        public static EDbProvider Provider { get { return _Provider; } }
        public static SqlPoolProcess SqlPoolProcess;
        public static void Init(string sqlHost, string sqlUsername, string sqlPassword, int port, string sqlDatabase, string sqlDriver, int sqlCommandTimeout, int numberOfConnections)
        {
            SqlPoolProcess = new SqlPoolProcess(sqlHost, sqlUsername, sqlPassword, port, sqlDatabase, 120, sqlCommandTimeout, sqlDriver, numberOfConnections, numberOfConnections);
            _Provider = SqlPoolProcess.Provider;

        }
        public static void InitFromIniFile(Func<string, string, string, string> readIniValueFunc, Func<string, string> descryptFunc, string configSection = "database_monitoring_info")
        {
            SqlPoolProcess = GetSqlProcessFromIniFile(readIniValueFunc, descryptFunc, configSection);
            _Provider = SqlPoolProcess.Provider;
        }
    }
}
