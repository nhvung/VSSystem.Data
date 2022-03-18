using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data.DTO
{
    public class PageDataResult<TResult>
    {

        int _PageSize;
        public int PageSize { get { return _PageSize; } set { _PageSize = value; } }

        int _PageNumber;
        public int PageNumber { get { return _PageNumber; } set { _PageNumber = value; } }

        int _TotalPages;
        public int TotalPages { get { return _TotalPages; } set { _TotalPages = value; } }
        int _TotalRecords;
        public int TotalRecords { get { return _TotalRecords; } set { _TotalRecords = value; } }
        List<TResult> _Records;
        public List<TResult> Records { get { return _Records; } set { _Records = value; } }
        public PageDataResult()
        {
            _PageNumber = 0;
            _PageSize = 0;
            _Records = new List<TResult>();
            _TotalRecords = 0;
            _TotalPages = 0;
        }
    }
}
