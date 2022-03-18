using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.DAL
{
    public class SqliteDataDAL : ADataDAL
    {
        protected SqliteDataDAL(SqlitePoolProcess sqlProcess) : base(sqlProcess)
        {
        }
        protected SqliteDataDAL(ISqlPoolProcess sqlProcess) : base(sqlProcess)
        {
        }
    }
}
