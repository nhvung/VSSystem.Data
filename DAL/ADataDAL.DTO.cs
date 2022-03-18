using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VSSystem.Data.DTO;
using VSSystem.Data.Filters;

namespace VSSystem.Data.DAL
{
    public abstract class ADataDAL<TDTO> : ADataDAL 
        where TDTO : DataDTO
    {
        protected string _TableName;
        protected string _CreateTableStatements = string.Empty;
        protected bool _AutoCreateTable;
        protected int _insertBlockSize, _updateBlockSize;

        protected SqlDbTable _TableStructure;

        protected ADataDAL(string tableName, ISqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = tableName;
            _insertBlockSize = 1000;
            _updateBlockSize = 1000;
        }
        protected ADataDAL(ISqlPoolProcess sqlProcess) : base(sqlProcess)
        {
            _TableName = string.Empty;
            _insertBlockSize = 1000;
            _updateBlockSize = 1000;
        }
        protected virtual void _InitTableStructure()
        {
            if(_TableStructure != null)
            {
                _CreateTableStatements = _TableStructure.ToSqlString(_provider);
            }
        }
        protected List<string> _GetTypeFields()
        {
            Type type = typeof(TDTO);
            return type.GetProperties().Select(ite => ite.Name).ToList();
        }
        abstract public  int Insert(TDTO dbObj);
        abstract public int Insert(List<TDTO> dbObjs);
        abstract protected int InsertRetry(TDTO dbObj);
        abstract protected int InsertRetry(List<TDTO> dbObjs);
        abstract public List<TDTO> Search<TFilter>(TFilter filter) where TFilter : BaseFilter;
        abstract public List<TDTO> SearchWithOrder<TFilter>(TFilter filter, string[] orderFields) where TFilter : BaseFilter;
        abstract public List<TDTO> SearchWithOrder<TFilter>(TFilter filter, List<KeyValuePair<string,string>> selectedFields, List<string> orderFields) where TFilter : BaseFilter;
        abstract public List<TResult> Search<TResult, TFilter>(TFilter filter) where TFilter : BaseFilter;
        abstract public List<TResult> SearchIDs<TResult, TFilter>(TFilter filter, string[] resultFields) where TFilter : BaseFilter;
        abstract public List<TDTO> GetAllData();
        abstract public int Truncate();
        abstract public int ExecuteUpdate(string[] keyFields, string[] updateFields, TDTO dbObj);
        abstract public int ExecuteUpdate(string[] keyFields, string[] updateFields, List<TDTO> dbObjs, int updateBlockSize = 50);
        abstract public TResult GetMaxValue<TResult>(string field);
        abstract public TResult GetMinValue<TResult>(string field);
        abstract public int GetTotal<TFilter>(TFilter filter) where TFilter : BaseFilter;
        public int CreateTable()
        {
            try
            {
                if (!string.IsNullOrEmpty(_CreateTableStatements))
                {
                    string query = string.Format(_CreateTableStatements, _TableName);
                    return ExecuteNonQuery(query);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        abstract protected bool _IsTableExistsException(Exception ex);
    }
}
