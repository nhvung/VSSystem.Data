using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbRecord
    {
        protected PropertyInfo[] _properties;
        protected string[] _dataFields;
        protected virtual void InitEmptyValue()
        {
            _properties = GetType().GetProperties();
            _dataFields = _properties.Select(ite => ite.Name).ToArray();
        }
        public SqlDbRecord() { InitEmptyValue(); }
        public string[] GetDbFields() { return _dataFields; }
        public virtual string ToDbValues(params string[] fields)
        {
            if (fields == null || fields.Length == 0) fields = _dataFields;
            List<string> values = new List<string>();
            string value = "";
            Dictionary<string, PropertyInfo> m_property = _properties.ToDictionary(ite => ite.Name, ite => ite, StringComparer.InvariantCultureIgnoreCase);

            foreach (string field in fields)
            {
                if (m_property.ContainsKey(field))
                {
                    PropertyInfo property = m_property[field];
                    try
                    {
                        object objVal = property.GetValue(this, null);
                        if (property.PropertyType == typeof(string))
                        {
                            if (objVal == null) values.Add("''");
                            else
                            {
                                value = objVal.ToString();
                                value = value.Replace("'", "''").Replace("\\", "\\\\");
                                values.Add("N'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(double))
                        {
                            if (objVal == null) values.Add("'0'");
                            else
                            {
                                value = objVal.ToString();
                                value = value.Replace("'", "''").Replace("\\", "\\\\");
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(Guid))
                        {
                            if (objVal == null)
                            {
                                value = new Guid().ToString();
                                values.Add("'" + value + "'");
                            }
                            else
                            {
                                value = ((Guid)objVal).ToString();
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                DateTime dt = (DateTime)objVal;
                                if (dt < new DateTime(1970, 1, 1))
                                {
                                    dt = new DateTime(1970, 1, 1);
                                }
                                value = (dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(byte[]))
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                value = string.Format("0x{0}", BitConverter.ToString((byte[])objVal).Replace("-", ""));
                                values.Add(value);
                            }
                        }
                        else if (property.PropertyType == typeof(char[]))
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                value = new string((char[])objVal);
                                value = value.Replace("'", "''").Replace("\\", "\\\\");
                                values.Add("N'" + value + "'");
                            }
                        }
                        else
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                value = objVal.ToString();
                                values.Add(value);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }


            value = "(" + string.Join(",", values.ToArray()) + ")";
            return value;
        }
        public virtual string ToSqliteDbValues(params string[] fields)
        {
            if (fields == null || fields.Length == 0) fields = _dataFields;
            List<string> values = new List<string>();
            string value = "";
            Dictionary<string, PropertyInfo> m_property = _properties.ToDictionary(ite => ite.Name, ite => ite, StringComparer.InvariantCultureIgnoreCase);

            foreach (string field in fields)
            {
                if (m_property.ContainsKey(field))
                {
                    PropertyInfo property = m_property[field];
                    try
                    {
                        object objVal = property.GetValue(this, null);
                        if (property.PropertyType == typeof(string))
                        {
                            if (objVal == null) values.Add("''");
                            else
                            {
                                value = objVal.ToString();
                                value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(float) || property.PropertyType == typeof(double))
                        {
                            if (objVal == null) values.Add("'0'");
                            else
                            {
                                value = objVal.ToString();
                                value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(Guid))
                        {
                            if (objVal == null)
                            {
                                value = new Guid().ToString();
                                values.Add("'" + value + "'");
                            }
                            else
                            {
                                value = ((Guid)objVal).ToString();
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(DateTime))
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                DateTime dt = (DateTime)objVal;
                                if (dt < new DateTime(1970, 1, 1))
                                {
                                    dt = new DateTime(1970, 1, 1);
                                }
                                value = (dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                values.Add("'" + value + "'");
                            }
                        }
                        else if (property.PropertyType == typeof(byte[]))
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                value = string.Format("X'{0}'", BitConverter.ToString((byte[])objVal).Replace("-", ""));
                                values.Add(value);
                            }
                        }
                        else if (property.PropertyType == typeof(char[]))
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                value = new string((char[])objVal);
                                value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                                values.Add("N'" + value + "'");
                            }
                        }
                        else
                        {
                            if (objVal == null) values.Add("null");
                            else
                            {
                                value = objVal.ToString();
                                values.Add(value);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
            value = "(" + string.Join(",", values.ToArray()) + ")";
            return value;
        }
        public static List<string> ToDbValues(string[] fields, Type dataType, params object[] objs)
        {
            if (fields == null || fields.Length == 0) return new List<string>();
            if (objs == null || objs.Length == 0) return new List<string>();


            var props = dataType.GetProperties();

            Dictionary<string, PropertyInfo> m_property = props.ToDictionary(ite => ite.Name, ite => ite, StringComparer.InvariantCultureIgnoreCase);

            List<string> result = new List<string>();

            List<string> values = new List<string>();
            foreach (var obj in objs)
            {
                string value = "";
                values = new List<string>();

                foreach (string field in fields)
                {
                    if (m_property.ContainsKey(field))
                    {
                        PropertyInfo property = m_property[field];
                        try
                        {
                            object objVal = property.GetValue(obj, null);
                            if (property.PropertyType == typeof(string))
                            {
                                if (objVal == null) values.Add("''");
                                else
                                {
                                    value = objVal.ToString();
                                    value = value.Replace("'", "''").Replace("\\", "\\\\");
                                    values.Add("N'" + value + "'");
                                }
                            }
                            else if (property.PropertyType == typeof(Guid))
                            {
                                if (objVal == null)
                                {
                                    value = new Guid().ToString();
                                    values.Add("'" + value + "'");
                                }
                                else
                                {
                                    value = ((Guid)objVal).ToString();
                                    values.Add("'" + value + "'");
                                }
                            }
                            else if (property.PropertyType == typeof(DateTime))
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    DateTime dt = (DateTime)objVal;
                                    if (dt < new DateTime(1970, 1, 1)) dt = new DateTime(1970, 1, 1);
                                    value = (dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    values.Add("'" + value + "'");
                                }
                            }
                            else if (property.PropertyType == typeof(byte[]))
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    value = string.Format("0x{0}", BitConverter.ToString((byte[])objVal).Replace("-", ""));
                                    values.Add(value);
                                }
                            }
                            else if (property.PropertyType == typeof(char[]))
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    value = new string((char[])objVal);
                                    value = value.Replace("'", "''").Replace("\\", "\\\\");
                                    values.Add("N'" + value + "'");
                                }
                            }
                            else
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    value = objVal.ToString();
                                    values.Add(value);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        values.Add("NULL");
                    }
                }

                value = "(" + string.Join(",", values.ToArray()) + ")";
                result.Add(value);
            }
            return result;
        }
        public static List<string> ToSqliteDbValues(string[] fields, Type dataType, params object[] objs)
        {
            if (fields == null || fields.Length == 0) return new List<string>();
            if (objs == null || objs.Length == 0) return new List<string>();


            var props = dataType.GetProperties();

            Dictionary<string, PropertyInfo> m_property = props.ToDictionary(ite => ite.Name, ite => ite, StringComparer.InvariantCultureIgnoreCase);

            List<string> result = new List<string>();

            List<string> values = new List<string>();
            foreach (var obj in objs)
            {
                string value = "";
                values = new List<string>();

                foreach (string field in fields)
                {
                    if (m_property.ContainsKey(field))
                    {
                        PropertyInfo property = m_property[field];
                        try
                        {
                            object objVal = property.GetValue(obj, null);
                            if (property.PropertyType == typeof(string))
                            {
                                if (objVal == null) values.Add("''");
                                else
                                {
                                    value = objVal.ToString();
                                    value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                                    values.Add("N'" + value + "'");
                                }
                            }
                            else if (property.PropertyType == typeof(Guid))
                            {
                                if (objVal == null)
                                {
                                    value = new Guid().ToString();
                                    values.Add("'" + value + "'");
                                }
                                else
                                {
                                    value = ((Guid)objVal).ToString();
                                    values.Add("'" + value + "'");
                                }
                            }
                            else if (property.PropertyType == typeof(DateTime))
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    DateTime dt = (DateTime)objVal;
                                    if (dt < new DateTime(1970, 1, 1)) dt = new DateTime(1970, 1, 1);
                                    value = (dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                    values.Add("'" + value + "'");
                                }
                            }
                            else if (property.PropertyType == typeof(byte[]))
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    value = string.Format("X'{0}'", BitConverter.ToString((byte[])objVal).Replace("-", ""));
                                    values.Add(value);
                                }
                            }
                            else if (property.PropertyType == typeof(char[]))
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    value = new string((char[])objVal);
                                    value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                                    values.Add("N'" + value + "'");
                                }
                            }
                            else
                            {
                                if (objVal == null) values.Add("null");
                                else
                                {
                                    value = objVal.ToString();
                                    values.Add(value);
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        values.Add("NULL");
                    }
                }

                value = "(" + string.Join(",", values.ToArray()) + ")";
                result.Add(value);
            }
            return result;
        }
        public virtual string ToUpdateDbValues(string joinString, params string[] fields)
        {
            if (fields == null || fields.Length == 0) fields = _dataFields;
            List<string> values = new List<string>();
            string value = "";
            foreach (PropertyInfo property in _properties)
            {
                if (!fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase)) continue;
                try
                {
                    object objVal = property.GetValue(this, null);
                    if (property.PropertyType == typeof(string))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = objVal.ToString();
                            value = value.Replace("'", "''").Replace("\\", "\\\\");
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(Guid))
                    {
                        if (objVal == null)
                        {
                            value = new Guid().ToString();
                            values.Add(property.Name + "='" + value + "'");
                        }
                        else
                        {
                            value = ((Guid)objVal).ToString();
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            DateTime dt = (DateTime)objVal;
                            if (dt < new DateTime(1970, 1, 1)) dt = new DateTime(1970, 1, 1);
                            value = (dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(byte[]))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = string.Format("0x{0}", BitConverter.ToString((byte[])objVal).Replace("-", ""));
                            values.Add(property.Name + "=" + value);
                        }
                    }
                    else if (property.PropertyType == typeof(char[]))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = new string((char[])objVal);
                            value = value.Replace("'", "''").Replace("\\", "\\\\");
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = objVal.ToString();
                            values.Add(property.Name + "=" + value);
                        }
                    }
                }
                catch
                {

                }
            }
            value = string.Join(joinString, values.ToArray());
            return value;
        }
        public virtual string ToUpdateSqliteDbValues(string joinString, params string[] fields)
        {
            if (fields == null || fields.Length == 0) fields = _dataFields;
            List<string> values = new List<string>();
            string value = "";
            foreach (PropertyInfo property in _properties)
            {
                if (!fields.Contains(property.Name, StringComparer.InvariantCultureIgnoreCase)) continue;
                try
                {
                    object objVal = property.GetValue(this, null);
                    if (property.PropertyType == typeof(string))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = objVal.ToString();
                            value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(Guid))
                    {
                        if (objVal == null)
                        {
                            value = new Guid().ToString();
                            values.Add(property.Name + "='" + value + "'");
                        }
                        else
                        {
                            value = ((Guid)objVal).ToString();
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            DateTime dt = (DateTime)objVal;
                            if (dt < new DateTime(1970, 1, 1)) dt = new DateTime(1970, 1, 1);
                            value = (dt).ToString("yyyy-MM-dd HH:mm:ss.fff");
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(byte[]))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = string.Format("X'{0}'", BitConverter.ToString((byte[])objVal).Replace("-", ""));
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else if (property.PropertyType == typeof(char[]))
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = new string((char[])objVal);
                            value = value.Replace("'", "''");//.Replace("\\", "\\\\");
                            values.Add(property.Name + "='" + value + "'");
                        }
                    }
                    else
                    {
                        if (objVal == null) values.Add(property.Name + "=" + "null");
                        else
                        {
                            value = objVal.ToString();
                            values.Add(property.Name + "=" + value);
                        }
                    }
                }
                catch
                {

                }
            }
            value = string.Join(joinString, values.ToArray());
            return value;
        }
    }
}
