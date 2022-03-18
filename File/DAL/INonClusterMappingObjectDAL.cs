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
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{{0}}` (`ID` int primary key AUTO_INCREMENT, `{0}` bigint, `Base_ID` bigint, `CreatedDateTime` bigint, index (`{0}`), index (`Base_ID`), index(`CreatedDateTime`));";

        public INonClusterMappingObjectDAL(string tableName, string item_IDFieldName) : base(tableName, item_IDFieldName)
        {
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
        }

        public INonClusterMappingObjectDAL(string tableName, string item_IDFieldName, SqlPoolProcess sqlPoolProcess) : base(tableName, item_IDFieldName, sqlPoolProcess)
        {
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
        }
    }
}
