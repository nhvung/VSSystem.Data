using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.Filters;
using VSSystem.Data.File.Images.DTO;

namespace VSSystem.Data.File.Images.DAL
{
    public class IImageHashDAL<TDTO> : DataDAL<TDTO>
        where TDTO : ImageHashDTO
    {
        const string _CREATE_TABLE_STATEMENTS = "create table if not exists `{0}` (`Sha1` binary(20) primary key, `Image_ID` bigint, `Width` int, `Height` int, `File_ID` int, `Position` bigint, `CreatedDateTime` bigint, unique index (`Image_ID`), index(`File_ID`));";
        public IImageHashDAL(string tableName) : base(Variables.SqlPoolProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public IImageHashDAL(string tableName, SqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _CreateTableStatements = _CREATE_TABLE_STATEMENTS;
            _AutoCreateTable = true;
        }
        public TDTO GetHashImage(byte[] sha1Key)
        {
            try
            {

                string query = string.Format("select * from {0} where Sha1 = 0x{1}", _TableName, BitConverter.ToString(sha1Key).Replace("-", ""));
                var dbObjs = ExecuteReader<TDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TInfoDTO GetHashImageInfo<TInfoDTO>(long image_ID)
            where TInfoDTO : ImageHashInfoDTO
        {
            try
            {
                string query = string.Format("select img.Image_ID, img.Width, img.Height, img.File_ID, img.Position, imgf.Path from {0} img " +
                    "join {0}File imgf on img.File_ID = imgf.File_ID " +
                    "where img.Image_ID = {1}", _TableName, image_ID);
                var dbObjs = ExecuteReader<TInfoDTO>(query);
                return dbObjs?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<TInfoDTO> GetHashImageInfo<TInfoDTO>(List<long> image_IDs)
            where TInfoDTO : ImageHashInfoDTO
        {
            try
            {
                if (image_IDs?.Count > 0)
                {
                    string ft = BaseFilter.GetFilter("img", "Image_ID", image_IDs.ToArray());
                    string query = string.Format("select img.Image_ID, img.Width, img.Height, img.File_ID, img.Position, imgf.Path from {0} img " +
                    "join {0}File imgf on img.File_ID = imgf.File_ID " +
                    "where {1}", _TableName, ft);
                    var dbObjs = ExecuteReader<TInfoDTO>(query);
                    return dbObjs;
                }
                return new List<TInfoDTO>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
