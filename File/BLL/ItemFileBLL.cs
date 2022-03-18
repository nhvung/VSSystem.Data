using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.BLL;
using VSSystem.Data.File.DAL;
using VSSystem.Data.File.DTO;

namespace VSSystem.Data.File.BLL
{
    class ItemFileBLL<TDAL, TDTO> : DataBLL<TDAL, TDTO>
        where TDAL : IItemFileDAL<TDTO>
        where TDTO : ItemFileDTO
    {
        public static int CloseFile(string tableName, int file_ID)
        {
            return GetDAL(tableName).CloseFile(file_ID);
        }
        public static TDTO GetLastFile(string tableName, string templateName)
        {
            try
            {
                var dal = GetDAL(tableName);
                var dbFile = dal.GetLastFile();
                if (dbFile == null)
                {
                    dbFile = dal.CreateNewFile(templateName);
                }
                return dbFile;
            }
            catch { }
            return null;
        }
    }
    class ItemFileBLL<TImageHashFileDAL> : ItemFileBLL<TImageHashFileDAL, ItemFileDTO>
        where TImageHashFileDAL : IItemFileDAL<ItemFileDTO>
    {

    }
    class ItemFileBLL : ItemFileBLL<IItemFileDAL<ItemFileDTO>>
    {

    }
}
