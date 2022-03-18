using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbColumn : SqlDbItem
    {
        SqlDbDataType _type;
        bool _isNullable;
        bool _isPrimaryKey;
        bool _isIdentity;
        public SqlDbDataType Type
        {
            get { return _type; }
        }
        public bool IsNullable
        {
            get { return _isNullable; }
        }
        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
        }
        public bool IsIdentity
        {
            get { return _isIdentity; }
        }

        public SqlDbColumn(string name, string dataType, string charset, long length, long nPrecision, long nScale, string isNullable, string columnKey, int isIdentity, EDbProvider provider) : base()
        {
            _name = name;
            _type = new SqlDbDataType(dataType, charset, length, nPrecision, nScale, provider);
            _isNullable = isNullable.Equals("yes", StringComparison.InvariantCultureIgnoreCase);
            _isPrimaryKey = columnKey.Equals("pri", StringComparison.InvariantCultureIgnoreCase);
            _isIdentity = isIdentity == 1;
        }
        public SqlDbColumn(string name, ESqlDbDataType eType, long length, bool isPrimaryKey, bool isNullable, bool isIdentity)
        {
            _name = name;
            _isPrimaryKey = isPrimaryKey;
            _isNullable = isNullable;
            _isIdentity = isIdentity;
            _type = new SqlDbDataType(eType, length);
        }
        public SqlDbColumn(string name, ESqlDbDataType eType, long nPrecision, long nScale, bool isPrimaryKey, bool isNullable, bool isIdentity)
        {
            _name = name;
            _isPrimaryKey = isPrimaryKey;
            _isNullable = isNullable;
            _isIdentity = isIdentity;
            _type = new SqlDbDataType(eType, nPrecision, nScale);
        }

        public override string[] ToSqlArrayString(EDbProvider provider)
        {
            string type = _type.ToSqlString(provider);
            string nullableString = _isNullable ? "" : " not null";
            return new string[] { string.Format("{0} {1}{2}", _name, type, nullableString) };
        }
        public override string ToSqlString(EDbProvider provider)
        {
            string type = _type.ToSqlString(provider);
            string nullableString = _isNullable ? "" : " not null";
            return string.Format("{0} {1}{2}", _name, type, nullableString);
        }



        public enum LineType { PRIVATELINE, PUBLICLINE, INITLINE }
        public Dictionary<LineType, string> ToClassField()
        {
            Dictionary<LineType, string> fieldProps = new Dictionary<LineType, string>();
            string type = _type.ToSystemType();
            string defaultValue = _type.GetDefaultValue();
            fieldProps[LineType.INITLINE] = string.Format("_{0} = {1};", _name, defaultValue);
            fieldProps[LineType.PRIVATELINE] = string.Format("{0} _{1};", type, _name);
            fieldProps[LineType.PUBLICLINE] = string.Format("public {0} {1} {{ get {{ return _{1}; }} set {{ _{1} = value; }} }}", type, _name);
            return fieldProps;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", _name, _type, _isNullable);
        }
    }
}
