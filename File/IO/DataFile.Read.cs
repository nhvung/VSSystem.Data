using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSSystem.Data.File
{
    public partial class DataFile
    {
        #region Base Read Methods
        static bool _ReadBoolean(FileStream fs)
        {
            bool result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadBoolean();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static byte _ReadByte(FileStream fs)
        {
            byte result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadByte();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static byte[] _ReadBytes(FileStream fs)
        {
            byte[] result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadBytes();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static char _ReadChar(FileStream fs)
        {
            char result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadChar();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static decimal _ReadDecimal(FileStream fs)
        {
            decimal result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadDecimal();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static double _ReadDouble(FileStream fs)
        {
            double result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadDouble();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static float _ReadSingle(FileStream fs)
        {
            float result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadSingle();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static int _ReadInt32(FileStream fs)
        {
            int result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadInt32();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static long _ReadInt64(FileStream fs)
        {
            long result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadInt64();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static sbyte _ReadSByte(FileStream fs)
        {
            sbyte result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadSByte();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static short _ReadInt16(FileStream fs)
        {
            short result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadInt16();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static string _ReadString(FileStream fs)
        {
            string result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadString();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static uint _ReadUInt32(FileStream fs)
        {
            uint result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadUInt32();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static ulong _ReadUInt64(FileStream fs)
        {
            ulong result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadUInt64();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        static ushort _ReadUInt16(FileStream fs)
        {
            ushort result;
            try
            {
                using (var bw = new BinaryReader(fs, Encoding.UTF8, true))
                {
                    result = bw.ReadUInt16();
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        static TObject _ReadObject<TObject>(FileStream fs)
        {
            TObject result = default;
            var objType = typeof(TObject);
            if (objType == typeof(bool))
            {
                result = (TObject)Convert.ChangeType(_ReadBoolean(fs), objType);
            }
            else if (objType == typeof(byte))
            {
                result = (TObject)Convert.ChangeType(_ReadByte(fs), objType);
            }
            else if (objType == typeof(byte[]))
            {
                result = (TObject)Convert.ChangeType(_ReadBytes(fs), objType);
            }
            else if (objType == typeof(char))
            {
                result = (TObject)Convert.ChangeType(_ReadChar(fs), objType);
            }
            else if (objType == typeof(decimal))
            {
                result = (TObject)Convert.ChangeType(_ReadDecimal(fs), objType);
            }
            else if (objType == typeof(double))
            {
                result = (TObject)Convert.ChangeType(_ReadDouble(fs), objType);
            }
            else if (objType == typeof(float))
            {
                result = (TObject)Convert.ChangeType(_ReadSingle(fs), objType);
            }
            else if (objType == typeof(int))
            {
                result = (TObject)Convert.ChangeType(_ReadInt32(fs), objType);
            }
            else if (objType == typeof(long))
            {
                result = (TObject)Convert.ChangeType(_ReadInt64(fs), objType);
            }
            else if (objType == typeof(sbyte))
            {
                result = (TObject)Convert.ChangeType(_ReadSByte(fs), objType);
            }
            else if (objType == typeof(short))
            {
                result = (TObject)Convert.ChangeType(_ReadInt16(fs), objType);
            }
            else if (objType == typeof(string))
            {
                result = (TObject)Convert.ChangeType(_ReadString(fs), objType);
            }
            else if (objType == typeof(uint))
            {
                result = (TObject)Convert.ChangeType(_ReadUInt32(fs), objType);
            }
            else if (objType == typeof(ulong))
            {
                result = (TObject)Convert.ChangeType(_ReadUInt64(fs), objType);
            }
            else if (objType == typeof(ushort))
            {
                result = (TObject)Convert.ChangeType(_ReadUInt16(fs), objType);
            }
            return result;
        }
        #endregion

        public static TObject Read<TObject>(FileInfo file)
        {
            TObject result = default;
            try
            {
                using (var fs = file.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    result = _ReadObject<TObject>(fs);
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        public static TObject Read<TObject>(string fileName)
        {
            return Read<TObject>(new FileInfo(fileName));
        }
    }
}
