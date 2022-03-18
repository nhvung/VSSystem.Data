using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.Monitoring.DAL;
using VSSystem.Data.Monitoring.DTO;

namespace VSSystem.Data.Monitoring.BLL
{
    public class LogBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
         where TDAL : ILogDAL<TDTO>
         where TDTO : LogDTO
    {
        public const string DEFAULT_TABLE_NAME = "TLogs";

        static public List<LogFilterDTO> GetFilters(string tableName, long lTicksFrom, long lTicksTo)
        {
            return GetDAL(tableName).GetFilters(lTicksFrom, lTicksTo);
        }
    }

    public class LogBLL<TDAL> : LogBLL<TDAL, LogDTO>
        where TDAL : ILogDAL<LogDTO>
    {

    }
    public class LogBLL : LogBLL<ILogDAL<LogDTO>>
    {

    }
}
