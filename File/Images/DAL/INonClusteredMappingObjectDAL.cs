using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.Images.DAL
{
    public class INonClusteredMappingObjectDAL<TDTO> : IMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
        public INonClusteredMappingObjectDAL(string tableName) : base(tableName)
        { 
        }
        public INonClusteredMappingObjectDAL(string tableName, SqlPoolProcess sqlPoolProcess) : base(tableName, sqlPoolProcess)
        {
        }
    }
}
