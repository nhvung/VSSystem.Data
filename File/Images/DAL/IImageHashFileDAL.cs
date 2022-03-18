using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.File.Define;
using VSSystem.Data.File.Images.DTO;

namespace VSSystem.Data.File.Images.DAL
{
    public class IImageHashFileDAL<TDTO> : VSSystem.Data.DAL.DataDAL<TDTO>
        where TDTO : ImageHashFileDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (`File_ID` int primary key, `Path` varchar(255), `ImportType` int, `Status` tinyint, `CreatedDateTime` bigint, index(`ImportType`), index (`CreatedDateTime`));";
        public IImageHashFileDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        protected IImageHashFileDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
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

        public TDTO GetLastFile(int importType)
        {
            try
            {
                string query = string.Format("select * from {0} where Status = {1} and ImportType = {2} order by CreatedDateTime desc limit 1;", _TableName, EFileStatus.NOT_FULL, importType);
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
        public TDTO CreateNewFile(string templateName, int importType)
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
                dbFile.ImportType = importType;
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
