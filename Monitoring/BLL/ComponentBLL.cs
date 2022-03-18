using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.Monitoring.DAL;
using VSSystem.Data.Monitoring.DTO;

namespace VSSystem.Data.Monitoring.BLL
{
    public class ComponentBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
           where TDAL : IComponentDAL<TDTO>
           where TDTO : ComponentDTO
    {
        public const string DEFAULT_TABLE_NAME = "Components";
        static public List<TDTO> GetComponentByName(string tableName, string name, int server_ID)
        {
            return GetDAL(tableName).GetComponentByName(name, server_ID);
        }
    }

    public class ComponentBLL<TDAL> : ComponentBLL<TDAL, ComponentDTO>
        where TDAL : IComponentDAL<ComponentDTO>
    {

    }
    public class ComponentBLL : ComponentBLL<IComponentDAL<ComponentDTO>>
    {

    }
}
