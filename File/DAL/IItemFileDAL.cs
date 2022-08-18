using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.File.Define;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.DAL
{
    public class IItemFileDAL<TDTO> : DataDAL<TDTO>
         where TDTO : ItemFileDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` ("
        + "`File_ID` int primary key, "
        + "`Path` varchar(255), "
        + "`Status` tinyint, "
        + "`CreatedDateTime` bigint, "
        + "index(`Status`), "
        + "index (`CreatedDateTime`)"
        + ");";
        public IItemFileDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        protected IItemFileDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public TDTO GetLastFile()
        {
            try
            {
                string query = string.Format("select * from {0} where Status = {1} order by CreatedDateTime desc limit 1;", _TableName, EFileStatus.NOT_FULL);
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public TDTO CreateNewFile(string templateName)
        {
            try
            {
                int maxFile_ID = GetMaxValue<int>("File_ID");
                TDTO dbFile = Activator.CreateInstance<TDTO>();
                DateTime now = DateTime.Now;
                dbFile.File_ID = maxFile_ID + 1;
                dbFile.CreatedDateTime = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
                dbFile.Path = string.Format("{0}/{1:yyyyMMdd}/file_{2:00000000#}.dat", templateName, now, dbFile.File_ID);
                dbFile.Status = EFileStatus.NOT_FULL;

                Insert(dbFile);

                return dbFile;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int CloseFile(int file_ID)
        {
            try
            {
                string query = string.Format("update {0} set Status = {1} where File_ID = {2}", _TableName, EFileStatus.FULL, file_ID);
                return ExecuteNonQuery(query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
