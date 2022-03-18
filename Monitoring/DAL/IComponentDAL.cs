using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.Monitoring.DTO;
using VSSystem.Data.DAL;

namespace VSSystem.Data.Monitoring.DAL
{
    public class IComponentDAL<TDTO> : DataDAL<TDTO>
       where TDTO : ComponentDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (`ID` int primary key auto_increment, `Parent_ID` int, `Server_ID` int, `Name` varchar(100), `Type` tinyint, `HttpPort` int,  `HttpsPort` int, `Status` tinyint, `CreatedDateTime` bigint, `UpdatedDateTime` bigint, index(`Server_ID`), index(`Parent_ID`), index(`Name`), index(`Type`));";

        public IComponentDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public IComponentDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }

        public List<TDTO> GetComponentByName(string name, int server_ID)
        {

            try
            {
                string query = $"select * from {_TableName} where Name = '{name}' and Server_ID = {server_ID}";
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
