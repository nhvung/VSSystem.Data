using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.Monitoring.DAL;
using VSSystem.Data.Monitoring.DTO;

namespace VSSystem.Data.Monitoring.BLL
{
    public class ServerBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
        where TDAL : IServerDAL<TDTO>
        where TDTO : ServerDTO
    {
        public const string DEFAULT_TABLE_NAME = "Servers";
        static public TDTO GetServerByIPAddress(string tableName, string ipAddress)
        {
            return GetDAL(tableName).GetServerByIPAddress(ipAddress);
        }

        static public TDTO GetServerByName(string tableName, string name)
        {
            return GetDAL(tableName).GetServerByName(name);
        }
    }

    public class ServerBLL<TDAL> : ServerBLL<TDAL, ServerDTO>
        where TDAL : IServerDAL<ServerDTO>
    {

    }
    public class ServerBLL : ServerBLL<IServerDAL<ServerDTO>>
    {

    }
}
