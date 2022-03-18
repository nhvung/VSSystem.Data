using System;
using System.Collections.Generic;
using System.Text;
using VSSystem.Data.Filters;

namespace VSSystem.Data.Monitoring.Filters
{
    public class LogFilter : BaseFilter
    {
        public class FieldsValues
        {

            List<string> _Fields;
            public List<string> Fields { get { return _Fields; } set { _Fields = value; } }

            List<string> _Values;
            public List<string> Values { get { return _Values; } set { _Values = value; } }
        }


        List<FieldsValues> _FieldsValuesItems;
        public List<FieldsValues> FieldsValuesItems { get { return _FieldsValuesItems; } set { _FieldsValuesItems = value; } }

        long _BeginTicks;
        public long BeginTicks { get { return _BeginTicks; } set { _BeginTicks = value; } }

        long _EndTicks;
        public long EndTicks { get { return _EndTicks; } set { _EndTicks = value; } }

        List<long> _IDs;
        public List<long> IDs { get { return _IDs; } set { _IDs = value; } }

        public LogFilter()
        {
            _FieldsValuesItems = new List<FieldsValues>();
            _BeginTicks = 0; ;
            _EndTicks = 0;
            _IDs = new List<long>();
        }

        protected override void InitFilters()
        {
            base.InitFilters();

            if(_BeginTicks > 0)
            {
                _arrayFilters.Add("CreatedTicks >= " + _BeginTicks);
            }

            if (_EndTicks > 0)
            {
                _arrayFilters.Add("CreatedTicks <= " + _EndTicks);
            }

            if (_FieldsValuesItems?.Count > 0)
            { 
                foreach(var fvItem in _FieldsValuesItems)
                {
                    string fields = "(" + string.Join(",", fvItem.Fields) + ")";
                    string values = "(" + string.Join(",", fvItem.Values) + ")";
                    string ft = "(" + fields + " in " + values + ")";
                    _arrayFilters.Add(ft);
                }
            }

            if(_IDs?.Count > 0)
            {
                string ft = BaseFilter.GetFilter("ID", _IDs.ToArray());
                if(!string.IsNullOrWhiteSpace(ft))
                {
                    _arrayFilters.Add(ft);
                }
            }
        }
    }
}
