using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VSSystem.Data
{
    public static class SqlDbResultExtension
    {
        public static List<TResult> Cast<TResult>(this SqlDbResult source)
        {
            try
            {
                Type rType = typeof(TResult);
                List<TResult> result = new List<TResult>();
                if (rType == typeof(byte[]) || rType.IsValueType || rType == typeof(string))
                {
                    try
                    {
                        result = source.Values.Select(ite => (TResult)ite[0]).ToList();
                    }
                    catch
                    {
                        result = source.Values.Select(ite => (TResult)Convert.ChangeType(ite[0].ToString(), rType)).ToList();
                    }
                }
                else
                {
                    Dictionary<string, int> rFields = source.Fields.Distinct(StringComparer.InvariantCultureIgnoreCase).Select((ite, idx) => new { ite, idx }).ToDictionary(ite => ite.ite, ite => ite.idx, StringComparer.InvariantCultureIgnoreCase);
                    PropertyInfo[] fields = rType.GetProperties();
                    object value;
                    foreach (object[] objs in source.Values)
                    {
                        TResult res = Activator.CreateInstance<TResult>();
                        foreach (PropertyInfo field in fields)
                        {
                            if (!rFields.ContainsKey(field.Name)) continue;

                            try
                            {
                                try { value = objs[rFields[field.Name]]; }
                                catch { value = null; }
                                value = value == DBNull.Value || value == null
                                        ? field.PropertyType == typeof(string) ? Activator.CreateInstance(field.PropertyType, string.Empty.ToArray())
                                        : field.PropertyType == typeof(byte[]) ? new byte[0]
                                        : field.PropertyType == typeof(char[]) ? new char[0]
                                        : Activator.CreateInstance(field.PropertyType)
                                        : value;
                                if (field.PropertyType.IsEnum)
                                {
                                    field.SetValue(res, Enum.ToObject(field.PropertyType, value), null);
                                }
                                else if (field.PropertyType == typeof(Guid))
                                {
                                    Guid gValue;
                                    if (value.GetType() == typeof(byte[]))
                                        gValue = new Guid((byte[])value);
                                    else
                                        gValue = new Guid(value.ToString());
                                    field.SetValue(res, gValue, null);
                                }
                                else if (field.PropertyType == typeof(byte[]))
                                {
                                    field.SetValue(res, value, null);
                                }
                                else if (field.PropertyType == typeof(char[]))
                                {
                                    field.SetValue(res, value, null);
                                }
                                
                                else
                                {
                                    field.SetValue(res, Convert.ChangeType(value.ToString(), field.PropertyType), null);
                                }
                            }
                            catch
                            {

                            }
                        }
                        result.Add(res);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<object> Cast(this SqlDbResult source, Type dtoType)
        {
            try
            {
                List<object> result = new List<object>();
                if (dtoType == typeof(byte[]) || dtoType.IsValueType || dtoType == typeof(string))
                {
                    try
                    {
                        result = source.Values.Select(ite => ite[0]).ToList();
                    }
                    catch
                    {
                        result = source.Values.Select(ite => Convert.ChangeType(ite[0].ToString(), dtoType)).ToList();
                    }
                }
                else
                {
                    Dictionary<string, int> rFields = source.Fields.Distinct(StringComparer.InvariantCultureIgnoreCase).Select((ite, idx) => new { ite, idx }).ToDictionary(ite => ite.ite, ite => ite.idx, StringComparer.InvariantCultureIgnoreCase);
                    PropertyInfo[] fields = dtoType.GetProperties();
                    object value;
                    foreach (object[] objs in source.Values)
                    {
                        object res = Activator.CreateInstance(dtoType);
                        foreach (PropertyInfo field in fields)
                        {
                            if (!rFields.ContainsKey(field.Name)) continue;

                            try
                            {
                                try { value = objs[rFields[field.Name]]; }
                                catch { value = null; }
                                value = value == DBNull.Value || value == null
                                        ? field.PropertyType == typeof(string) ? Activator.CreateInstance(field.PropertyType, string.Empty.ToArray())
                                        : field.PropertyType == typeof(byte[]) ? new byte[0]
                                        : field.PropertyType == typeof(char[]) ? new char[0]
                                        : Activator.CreateInstance(field.PropertyType)
                                        : value;
                                if (field.PropertyType.IsEnum) field.SetValue(res, Enum.ToObject(field.PropertyType, value), null);
                                else if (field.PropertyType == typeof(Guid))
                                {
                                    Guid gValue;
                                    if (value.GetType() == typeof(byte[]))
                                        gValue = new Guid((byte[])value);
                                    else
                                        gValue = new Guid(value.ToString());
                                    field.SetValue(res, gValue, null);
                                }
                                else if (field.PropertyType == typeof(byte[]))
                                {
                                    field.SetValue(res, value, null);
                                }
                                else if (field.PropertyType == typeof(char[]))
                                {
                                    field.SetValue(res, value, null);
                                }
                                else
                                {
                                    field.SetValue(res, Convert.ChangeType(value.ToString(), field.PropertyType), null);
                                }
                            }
                            catch
                            {

                            }
                        }
                        result.Add(res);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
