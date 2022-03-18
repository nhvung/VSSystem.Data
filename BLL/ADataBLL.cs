using System;
using System.Collections.Generic;
using System.Reflection;
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
        protected static TDAL GetDAL()
        {
            return Activator.CreateInstance<TDAL>();
        }
        protected static TDAL GetDAL(params object[] pars)
        {
            return (TDAL)Activator.CreateInstance(typeof(TDAL), pars);
        }
        protected static TDAL GetDAL(string tableName)
        {
            TDAL dal = GetDAL(new object[] { tableName });
            return dal;
        }

        #region Methods
        protected static List<TDTO> _Search<TFilter>(TDAL dal, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TDTO>();
            List<TDTO> dbObjs = dal.Search(filter);
            return dbObjs;
        }
        protected static List<TResult> _Search<TResult, TFilter>(TDAL dal, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            List<TResult> dbObjs = dal.Search<TResult, TFilter>(filter);
            return dbObjs;
        }
        protected static List<TResult> _SearchIDs<TResult, TFilter>(TDAL dal, TFilter filter, params string[] resultFields)
            where TFilter : BaseFilter
        {
            if (filter == null) return new List<TResult>();
            List<TResult> dbObjs = dal.SearchIDs<TResult, TFilter>(filter, resultFields);
            return dbObjs;
        }
        protected static PageDataResult<TDTO> _PageSplitSearch<TFilter>(TDAL dal, TFilter filter, int pageSize, int pageNumber, string[] orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new PageDataResult<TDTO>();
            }

            PageDataResult<TDTO> result = new PageDataResult<TDTO>();

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
        protected static PageDataResult<TDTO> _PageSplitSearch<TFilter>(TDAL dal, TFilter filter, int pageSize, int pageNumber, List<KeyValuePair<string, string>> selectedFields, List<string> orderFields = null)
            where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new PageDataResult<TDTO>();
            }

            PageDataResult<TDTO> result = new PageDataResult<TDTO>();

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
                    result.Records = dal.SearchWithOrder(filter, selectedFields, orderFields);
                }
                else
                {
                    result.PageNumber = -1;
                }
            }


            return result;
        }
        protected static int _Insert(TDAL dal, TDTO dbObj)
        {
            int exec = dal.Insert(dbObj);
            return exec;
        }
        protected static int _Insert(TDAL dal, List<TDTO> dbObjs)
        {
            int exec = dal.Insert(dbObjs);
            return exec;
        }
        protected static int _Truncate(TDAL dal)
        {
            int exec = dal.Truncate();
            return exec;
        }
        protected static T _GetMaxValue<T>(TDAL dal, string field)
        {
            T result = dal.GetMaxValue<T>(field);
            return result;
        }
        protected static T _GetMinValue<T>(TDAL dal, string field)
        {
            T result = dal.GetMinValue<T>(field);
            return result;
        }
        protected static List<TDTO> _GetAllData(TDAL dal)
        {
            List<TDTO> result = dal.GetAllData();
            return result;
        }
        protected static int _GetTotal<TFilter>(TDAL dal, TFilter filter) where TFilter : BaseFilter
        {
            if (filter == null) return 0;
            int result = dal.GetTotal(filter);
            return result;
        }
        protected static int _Update(TDAL dal, TDTO dbObj, string[] keyFields, string[] updateFields)
        {
            int exec = dal.ExecuteUpdate(keyFields, updateFields, dbObj);
            return exec;
        }
        protected static int _Update(TDAL dal, List<TDTO> dbObjs, string[] keyFields, string[] updateFields)
        {
            int exec = dal.ExecuteUpdate(keyFields, updateFields, dbObjs);
            return exec;
        }
        protected static List<TStatResult> _Calculate<TStatResult, TFilter>(TDAL dal, TFilter filter, KeyValuePair<string, string>[] statFields, KeyValuePair<string, string>[] calculateFields, string[] orderFields = null)
           where TFilter : BaseFilter
        {
            if (filter == null)
            {
                return new List<TStatResult>();
            }
            List<TStatResult> result = dal.Calculate<TFilter, TStatResult>(filter, statFields, calculateFields, orderFields);
            return result;
        }
        #endregion

        
    }
}
