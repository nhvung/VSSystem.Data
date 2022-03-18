using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.File.DTO;
using VSSystem.Data.Filters;

namespace VSSystem.Data.File.DAL
{
    public class IItemDAL<TDTO> : DataDAL<TDTO>
        where TDTO : ItemDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (`Sha1` binary(20) primary key, `ID` bigint, `File_ID` int, `Position` bigint, `CreatedDateTime` bigint, unique index (`ID`), index(`File_ID`));";
        public IItemDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public IItemDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public TDTO GetItemByHash(byte[] sha1Bytes)
        {
            try
            {
                string query = string.Format("select * from {0} where Sha1 = 0x{1}", _TableName, BitConverter.ToString(sha1Bytes).Replace("-", ""));
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TDTO GetItemByHash(string sha1String)
        {
            try
            {
                string query = string.Format("select * from {0} where Sha1 = 0x{1}", _TableName, sha1String.Replace("-", ""));
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TItemInfoDTO GetItemInfoByID<TItemInfoDTO>(long id) where TItemInfoDTO : ItemInfoDTO
        {

            try
            {
                string query = $"select item.ID, item.File_ID, item.Position, itemf.Path from {_TableName} item " +
                    $"join {_TableName}File itemf on itemf.File_ID = item.File_ID " +
                    $"where item.ID = {id}";
                var dbObjs = ExecuteReader<TItemInfoDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<TItemInfoDTO> GetItemInfoByID<TItemInfoDTO>(List<long> ids) where TItemInfoDTO : ItemInfoDTO
        {

            try
            {
                if (ids?.Count > 0) 
                {
                    string ft = BaseFilter.GetFilter("item", "ID", ids.ToArray());
                    string query = $"select item.ID, item.File_ID, item.Position, itemf.Path from {_TableName} item " +
                    $"join {_TableName}File itemf on itemf.File_ID = item.File_ID " +
                    $"where {ft}";
                    var dbObjs = ExecuteReader<TItemInfoDTO>(query);
                    return dbObjs;
                }
                return new List<TItemInfoDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
