using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DTO;

namespace VSSystem.Data.DAL
{
    public class IAvailableValueDAL<TDTO> : DataDAL<TDTO> where TDTO : DataDTO
    {
        public IAvailableValueDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
        }
    }
}
