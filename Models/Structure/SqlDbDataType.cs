using System;
using System.Collections.Generic;
using System.Text;

namespace VSSystem.Data
{
    public class SqlDbDataType : SqlDbItem
    {
        long _length, _nPrecision, _nScale;
        ESqlDbDataType _eType;

        public long Length { get => _length; }
        public long NPrecision { get => _nPrecision; }
        public long NScale { get => _nScale; }
        public ESqlDbDataType EType { get => _eType; }

        public SqlDbDataType(string typeName, string charset, long length, long nprecision, long nscale, EDbProvider provider)
        {
            _length = length;
            _nPrecision = nprecision;
            _nScale = nscale;
            _eType = GetEType(typeName, charset, length, provider);
        }
        public SqlDbDataType(ESqlDbDataType eType, long length)
        {
            _length = length;
            _eType = eType;
        }
        public SqlDbDataType(ESqlDbDataType eType, long nPrecision, long nScale)
        {
            _nPrecision = nPrecision;
            _nScale = nScale;
            _eType = eType;
        }

        public override string[] ToSqlArrayString(EDbProvider provider)
        {
            try
            {
                string result = ToSqlString(provider);
                return new string[] { result };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override string ToSqlString(EDbProvider provider)
        {
            try
            {
                string result = "";
                if (_eType == ESqlDbDataType.Byte) result = "tinyint";
                else if (_eType == ESqlDbDataType.Int16) result = "smallint";
                else if (_eType == ESqlDbDataType.Int32) result = "int";
                else if (_eType == ESqlDbDataType.Int64) result = "bigint";
                else if (_eType == ESqlDbDataType.Decimal) result = string.Format("decimal({0},{1})", _nPrecision, _nScale);
                else if (_eType == ESqlDbDataType.Float) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? "float" : "real";
                else if (_eType == ESqlDbDataType.Double) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? "double" : "float";
                else if (_eType == ESqlDbDataType.Date) result = "date";
                else if (_eType == ESqlDbDataType.DateTime) result = "datetime";
                else if (_eType == ESqlDbDataType.TimeStamp) result = "timestamp";
                else if (_eType == ESqlDbDataType.Guid) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? "varchar(36)" : "uniqueidentifier";
                else if (_eType == ESqlDbDataType.String)
                {
                    if (_length == -1 || _length >= 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumtext character set ascii") : string.Format("char(max)");
                    else if (_length < 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("char({0}) character set ascii", _length) : string.Format("char({0})", _length);
                }
                else if (_eType == ESqlDbDataType.UnicodeString)
                {
                    if (_length == -1 || _length >= 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumtext character set utf8") : string.Format("nchar(max)");
                    else if (_length < 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("char({0}) character set utf8", _length) : string.Format("nchar({0})", _length);
                }
                else if (_eType == ESqlDbDataType.VString)
                {
                    if (_length == -1 || _length >= 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumtext character set ascii") : string.Format("varchar(max)");
                    else if (_length < 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("varchar({0}) character set ascii", _length) : string.Format("varchar({0})", _length);
                }
                else if (_eType == ESqlDbDataType.VUnicodeString)
                {
                    if (_length == -1 || _length >= 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumtext character set utf8") : string.Format("nvarchar(max)");
                    else if (_length < 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("varchar({0}) character set utf8", _length) : string.Format("nvarchar({0})", _length);
                }
                else if (_eType == ESqlDbDataType.Text) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumtext character set ascii", _length) : string.Format("varchar(max)", _length);
                else if (_eType == ESqlDbDataType.UnicodeText) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumtext character set utf8", _length) : string.Format("nvarchar(max)", _length);
                else if (_eType == ESqlDbDataType.Bytes)
                {
                    if (_length == -1 || _length >= 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumblob") : string.Format("binary(max)");
                    else if (_length < 8000) result = string.Format("binary({0})", _length);
                }
                else if (_eType == ESqlDbDataType.VBytes)
                {
                    if (_length == -1 || _length >= 8000) result = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? string.Format("mediumblob") : string.Format("varbinary(max)");
                    else if (_length < 8000) result = string.Format("varbinary({0})", _length);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ESqlDbDataType GetEType(string typeName, string charset, long length, EDbProvider provider)
        {
            try
            {
                ESqlDbDataType dType = ESqlDbDataType.Undefine;
                if (typeName.Equals("tinyint", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Byte; }
                else if (typeName.Equals("smallint", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Int16; }
                else if (typeName.Equals("int", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Int32; }
                else if (typeName.Equals("bigint", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Int64; }
                else if (typeName.Equals("float", StringComparison.InvariantCultureIgnoreCase)) { dType = provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB ? ESqlDbDataType.Float : ESqlDbDataType.Double; }
                else if (typeName.Equals("real", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Float; }
                else if (typeName.Equals("double", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Double; }
                else if (typeName.Equals("decimal", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Decimal; }
                else if (typeName.Equals("date", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Date; }
                else if (typeName.Equals("datetime", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.DateTime; }
                else if (typeName.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.TimeStamp; }
                else if (typeName.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.Guid; }
                else if (typeName.Equals("char", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (charset.Equals("utf8", StringComparison.InvariantCultureIgnoreCase) && (provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB)) dType = ESqlDbDataType.UnicodeString;
                    else dType = ESqlDbDataType.String;
                }
                else if (typeName.Equals("nchar", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.UnicodeString; }
                else if (typeName.Equals("varchar", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (length == 36 && (provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB))
                        dType = ESqlDbDataType.Guid;
                    else if (charset.Equals("utf8", StringComparison.InvariantCultureIgnoreCase) && (provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB))
                        dType = ESqlDbDataType.VUnicodeString;
                    else dType = ESqlDbDataType.VString;
                }
                else if (typeName.Equals("nvarchar", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.VUnicodeString; }
                else if (typeName.Equals("text", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (charset.Equals("utf8", StringComparison.InvariantCultureIgnoreCase) && (provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB)) dType = ESqlDbDataType.VUnicodeString;
                    else dType = ESqlDbDataType.Text;
                }
                else if (typeName.Equals("ntext", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.UnicodeText; }
                else if (typeName.Equals("mediumtext", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.UnicodeText; }
                else if (typeName.Equals("longtext", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.UnicodeText; ; }
                else if (typeName.Equals("binary", StringComparison.InvariantCultureIgnoreCase))
                {
                    dType = length == 16 && (provider == EDbProvider.Mysql || provider == EDbProvider.MariaDB) ? ESqlDbDataType.Guid : ESqlDbDataType.Bytes;
                }
                else if (typeName.Equals("varbinary", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.VBytes; }
                else if (typeName.Equals("blob", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.VBytes; }
                else if (typeName.Equals("mediumblob", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.VBytes; }
                else if (typeName.Equals("longblob", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.VBytes; }
                else if (typeName.Equals("image", StringComparison.InvariantCultureIgnoreCase)) { dType = ESqlDbDataType.VBytes; }
                return dType;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public SqlDbDataType()
        {

        }

        public string ToSystemType()
        {
            string result = "";
            switch (_eType)
            {
                case ESqlDbDataType.Byte: result = "byte"; break;
                case ESqlDbDataType.Bytes:
                case ESqlDbDataType.VBytes: result = "byte[]"; break;
                case ESqlDbDataType.String:
                case ESqlDbDataType.UnicodeString:
                case ESqlDbDataType.VString:
                case ESqlDbDataType.VUnicodeString:
                case ESqlDbDataType.Text:
                case ESqlDbDataType.UnicodeText: result = "string"; break;
                case ESqlDbDataType.Int16: result = "short"; break;
                case ESqlDbDataType.Int32: result = "int"; break;
                case ESqlDbDataType.Int64: result = "long"; break;
                case ESqlDbDataType.Decimal: result = "decimal"; break;
                case ESqlDbDataType.DateTime:
                case ESqlDbDataType.Date: result = "DateTime"; break;
                case ESqlDbDataType.Double: result = "double"; break;
                case ESqlDbDataType.Float: result = "float"; break;
                case ESqlDbDataType.Guid: result = "Guid"; break;
                default: result = ""; break;
            }
            return result;
        }
        public string GetDefaultValue()
        {
            string result = "";
            switch (_eType)
            {
                case ESqlDbDataType.Byte:
                case ESqlDbDataType.Int16:
                case ESqlDbDataType.Int32:
                case ESqlDbDataType.Int64:
                case ESqlDbDataType.Decimal:
                case ESqlDbDataType.Double:
                case ESqlDbDataType.Float: result = "0"; break;
                case ESqlDbDataType.Bytes:
                case ESqlDbDataType.VBytes: result = "null"; break;
                case ESqlDbDataType.String:
                case ESqlDbDataType.UnicodeString:
                case ESqlDbDataType.VString:
                case ESqlDbDataType.VUnicodeString:
                case ESqlDbDataType.Text:
                case ESqlDbDataType.UnicodeText: result = "string.Empty"; break;
                case ESqlDbDataType.DateTime:
                case ESqlDbDataType.Date: result = "DateTime.MinValue"; break;
                case ESqlDbDataType.Guid: result = "Guid.NewGuid()"; break;
                default: result = ""; break;
            }
            return result;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", _eType, _eType == ESqlDbDataType.Decimal ? _nPrecision + ", " + _nScale : _length.ToString());
        }

        public bool IsString()
        {
            return _eType == ESqlDbDataType.String || _eType == ESqlDbDataType.VString || _eType == ESqlDbDataType.Text || _eType == ESqlDbDataType.Guid;
        }
        public bool IsUnicodeString()
        {
            return _eType == ESqlDbDataType.UnicodeString || _eType == ESqlDbDataType.VUnicodeString || _eType == ESqlDbDataType.UnicodeText;
        }
    }
}
