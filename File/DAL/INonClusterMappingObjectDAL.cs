using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.DAL
{
    public class INonClusterMappingObjectDAL<TDTO> : IMappingObjectDAL<TDTO>
        where TDTO : MappingObjectDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{{0}}` ("
        + "`ID` int primary key AUTO_INCREMENT, "
        + "`{1}` bigint, "
        + "`Base_ID` bigint, "
        + "`ObjectKey` varchar(50), "
        + "`CreatedDateTime` bigint, "
        + "index (`{1}`), "
        + "index (`Base_ID`), "
        + "index (`ObjectKey`), "
        + "index(`CreatedDateTime`)"
        + ");";

        public INonClusterMappingObjectDAL(string tableName, string item_IDFieldName) : base(tableName, item_IDFieldName)
        {
        }
        public INonClusterMappingObjectDAL(string tableName) : base(tableName)
        {
        }

        public INonClusterMappingObjectDAL(string tableName, string item_IDFieldName, SqlPoolProcess sqlPoolProcess) : base(tableName, item_IDFieldName, sqlPoolProcess)
        {
        }
        public INonClusterMappingObjectDAL(string tableName, SqlPoolProcess sqlPoolProcess) : base(tableName, sqlPoolProcess)
        {
        }
        protected override void _InitCreateTableStatements()
        {
            _CreateTableStatements = string.Format(_CREATE_TABLE_STATEMENTS, _TableName, _Item_IDFieldName);
        }
    }
}
