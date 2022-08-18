using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.DAL;
using VSSystem.Data.DTO;
using VSSystem.Data.Filters;

namespace VSSystem.Data.BLL
{
    public class DataBLL<TDAL, TDTO> : ADataBLL<TDAL, TDTO>
        where TDAL : DataDAL<TDTO>
        where TDTO : DataDTO
    {

        #region Default Methods
        public static List<TDTO> Search<TFilter>(TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TDTO>();
            TDAL dal = GetDAL();
            List<TDTO> dbObjs = dal.Search(filter);
            return dbObjs;
        }
        public static List<TResult> Search<TResult, TFilter>(TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL();
            List<TResult> dbObjs = dal.Search<TResult, TFilter>(filter);
            return dbObjs;
        }
        public static List<TResult> SearchIDs<TResult, TFilter>(TFilter filter, params string[] resultFields)
            where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            TDAL dal = GetDAL();
            List<TResult> dbObjs = dal.SearchIDs<TResult, TFilter>(filter, resultFields);
            return dbObjs;
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(TFilter filter, int pageSize, int pageNumber, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new PageDataResult<TDTO>();
            }

            PageDataResult<TDTO> result = new PageDataResult<TDTO>();
            TDAL dal = GetDAL();

            int total = dal.GetTotal(filter);
            if (total > 0)
            {
                result.PageSize = pageSize;
                result.TotalRecords = total;
                int totalPages = (total - 1) / pageSize + 1;
                result.TotalPages = totalPages;

                if (pageNumber <= totalPages)
                {
                    result.PageNumber = pageNumber;
                    filter.FromIndex = (pageNumber - 1) * pageSize + 1;
                    filter.ToIndex = pageNumber * pageSize;

                    if (filter.ToIndex > total)
                    {
                        filter.ToIndex = total;
                    }
                    result.Records = dal.SearchWithOrder(filter, orderFields);
                }
                else
                {
                    result.PageNumber = -1;
                }
            }


            return result;
        }
        public static PageDataResult<TDTO> PageSplitSearch<TFilter>(TFilter filter, int pageSize, int pageNumber, List<KeyValuePair<string, string>> selectedFields, out string outQuery, List<string> orderFields = null)
            where TFilter : BaseFilter
        {
            outQuery = string.Empty;
            if (filter == null)
            {
                return new PageDataResult<TDTO>();
            }

            PageDataResult<TDTO> result = new PageDataResult<TDTO>();
            TDAL dal = GetDAL();


            int total = dal.GetTotal(filter, selectedFields, out outQuery);
            if (total > 0)
            {
                result.PageSize = pageSize;
                result.TotalRecords = total;
                int totalPages = (total - 1) / pageSize + 1;
                result.TotalPages = totalPages;

                if (pageNumber <= totalPages)
                {
                    result.PageNumber = pageNumber;
                    filter.FromIndex = (pageNumber - 1) * pageSize + 1;
                    filter.ToIndex = pageNumber * pageSize;

                    if (filter.ToIndex > total)
                    {
                        filter.ToIndex = total;
                    }
                    result.Records = dal.SearchWithOrder(filter, selectedFields, orderFields);
                }
                else
                {
                    result.PageNumber = -1;
                }
            }


            return result;
        }

        public static int Insert(TDTO dbObj)
        {
            TDAL dal = GetDAL();
            int exec = dal.Insert(dbObj);
            return exec;
        }
        public static int Insert(List<TDTO> dbObjs)
        {
            TDAL dal = GetDAL();
            int exec = dal.Insert(dbObjs);
            return exec;
        }
        public static int Truncate()
        {
            TDAL dal = GetDAL();
            int exec = dal.Truncate();
            return exec;
        }
        public static T GetMaxValue<T>(string field)
        {
            TDAL dal = GetDAL();
            T result = dal.GetMaxValue<T>(field);
            return result;
        }
        public static T GetMinValue<T>(string field)
        {
            TDAL dal = GetDAL();
            T result = dal.GetMinValue<T>(field);
            return result;
        }
        public static List<TDTO> GetAllData()
        {
            TDAL dal = GetDAL();
            List<TDTO> result = dal.GetAllData();
            return result;
        }
        public static int GetTotal<TFilter>(TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return 0;
            TDAL dal = GetDAL();
            int result = dal.GetTotal(filter);
            return result;
        }
        public static int Update(TDTO dbObj, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL();
            int exec = dal.ExecuteUpdate(keyFields, updateFields, dbObj);
            return exec;
        }
        public static int Update(List<TDTO> dbObjs, string[] keyFields, string[] updateFields)
        {
            TDAL dal = GetDAL();
            int exec = dal.ExecuteUpdate(keyFields, updateFields, dbObjs);
            return exec;
        }
        public static List<TStatResult> Calculate<TFilter, TStatResult>(TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
           where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new List<TStatResult>();
            }
            TDAL dal = GetDAL();
            List<TStatResult> result = dal.Calculate<TFilter, TStatResult>(filter, statFields, calculateFields, orderFields);
            return result;
        }
        #endregion
    }
}
