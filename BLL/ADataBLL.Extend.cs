using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.DTO;
using VSSystem.Data.Filters;

namespace VSSystem.Data.BLL
{
    public abstract partial class ADataBLL<TDAL, TDTO>
         where TDAL : ADataDAL<TDTO>
         where TDTO : DataDTO
    {

        #region Extend Methods

        #region SQLProcess

        public static List<TDTO> Search<TFilter>(ISqlPoolProcess sqlProcess, TFilter filter)
            where TFilter : BaseFilter
        {
            if (filter == null) return new List<TDTO>();
            TDAL dal = GetDAL(sqlProcess);
            return _Search(dal, filter);
        }
        public static List<TResult> Search<TResult, TFilter>(ISqlPoolProcess sqlProcess, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL(sqlProcess);
            return _Search<TResult, TFilter>(dal, filter);
        }
        public static List<TResult> SearchIDs<TResult, TFilter>(ISqlPoolProcess sqlProcess, TFilter filter, params string[] resultFields)
            where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL(sqlProcess);
            return _SearchIDs<TResult, TFilter>(dal, filter, resultFields);
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(ISqlPoolProcess sqlProcess, TFilter filter, int pageSize, int pageNumber, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new PageDataResult<TDTO>();
            }
            TDAL dal = GetDAL(sqlProcess);
            PageDataResult<TDTO> result = _PageSplitSearch(dal, filter, pageSize, pageNumber, orderFields);
            return result;
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(ISqlPoolProcess sqlProcess, TFilter filter, int pageSize, int pageNumber, List<KeyValuePair<string, string>> selectedFields, List<string> orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new PageDataResult<TDTO>();
            }
            TDAL dal = GetDAL(sqlProcess);
            PageDataResult<TDTO> result = _PageSplitSearch(dal, filter, pageSize, pageNumber, selectedFields, orderFields);
            return result;
        }
        public static int Insert(ISqlPoolProcess sqlProcess, TDTO dbObj)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _Insert(dal, dbObj);
        }
        public static int Insert(ISqlPoolProcess sqlProcess, List<TDTO> dbObjs)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _Insert(dal, dbObjs);
        }
        public static int Truncate(ISqlPoolProcess sqlProcess)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _Truncate(dal);
        }
        public static T GetMaxValue<T>(ISqlPoolProcess sqlProcess, string field)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _GetMaxValue<T>(dal, field);
        }
        public static T GetMinValue<T>(ISqlPoolProcess sqlProcess, string field)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _GetMinValue<T>(dal, field);
        }
        public static List<TDTO> GetAllData(ISqlPoolProcess sqlProcess)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _GetAllData(dal);
        }
        public static int GetTotal<TFilter>(ISqlPoolProcess sqlProcess, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return 0;
            TDAL dal = GetDAL(sqlProcess);
            return _GetTotal(dal, filter);
        }
        public static int Update(ISqlPoolProcess sqlProcess, TDTO dbObj, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _Update(dal, dbObj, keyFields, updateFields);
        }
        public static int Update(ISqlPoolProcess sqlProcess, List<TDTO> dbObjs, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL(sqlProcess);
            return _Update(dal, dbObjs, keyFields, updateFields);
        }
        public static List<TStatResult> Calculate<TStatResult, TFilter>(ISqlPoolProcess sqlProcess, TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
           where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new List<TStatResult>();
            }
            TDAL dal = GetDAL(sqlProcess);
            return _Calculate<TStatResult, TFilter>(dal, filter, statFields, calculateFields, orderFields);
        }
        #endregion

        #region TableName + SQLProcess
        public static List<TDTO> Search<TFilter>(string tableName, ISqlPoolProcess sqlProcess, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TDTO>();
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Search(dal, filter);
        }
        public static List<TResult> Search<TResult, TFilter>(string tableName, ISqlPoolProcess sqlProcess, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Search<TResult, TFilter>(dal, filter);
        }
        public static List<TResult> SearchIDs<TResult, TFilter>(string tableName, ISqlPoolProcess sqlProcess, TFilter filter, params string[] resultFields)
            where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _SearchIDs<TResult, TFilter>(dal, filter, resultFields);
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(string tableName, ISqlPoolProcess sqlProcess, TFilter filter, int pageSize, int pageNumber, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null) return new PageDataResult<TDTO>();
            TDAL dal = GetDAL(tableName, sqlProcess);
            PageDataResult<TDTO> result = _PageSplitSearch(dal, filter, pageSize, pageNumber, orderFields);
            return result;
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(string tableName, ISqlPoolProcess sqlProcess, TFilter filter, int pageSize, int pageNumber, List<KeyValuePair<string, string>> selectedFields, List<string> orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null) return new PageDataResult<TDTO>();
            TDAL dal = GetDAL(tableName, sqlProcess);
            PageDataResult<TDTO> result = _PageSplitSearch(dal, filter, pageSize, pageNumber, selectedFields, orderFields);
            return result;
        }
        public static int Insert(string tableName, ISqlPoolProcess sqlProcess, TDTO dbObj)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Insert(dal, dbObj);
        }
        public static int Insert(string tableName, ISqlPoolProcess sqlProcess, List<TDTO> dbObjs)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Insert(dal, dbObjs);
        }
        public static int Truncate(string tableName, ISqlPoolProcess sqlProcess)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Truncate(dal);
        }
        public static T GetMaxValue<T>(string tableName, ISqlPoolProcess sqlProcess, string field)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _GetMaxValue<T>(dal, field);
        }
        public static T GetMinValue<T>(string tableName, ISqlPoolProcess sqlProcess, string field)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _GetMinValue<T>(dal, field);
        }
        public static List<TDTO> GetAllData(string tableName, ISqlPoolProcess sqlProcess)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _GetAllData(dal);
        }
        public static int GetTotal<TFilter>(ISqlPoolProcess sqlProcess, string tableName, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return 0;
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _GetTotal(dal, filter);
        }
        public static int Update(ISqlPoolProcess sqlProcess, string tableName, TDTO dbObj, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Update(dal, dbObj, keyFields, updateFields);
        }
        public static int Update(ISqlPoolProcess sqlProcess, string tableName, List<TDTO> dbObjs, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Update(dal, dbObjs, keyFields, updateFields);
        }
        public static List<TStatResult> Calculate<TStatResult, TFilter>(ISqlPoolProcess sqlProcess, string tableName, TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new List<TStatResult>();
            }
            TDAL dal = GetDAL(tableName, sqlProcess);
            return _Calculate<TStatResult, TFilter>(dal, filter, statFields, calculateFields, orderFields);
        }


        #endregion

        #region TableName
        public static List<TDTO> Search<TFilter>(string tableName, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TDTO>();
            TDAL dal = GetDAL(tableName);
            return _Search(dal, filter);
        }
        public static List<TResult> Search<TResult, TFilter>(string tableName, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL(tableName);
            return _Search<TResult, TFilter>(dal, filter);
        }
        public static List<TResult> SearchIDs<TResult, TFilter>(string tableName, TFilter filter, params string[] resultFields)
            where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL(tableName);
            return _SearchIDs<TResult, TFilter>(dal, filter, resultFields);
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(string tableName, TFilter filter, int pageSize, int pageNumber, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null) return new PageDataResult<TDTO>();

            try
            {
                TDAL dal = GetDAL(tableName);
                PageDataResult<TDTO> result = _PageSplitSearch(dal, filter, pageSize, pageNumber, orderFields);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(string tableName, TFilter filter, int pageSize, int pageNumber, List<KeyValuePair<string, string>> selectedFields, List<string> orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null) return new PageDataResult<TDTO>();

            try
            {
                TDAL dal = GetDAL(tableName);
                PageDataResult<TDTO> result = _PageSplitSearch(dal, filter, pageSize, pageNumber, selectedFields, orderFields);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int Insert(string tableName, TDTO dbObj)
        {
            TDAL dal = GetDAL(tableName);
            return _Insert(dal, dbObj);
        }

        public static int Insert(string tableName, List<TDTO> dbObjs)
        {
            TDAL dal = GetDAL(tableName);
            return _Insert(dal, dbObjs);
        }

        public static int Truncate(string tableName)
        {
            TDAL dal = GetDAL(tableName);
            return _Truncate(dal);
        }

        public static T GetMaxValue<T>(string tableName, string field)
        {
            TDAL dal = GetDAL(tableName);
            return _GetMaxValue<T>(dal, field);
        }

        public static T GetMinValue<T>(string tableName, string field)
        {
            TDAL dal = GetDAL(tableName);
            return _GetMinValue<T>(dal, field);
        }

        public static List<TDTO> GetAllData(string tableName)
        {
            TDAL dal = GetDAL(tableName);
            return _GetAllData(dal);
        }

        public static int GetTotal<TFilter>(string tableName, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return 0;
            TDAL dal = GetDAL(tableName);
            return _GetTotal(dal, filter);
        }

        public static int Update(string tableName, TDTO dbObj, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL(tableName);
            return _Update(dal, dbObj, keyFields, updateFields);
        }

        public static int Update(string tableName, List<TDTO> dbObjs, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL(tableName);
            return _Update(dal, dbObjs, keyFields, updateFields);
        }

        public static List<TStatResult> Calculate<TStatResult, TFilter>(string tableName, TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new List<TStatResult>();
            }
            TDAL dal = GetDAL(tableName);
            List<TStatResult> result = _Calculate<TStatResult, TFilter>(dal, filter, statFields, calculateFields, orderFields);
            return result;
        } 
        #endregion

        #endregion
    }
}
