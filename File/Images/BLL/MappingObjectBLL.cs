using System.Collections.Generic;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.Images.BLL
{
    public class MappingObjectBLL<TDAL, TDTO> : File.BLL.MappingObjectBLL<TDAL, TDTO>
         where TDAL : File.Images.DAL.IMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
        new static public TDTO GetMappingByID(string tableName, long id)
        {
            return GetDAL(tableName).GetMappingByID(id);
        }
        new static public List<TDTO> GetMappingsByIDs(string tableName, List<long> ids)
        {
            return GetDAL(tableName).GetMappingsByIDs(ids);
        }
    }
    public class MappingObjectBLL<TMappingObjectDAL> : MappingObjectBLL<TMappingObjectDAL, MappingObjectDTO>
        where TMappingObjectDAL : File.Images.DAL.IMappingObjectDAL<MappingObjectDTO>
    {

    }
    public class MappingObjectBLL : MappingObjectBLL<File.Images.DAL.IMappingObjectDAL<MappingObjectDTO>>
    {

    }
}
