using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.Monitoring.DAL;
using VSSystem.Data.Monitoring.DTO;

namespace VSSystem.Data.Monitoring.BLL
{
    public class ConfigurationBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
           where TDAL : IConfigurationDAL<TDTO>
           where TDTO : ConfigurationDTO
    {
        public const string DEFAULT_TABLE_NAME = "Configurations";
        static public TDTO GetConfiguration(string tableName, int server_ID, int component_ID, string path)
        {
            return GetDAL(tableName).GetConfiguration(server_ID, component_ID, path);
        }
    }

    public class ConfigurationBLL<TComponentDAL> : ConfigurationBLL<TComponentDAL, ConfigurationDTO>
        where TComponentDAL : IConfigurationDAL<ConfigurationDTO>
    {

    }
    public class ConfigurationBLL : ConfigurationBLL<IConfigurationDAL<ConfigurationDTO>>
    {

    }
}
