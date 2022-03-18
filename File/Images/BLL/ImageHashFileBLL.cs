using VSSystem.Data.BLL;
using VSSystem.Data.File.Images.DAL;
using VSSystem.Data.File.Images.DTO;

namespace VSSystem.Data.File.Images.BLL
{
    class ImageHashFileBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
        where TDAL : IImageHashFileDAL<TDTO>
         where TDTO : ImageHashFileDTO
         
    {
        public static int CloseFile(string tableName, int file_ID)
        {
            return GetDAL(tableName).CloseFile(file_ID);
        }
        //public static TDTO GetLastFile(string tableName, string templateName)
        //{
        //    try
        //    {
        //        var dal = GetDAL(tableName);
        //        var dbFile = dal.GetLastFile();
        //        if (dbFile == null)
        //        {
        //            dbFile = dal.CreateNewFile(templateName);
        //        }
        //        return dbFile;
        //    }
        //    catch { }
        //    return null;
        //}
        public static TDTO GetLastFile(string tableName, string templateName, int importType)
        {
            try
            {
                var dal = GetDAL(tableName);
                var dbFile = dal.GetLastFile(importType);
                if (dbFile == null)
                {
                    dbFile = dal.CreateNewFile(templateName, importType);
                }
                return dbFile;
            }
            catch { }
            return null;
        }
    }
    class ImageHashFileBLL<TImageHashFileDAL> : ImageHashFileBLL<TImageHashFileDAL, ImageHashFileDTO>
        where TImageHashFileDAL : IImageHashFileDAL<ImageHashFileDTO>
    {

    }
    class ImageHashFileBLL : ImageHashFileBLL<IImageHashFileDAL<ImageHashFileDTO>>
    {

    }
}
