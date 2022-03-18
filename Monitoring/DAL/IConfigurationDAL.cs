using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.Monitoring.DTO;
using VSSystem.Data.DAL;

namespace VSSystem.Data.Monitoring.DAL
{
    public class IConfigurationDAL<TDTO> : DataDAL<TDTO>
        where TDTO : ConfigurationDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (`ID` int primary key auto_increment, `Server_ID` int, `Component_ID` int, `Path` varchar(255), `Value` mediumtext, `Status` tinyint, `CreatedDateTime` bigint, `UpdatedDateTime` bigint, index(`Server_ID`, `Component_ID`, `Path`));";
        public IConfigurationDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public IConfigurationDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }

        public TDTO GetConfiguration(int server_ID, int component_ID, string path)
        {
            try
            {
                string query = $"select * from {_TableName} where Server_ID = '{server_ID}' and Component_ID = {component_ID}";
                if (!string.IsNullOrWhiteSpace(path))
                {
                    query += $" and Path = '{path.Replace("'", "''").Replace("\\", "\\\\")}'";
                }
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
