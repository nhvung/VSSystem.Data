using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.Images.DAL
{
    public class IMappingObjectDAL<TDTO> : File.DAL.IMappingObjectDAL<TDTO>
         where TDTO : MappingObjectDTO
    {
        public IMappingObjectDAL(string tableName) 
            : base(tableName, "Image_ID", Variables.SqlPoolProcess)
        { 
        }
        public IMappingObjectDAL(string tableName, SqlPoolProcess sqlPoolProcess) 
            : base(tableName, "Image_ID", sqlPoolProcess)
        {
        }
    }
}
