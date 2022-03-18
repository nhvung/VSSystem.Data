using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.DAL
{
    public class IMappingObjectDAL<TDTO> : DataDAL<TDTO>
        where TDTO : MappingObjectDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{{0}}` (`ID` int primary key AUTO_INCREMENT, `{0}` bigint, `Base_ID` bigint, `CreatedDateTime` bigint, unique index (`{0}`), index (`Base_ID`), index(`CreatedDateTime`));";
        protected string _Item_IDFieldName;
        public IMappingObjectDAL(string tableName, string item_IDFieldName) : base(Variables.SqlPoolProcess)
        {
            _Item_IDFieldName = item_IDFieldName;
            _TableName = tableName;
            _AutoCreateTable = true;
            _InitCreateTableStatements();
        }
        public IMappingObjectDAL(string tableName, string item_IDFieldName, SqlPoolProcess sqlPoolProcess) : base(sqlPoolProcess)
        {
            _Item_IDFieldName = item_IDFieldName;
            _TableName = tableName;
            _AutoCreateTable = true;
            _InitCreateTableStatements();
        }
        protected virtual void _InitCreateTableStatements()
        {
            _CreateTableStatements = string.Format(_CREATE_TABLE_STATEMENTS, _Item_IDFieldName);
        }
        public virtual List<TDTO> GetMappingByIDs(List<long> ids)
        {

            try
            {
                if (ids?.Count > 0)
                {
                    string query = string.Format("select ID, {1} as Item_ID, Base_ID, CreatedDateTime from {0} where {1} in ({2})", _TableName, _Item_IDFieldName, string.Join(", ", ids));
                    var dbObjs = ExecuteReader<TDTO>(query);
                    return dbObjs;
                }
                return new List<TDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TDTO GetMappingByID(long id)
        {
            try
            {
                string query = string.Format("select ID, {1} as Item_ID, Base_ID, CreatedDateTime from {0} where {1} = {2}", _TableName, _Item_IDFieldName, id);
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override int Insert(TDTO dbObj)
        {
            try
            {
                return _sqlProcess.ExecuteInsert(_TableName, new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string, string>(_Item_IDFieldName, "Item_ID"),
                            new KeyValuePair<string, string>("Base_ID", "Base_ID"),
                            new KeyValuePair<string, string>("CreatedDateTime", "CreatedDateTime"),
                        }, dbObj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
