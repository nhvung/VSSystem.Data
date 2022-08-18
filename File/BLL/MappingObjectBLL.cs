using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.BLL
{
    public class MappingObjectBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
         where TDAL : IMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
        static public TDTO GetMappingByID(string tableName, string itemIDFieldName, long id)
        {
            return GetDAL(tableName, itemIDFieldName).GetMappingByID(id);
        }
        static public TDTO GetMappingByID(string tableName, long id)
        {
            return GetDAL(tableName).GetMappingByID(id);
        }
        static public TDTO GetMappingByKey(string tableName, string itemIDFieldName, string key)
        {
            return GetDAL(tableName, itemIDFieldName).GetMappingByKey(key);
        }
        static public TDTO GetMappingByKey(string tableName, string key)
        {
            return GetDAL(tableName).GetMappingByKey(key);
        }
        static public List<TDTO> GetMappingsByIDs(string tableName, string itemIDFieldName, List<long> ids)
        {
            return GetDAL(tableName, itemIDFieldName).GetMappingsByIDs(ids);
        }
        static public List<TDTO> GetMappingsByIDs(string tableName, List<long> ids)
        {
            return GetDAL(tableName).GetMappingsByIDs(ids);
        }
        static public List<TDTO> GetMappingsByKeys(string tableName, string itemIDFieldName, List<string> keys)
        {
            return GetDAL(tableName, itemIDFieldName).GetMappingsByKeys(keys);
        }
        static public List<TDTO> GetMappingsByKeys(string tableName, List<string> keys)
        {
            return GetDAL(tableName).GetMappingsByKeys(keys);
        }
    }
    public class MappingObjectBLL<TMappingObjectDAL> : MappingObjectBLL<TMappingObjectDAL, MappingObjectDTO>
        where TMappingObjectDAL : IMappingObjectDAL<MappingObjectDTO>
    {

    }
    public class MappingObjectBLL : MappingObjectBLL<IMappingObjectDAL<MappingObjectDTO>>
    {

    }
}
