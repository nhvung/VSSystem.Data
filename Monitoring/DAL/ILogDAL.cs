using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.Monitoring.DTO;

namespace VSSystem.Data.Monitoring.DAL
{
    public class ILogDAL<TDTO> : DataDAL<TDTO>
        where TDTO : LogDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (" +
            "`ID` bigint primary key auto_increment, " +
            "`Server_ID` int, " +
            "`Component_ID` int, " +
            "`Type` tinyint, " +
            "`Name` varchar(255), " +
            "`Tag` varchar(255), " +
            "`Contents` mediumtext, " +
            "`CreatedTicks` bigint, " +
            "`ServerTicks` bigint, " +
            "index(`Server_ID`), " +
            "index(`Component_ID`), " +
            "index(`Type`), " +
            "index(`Name`), " +
            "index(`Tag`), " +
            "index(`ServerTicks`)," +
            "index(`CreatedTicks`)" +
            ");";
        public ILogDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public ILogDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }

        public List<LogFilterDTO> GetFilters(long lTicksFrom, long lTicksTo)
        {
            try
            {
                string query = string.Format("select Server_ID, Component_ID, Type, Name, count(ID) as Total from {0}", _TableName);
                List<string> timeFts = new List<string>();
                if (lTicksFrom > 0)
                {
                    timeFts.Add("CreatedTicks >= " + lTicksFrom);
                }
                if (lTicksTo > 0)
                {
                    timeFts.Add("CreatedTicks <= " + lTicksTo);
                }
                if (timeFts?.Count > 0)
                {
                    query += " where " + string.Join(" and ", timeFts);
                }
                query += " group by Server_ID, Component_ID, Type, Name";
                return ExecuteReader<LogFilterDTO>(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
