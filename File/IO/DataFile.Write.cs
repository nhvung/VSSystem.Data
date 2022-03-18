using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VSSystem.Data.File
{
    public partial class DataFile
    {
        #region Base Write Methods
        static void _Write(FileStream fs, bool value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, byte value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, byte[] buffer)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(buffer);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, char value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, decimal value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, double value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, float value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, int value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, long value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, sbyte value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, short value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, string value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, uint value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, ulong value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _Write(FileStream fs, ushort value)
        {
            try
            {
                using (var bw = new BinaryWriter(fs, Encoding.UTF8, true))
                {
                    bw.Write(value);
                    bw.Close();
                    bw.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static void _WriteObject(FileStream fs, object value)
        {

            try
            {
                if (value != null)
                {
                    var objType = value.GetType();
                    if (objType == typeof(bool))
                    {
                        _Write(fs, (bool)value);
                    }
                    else if (objType == typeof(byte))
                    {
                        _Write(fs, (byte)value);
                    }
                    else if (objType == typeof(byte[]))
                    {
                        _Write(fs, (byte[])value);
                    }
                    else if (objType == typeof(char))
                    {
                        _Write(fs, (char)value);
                    }
                    else if (objType == typeof(decimal))
                    {
                        _Write(fs, (decimal)value);
                    }
                    else if (objType == typeof(double))
                    {
                        _Write(fs, (double)value);
                    }
                    else if (objType == typeof(float))
                    {
                        _Write(fs, (float)value);
                    }
                    else if (objType == typeof(int))
                    {
                        _Write(fs, (int)value);
                    }
                    else if (objType == typeof(long))
                    {
                        _Write(fs, (long)value);
                    }
                    else if (objType == typeof(sbyte))
                    {
                        _Write(fs, (sbyte)value);
                    }
                    else if (objType == typeof(short))
                    {
                        _Write(fs, (short)value);
                    }
                    else if (objType == typeof(string))
                    {
                        _Write(fs, (string)value);
                    }
                    else if (objType == typeof(uint))
                    {
                        _Write(fs, (uint)value);
                    }
                    else if (objType == typeof(ulong))
                    {
                        _Write(fs, (ulong)value);
                    }
                    else if (objType == typeof(ushort))
                    {
                        _Write(fs, (ushort)value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        public static long Write(FileInfo file, object value, Action<Exception> errorLogAction = default)
        {
            long position = -1;
            try
            {
                using (var fs = file.Open(FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read))
                {
                    fs.Seek(0, SeekOrigin.End);
                    position = fs.Position;

                    try
                    {
                        _WriteObject(fs, value);
                    }
                    catch(Exception ex)
                    {
                        position = -1;
                        errorLogAction?.Invoke(ex);
                    }
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return position;
        }
        public static long Write(string fileName, object value)
        {
            return Write(new FileInfo(fileName), value);
        }
    }
}
