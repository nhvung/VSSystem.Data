using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbResult
    {
        string[] _fields;
        List<object[]> _values;

        public SqlDbResult()
        {
            _fields = new string[0];
            _values = new List<object[]>();
        }
        public SqlDbResult(DbDataReader reader)
        {
            try
            {
                _fields = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++) _fields[i] = reader.GetName(i);
                object[] objs;
                _values = new List<object[]>();
                while (reader.Read())
                {
                    objs = new object[_fields.Length];
                    reader.GetValues(objs);
                    _values.Add(objs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public SqlDbResult(IDataReader reader)
        {
            try
            {
                _fields = new string[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++) _fields[i] = reader.GetName(i);
                object[] objs;
                _values = new List<object[]>();
                while (reader.Read())
                {
                    objs = new object[_fields.Length];
                    reader.GetValues(objs);
                    _values.Add(objs);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string[] Fields
        {
            get
            {
                return _fields;
            }

            set
            {
                _fields = value;
            }
        }

        public List<object[]> Values
        {
            get
            {
                return _values;
            }

            set
            {
                _values = value;
            }
        }
    }
}
