using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

public static class ConversionEntity
{
    public static NumberFormatInfo nfi = Thread.CurrentThread.CurrentCulture.NumberFormat;

    public static T Parse<T>(string sourceValue) where T : IConvertible
    {
        if (string.IsNullOrEmpty(sourceValue))
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T), nfi);
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(object sourceValue) where T : IConvertible
    {

        try
        {
            if (sourceValue == null || sourceValue == DBNull.Value)
                return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());

            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(object sourceValue, object defaultValue) where T : IConvertible
    {

        try
        {
            if ((sourceValue == null || sourceValue == DBNull.Value) && defaultValue == null)
                return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
            return (T)Convert.ChangeType(sourceValue, typeof(T), null);
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T), null);
        }
    }

    public static T Parse<T>(string sourceValue, string defaultValue) where T : IConvertible
    {
        try
        {
            if (defaultValue != null && sourceValue == null)
            {
                return (T)Convert.ChangeType(defaultValue, typeof(T), null);
            }
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(short sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(short sourceValue, short defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(int sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(int sourceValue, int defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(long sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(long sourceValue, long defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(DateTime sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(DateTime sourceValue, long defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(bool sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(bool sourceValue, bool defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(Decimal sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T), nfi);
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(Decimal sourceValue, Decimal defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T), nfi);
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(float sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(float sourceValue, float defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static T Parse<T>(byte sourceValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)ConversionEntity.DefaultSetting(typeof(T).ToString());
        }
    }

    public static T Parse<T>(byte sourceValue, byte defaultValue) where T : IConvertible
    {
        try
        {
            return (T)Convert.ChangeType(sourceValue, typeof(T));
        }
        catch
        {
            return (T)Convert.ChangeType(defaultValue, typeof(T));
        }
    }

    public static DateTime ChangeTime(this DateTime dateTime, int hours, int minutes, int seconds, int milliseconds)
    {
        return new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            hours,
            minutes,
            seconds,
            milliseconds,
            dateTime.Kind);
    }

    public static object DefaultSetting(string type)
    {
        switch (type)
        {
            case "System.String":
                return string.Empty;
            case "System.Int32":
                return 0;
            case "System.Int16":
                return (short)0;
            case "System.Int64":
                return 0L;
            case "System.DateTime":
                return DateTime.Now;
            case "System.Decimal":
                return new Decimal(0);
            case "System.Boolean":
                return false;
            case "System.Double":
                return 0.0;
            case "System.Single":
                return 0.0f;
            case "System.Byte":
                return (byte)0;
            case "System.Char":
                return char.MinValue;
            default:
                return null;
        }
    }

    public static List<T> CreateList<T>(this SqlDataReader reader)
    {
        var results = new List<T>();
        var properties = typeof(T).GetProperties();

        while (reader.Read())
        {
            var item = Activator.CreateInstance<T>();
            foreach (var property in typeof(T).GetProperties())
            {
                try
                {
                    if (!property.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                        {
                            Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                            property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);

                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.WriteException();

                    Type convertTo = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    try
                    {
                        switch ((Type.GetTypeCode(convertTo)))
                        {
                            case TypeCode.Boolean:
                                property.SetValue(item, ConversionEntity.Parse<bool>(reader[property.Name], false), null);
                                break;
                            case TypeCode.Byte:
                                property.SetValue(item, ConversionEntity.Parse<byte>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.Int16:
                                property.SetValue(item, ConversionEntity.Parse<int>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.UInt16:
                                property.SetValue(item, ConversionEntity.Parse<UInt16>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.Int32:
                                property.SetValue(item, ConversionEntity.Parse<int>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.UInt32:
                                property.SetValue(item, ConversionEntity.Parse<UInt32>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.Int64:
                                property.SetValue(item, ConversionEntity.Parse<Int64>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.UInt64:
                                property.SetValue(item, ConversionEntity.Parse<UInt64>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.Double:
                                property.SetValue(item, ConversionEntity.Parse<double>(reader[property.Name], 0), null);
                                break;
                            case TypeCode.Decimal:
                                property.SetValue(item, ConversionEntity.Parse<decimal>(reader[property.Name], new decimal(0)), null);
                                break;
                            case TypeCode.DateTime:
                                property.SetValue(item, ConversionEntity.Parse<DateTime>(reader[property.Name], new DateTime(1899, 12, 30, 0, 0, 0)), null);
                                break;
                            case TypeCode.String:
                                property.SetValue(item, ConversionEntity.Parse<string>(reader[property.Name], ""), null);
                                break;
                            default:
                                if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                                {
                                    property.SetValue(item, Convert.ChangeType(reader[property.Name], convertTo), null);
                                }
                                break;
                        }
                    }
                    catch (Exception ex2)
                    {
                        ex2.WriteException();
                    }
                }
            }
            results.Add(item);
        }
        reader.Close();

        return results;
    }

    public static T Fill<T>(this DataRow Row)
    {
        T results = (T)Activator.CreateInstance(typeof(T));

        Dictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();
        foreach (PropertyInfo p in results.GetType().GetProperties())
        {
            if (!p.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {
                props.Add(p.Name, p);
            }
        }

        foreach (DataColumn col in Row.Table.Columns)
        {
            string name = col.ColumnName;
            if (props.ContainsKey(name))
            {
                object item = Row[name];
                PropertyInfo p = props[name];

                try
                {
                    switch ((Type.GetTypeCode(p.PropertyType)))
                    {
                        case TypeCode.DateTime:
                            p.SetValue(results, ConversionEntity.Parse<DateTime>(item, new DateTime(1899, 12, 30, 0, 0, 0)), null);
                            break;
                        default:
                            if (Row[name] != DBNull.Value)
                            {
                                if (p.PropertyType != col.DataType)
                                {
                                    item = Convert.ChangeType(item, p.PropertyType);
                                }
                                p.SetValue(results, item, null);

                            }
                            break;
                    }
                }
                catch (Exception ex2)
                {
                    ex2.WriteException();
                }
            }
        }
        return results;
    }
    public static T Fill<T>(this DataRowView Row)
    {
        T results = (T)Activator.CreateInstance(typeof(T));

        Dictionary<string, PropertyInfo> props = new Dictionary<string, PropertyInfo>();
        foreach (PropertyInfo p in results.GetType().GetProperties())
        {
            if (!p.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {
                props.Add(p.Name, p);
            }
        }

        foreach (DataColumn col in Row.Row.Table.Columns)
        {
            string name = col.ColumnName;
            if (props.ContainsKey(name))
            {
                object item = Row[name];
                PropertyInfo p = props[name];

                try
                {
                    switch ((Type.GetTypeCode(p.PropertyType)))
                    {
                        case TypeCode.DateTime:
                            p.SetValue(results, ConversionEntity.Parse<DateTime>(item, new DateTime(1899, 12, 30, 0, 0, 0)), null);
                            break;
                        default:
                            if (Row[name] != DBNull.Value)
                            {
                                if (p.PropertyType != col.DataType)
                                {
                                    item = Convert.ChangeType(item, p.PropertyType);
                                }
                                p.SetValue(results, item, null);

                            }
                            break;
                    }
                }
                catch (Exception ex2)
                {
                    ex2.WriteException();
                }


            }
        }
        return results;
    }

    public static string CreateInsertString<T>(T element, string DbName, out List<SqlParameter> list, params string[] disabledColumn)
    {
        Type deneme = typeof(T);
        list = new List<SqlParameter>() { };

        string keyName = string.Empty;
        string objectName = string.Empty;

        System.Text.StringBuilder query = new System.Text.StringBuilder();
        System.Text.StringBuilder parameters = new System.Text.StringBuilder();
        query.Append("USE [" + DbName + "] ");
        query.Append("INSERT INTO ");
        if (deneme.CustomAttributes.Count() > 0)
        {
            objectName = deneme.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[0].Value.ToString();
            query.Append(objectName);
        }
        else
        {
            query.Append(deneme.Name);
        }
        query.Append("(");
        int sayi = deneme.GetProperties().Length;
        int yasakliSayi = ConversionEntity.Parse<int>(deneme.GetProperties().Where(x => x.CustomAttributes.Any(y => y.AttributeType.Name == "NotMappedAttribute")).Count(), 0);
        if (disabledColumn != null)
        {
            sayi = sayi - disabledColumn.Length;
        }

        foreach (System.Reflection.PropertyInfo pi in deneme.GetProperties())
        {
            if ((disabledColumn != null && disabledColumn.Contains(pi.Name)))
            {

            }
            else if (pi.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {

            }
            else
            {
                query.Append(pi.Name);
                parameters.Append("@" + pi.Name);

                query.Append(", ");
                parameters.Append(", ");
            }
        }
        query.Remove(query.Length - 2, 2);
        parameters.Remove(parameters.Length - 2, 2);

        query.Append(") ");

        query.Append("VALUES (");
        query.Append(parameters);
        query.Append(")");


        deneme = element.GetType();

        foreach (System.Reflection.PropertyInfo item in deneme.GetProperties())
        {
            if (disabledColumn != null && disabledColumn.Contains(item.Name))
            { }
            else if (item.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {

            }
            else
            {
                SqlDbType dbType = GetDbType(item.PropertyType);
                object deger = DBNull.Value;
                if (dbType == SqlDbType.DateTime || dbType == SqlDbType.Date || dbType == SqlDbType.DateTime2)
                {
                    if (ConversionEntity.Parse<DateTime>(item.GetValue(element)) == DateTime.MinValue)
                    {
                        deger = new DateTime(1899, 12, 30, 0, 0, 0);
                    }
                    else
                    {
                        deger = item.GetValue(element);
                    }
                }
                else
                {
                    deger = item.GetValue(element) == null ? DBNull.Value : item.GetValue(element);
                }

                list.Add(new SqlParameter() { ParameterName = "@" + item.Name, SqlDbType = dbType, Value = deger });
            }

        }

        return query.ToString();
    }
    public static string GetHash(string input)
    {
        HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

        byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

        byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

        return Convert.ToBase64String(byteHash);
    }

    public static SqlDbType GetDbType(Type type)
    {
        Dictionary<Type, SqlDbType> typeMap = new Dictionary<Type, SqlDbType>
        {
            [typeof(string)] = SqlDbType.NVarChar,
            [typeof(char[])] = SqlDbType.NVarChar,
            [typeof(byte)] = SqlDbType.TinyInt,
            [typeof(short)] = SqlDbType.SmallInt,
            [typeof(int)] = SqlDbType.Int,
            [typeof(long)] = SqlDbType.BigInt,
            [typeof(byte[])] = SqlDbType.Image,
            [typeof(bool)] = SqlDbType.Bit,
            [typeof(DateTime)] = SqlDbType.DateTime,
            [typeof(DateTimeOffset)] = SqlDbType.DateTimeOffset,
            [typeof(decimal)] = SqlDbType.Money,
            [typeof(float)] = SqlDbType.Real,
            [typeof(double)] = SqlDbType.Float,
            [typeof(TimeSpan)] = SqlDbType.Time,
            [typeof(Guid)] = SqlDbType.UniqueIdentifier
        };
        type = Nullable.GetUnderlyingType(type) ?? type;

        if (typeMap.ContainsKey(type))
        {
            return typeMap[type];
        }

        throw new ArgumentException($"{type.FullName} is not a supported .NET class");
    }


    public static void WriteException(this Exception val)
    {
        try
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "\\Exceptions"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\Exceptions");
            }
            string filePath = Environment.CurrentDirectory + "\\Exceptions\\" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".log";

            using (StreamWriter sw = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                sw.WriteLine(DateTime.Now.ToString("dd MMMM yyyy, dddd HH:mm:ss"));
                sw.WriteLine(val.Message);
                sw.WriteLine("----------------------");
                sw.WriteLine(val.Source);
                sw.WriteLine("----------------------");
                sw.WriteLine(val.StackTrace);
                sw.Close();
            }
        }
        catch (Exception ex)
        {
            ex.WriteException();
        }
    }

    public static T GetObject<T>() where T : new()
    {
        return new T();
    }
}
