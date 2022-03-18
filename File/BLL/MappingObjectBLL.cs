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
        static public TDTO GetMappingByIDs(string tableName, string itemIDFieldName,long id)
        {
            return GetDAL(tableName, itemIDFieldName).GetMappingByID(id);
        }
        static public List<TDTO> GetMappingByIDs(string tableName, string itemIDFieldName, List<long> ids)
        {
            return GetDAL(tableName, itemIDFieldName).GetMappingByIDs(ids);
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
