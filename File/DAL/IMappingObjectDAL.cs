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
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{{0}}` ("
        + "`ID` int primary key AUTO_INCREMENT, "
        + "`{1}` bigint, "
        + "`Base_ID` bigint, "
        + "`ObjectKey` varchar(50), "
        + "`CreatedDateTime` bigint, "
        + "unique index (`{1}`), "
        + "index (`Base_ID`), "
        + "index (`ObjectKey`), "
        + "index(`CreatedDateTime`)"
        + ");";
        protected string _Item_IDFieldName;

        public IMappingObjectDAL(string tableName, string item_IDFieldName) : base(Variables.SqlPoolProcess)
        {
            _Item_IDFieldName = item_IDFieldName;
            _TableName = tableName;
            _AutoCreateTable = true;
            _InitCreateTableStatements();
        }
        public IMappingObjectDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _Item_IDFieldName = "Item_ID";
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
        public IMappingObjectDAL(string tableName, SqlPoolProcess sqlPoolProcess) : base(sqlPoolProcess)
        {
            _Item_IDFieldName = "Item_ID";
            _TableName = tableName;
            _AutoCreateTable = true;
            _InitCreateTableStatements();
        }
        protected virtual void _InitCreateTableStatements()
        {
            _CreateTableStatements = string.Format(_CREATE_TABLE_STATEMENTS, _TableName, _Item_IDFieldName);
        }
        public virtual List<TDTO> GetMappingsByIDs(List<long> ids)
        {

            try
            {
                if (ids?.Count > 0)
                {
                    string query = string.Format("select ID, {1} as Item_ID, Base_ID, ObjectKey, CreatedDateTime from {0} where {1} in ({2})", _TableName, _Item_IDFieldName, string.Join(", ", ids));
                Retry:
                    List<TDTO> dbObjs = null;
                    try
                    {
                        dbObjs = ExecuteReader<TDTO>(query);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("Unknown column 'ObjectKey'", StringComparison.InvariantCultureIgnoreCase) >= 0)
                        {
                            query = string.Format("select ID, {1} as Item_ID, Base_ID, CreatedDateTime from {0} where {1} in ({2})", _TableName, _Item_IDFieldName, string.Join(", ", ids));
                            goto Retry;
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    return dbObjs;
                }
                return new List<TDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual List<TDTO> GetMappingsByKeys(List<string> objectKeys)
        {

            try
            {
                if (objectKeys?.Count > 0)
                {
                    string query = string.Format("select ID, {1} as Item_ID, Base_ID, ObjectKey, CreatedDateTime from {0} where ObjectKey in ({2})", _TableName, _Item_IDFieldName, string.Join(", ", objectKeys.Select(ite => $"'{ite}'")));
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
                string query = string.Format("select ID, {1} as Item_ID, Base_ID, ObjectKey, CreatedDateTime from {0} where {1} = {2} order by CreatedDateTime desc", _TableName, _Item_IDFieldName, id);
            Retry:
                List<TDTO> dbObjs = null;
                try
                {
                    dbObjs = ExecuteReader<TDTO>(query);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Unknown column 'ObjectKey'", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        query = string.Format("select ID, {1} as Item_ID, Base_ID, CreatedDateTime from {0} where {1} = {2} order by CreatedDateTime desc", _TableName, _Item_IDFieldName, id);
                        goto Retry;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual TDTO GetMappingByKey(string objectKey)
        {
            try
            {
                string query = string.Format("select ID, {1} as Item_ID, Base_ID, ObjectKey, CreatedDateTime from {0} where ObjectKey = '{2}' order by CreatedDateTime desc", _TableName, _Item_IDFieldName, objectKey);
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
                KeyValuePair<string, string>[] insertFields = new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string, string>(_Item_IDFieldName, "Item_ID"),
                            new KeyValuePair<string, string>("Base_ID", "Base_ID"),
                            new KeyValuePair<string, string>("ObjectKey", "ObjectKey"),
                            new KeyValuePair<string, string>("CreatedDateTime", "CreatedDateTime"),
                        };
                int exec = 0;
            Retry:
                try
                {
                    exec = _sqlProcess.ExecuteInsert(_TableName, insertFields, dbObj);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Unknown column 'ObjectKey'", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        insertFields = new KeyValuePair<string, string>[]
                        {
                            new KeyValuePair<string, string>(_Item_IDFieldName, "Item_ID"),
                            new KeyValuePair<string, string>("Base_ID", "Base_ID"),
                            new KeyValuePair<string, string>("CreatedDateTime", "CreatedDateTime"),
                        };
                        goto Retry;
                    }
                    else
                    {
                        throw ex;
                    }
                }
                return exec;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
