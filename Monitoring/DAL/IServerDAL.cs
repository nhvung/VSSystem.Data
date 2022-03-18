using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.Monitoring.DTO;
using VSSystem.Data.DAL;

namespace VSSystem.Data.Monitoring.DAL
{
    public class IServerDAL<TDTO> : DataDAL<TDTO>
        where TDTO : ServerDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (" +
            "`ID` int primary key auto_increment, " +
            "`Name` varchar(100), " +
            "`IPAddress` varchar(100), " +
            "`Url` varchar(255), " +
            "`Status` tinyint, " +
            "`CreatedDateTime` bigint, " +
            "`UpdatedDateTime` bigint, " +
            "index(`Name`), " +
            "index(`IPAddress`)" +
            ");";
        public IServerDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public IServerDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }

        public TDTO GetServerByIPAddress(string ipAddress)
        {

            try
            {
                string query = $"select * from {_TableName} where IPAddress = '{ipAddress}' and Status = 1";
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TDTO GetServerByName(string name)
        {

            try
            {
                string query = $"select * from {_TableName} where Name = '{name}' and Status = 1";
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
