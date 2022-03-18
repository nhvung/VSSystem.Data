using VSSystem.Data.File.DTO;
using VSSystem.Data.File.Images.DAL;

namespace VSSystem.Data.File.Images.BLL
{
    public class NonClusteredMappingObjectBLL<TDAL, TDTO> : MappingObjectBLL<TDAL, TDTO>
         where TDAL : INonClusteredMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
    }
    public class NonClusteredMappingObjectBLL<TMappingObjectDAL> : NonClusteredMappingObjectBLL<TMappingObjectDAL, MappingObjectDTO>
        where TMappingObjectDAL : INonClusteredMappingObjectDAL<MappingObjectDTO>
    {

    }
    public class NonClusteredMappingObjectBLL : NonClusteredMappingObjectBLL<INonClusteredMappingObjectDAL<MappingObjectDTO>>
    {

    }
}
