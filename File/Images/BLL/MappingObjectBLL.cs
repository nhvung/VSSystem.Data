using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.File.BLL;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.DTO;
using VSSystem.Data.File.Images.DAL;

namespace VSSystem.Data.File.Images.BLL
{
    public class MappingObjectBLL<TDAL, TDTO> : File.BLL.MappingObjectBLL<TDAL, TDTO>
         where TDAL : File.Images.DAL.IMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
        static public TDTO GetMappingByIDs(string tableName,long id)
        {
            return GetDAL(tableName).GetMappingByID(id);
        }
        static public List<TDTO> GetMappingByIDs(string tableName, List<long> ids)
        {
            return GetDAL(tableName).GetMappingByIDs(ids);
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
