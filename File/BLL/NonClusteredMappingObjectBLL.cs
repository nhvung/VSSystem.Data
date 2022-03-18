using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.BLL
{
    public class NonClusteredMappingObjectBLL<TDAL, TDTO> : MappingObjectBLL<TDAL, TDTO>
         where TDAL : INonClusterMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
    }
    public class NonClusteredMappingObjectBLL<TMappingObjectDAL> : NonClusteredMappingObjectBLL<TMappingObjectDAL, MappingObjectDTO>
        where TMappingObjectDAL : INonClusterMappingObjectDAL<MappingObjectDTO>
    {

    }
    public class NonClusteredMappingObjectBLL : NonClusteredMappingObjectBLL<INonClusterMappingObjectDAL<MappingObjectDTO>>
    {

    }
}
