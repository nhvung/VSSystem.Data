using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.DAL
{
    public class DataDAL : ADataDAL
    {

        protected DataDAL(SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
        }
        protected DataDAL(ISqlPoolProcess sqlProcess) : base(sqlProcess)
        {
        }
    }
}
