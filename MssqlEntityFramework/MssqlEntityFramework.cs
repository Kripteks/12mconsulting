using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

public class MssqlEntityFramework : IDisposable
{
    private bool disposed = false;

    private SqlConnection sqlConnection = null;
    private SqlTransaction SqlTrans = null;
    private SqlCommand sqlCommand = null;

    public List<SqlParameter> sqlParameters = new List<SqlParameter>() { };

    private string ServerAddress = string.Empty;
    private string DatabaseName = string.Empty;
    private string UserName = string.Empty;
    private string Password = string.Empty;

    public void AddParameter(string ParamName, SqlDbType ParamType, object ParamValue)
    {
        SqlParameter localParam = new SqlParameter(ParamName, ParamType)
        {
            Value = ParamValue
        };
        sqlParameters.Add(localParam);
    }
    public bool CheckConnection()
    {
        try
        {
            SqlConnection _tempConnection = sqlConnection;
            sqlConnection = new SqlConnection(string.Format("SERVER={0}; Database={1}; User Id={2}; Password={3}; Connect Timeout=5", ServerAddress, DatabaseName, UserName, Password));
            sqlConnection.Open();
            sqlConnection.Close();
            sqlConnection = _tempConnection;
            return true;
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return false;
        }
    }
    public void ClearParameters()
    {
        sqlParameters = new List<SqlParameter>() { };
        if (sqlCommand != null)
        {
            if (sqlCommand.Parameters.Count > 0)
            {
                sqlCommand.Parameters.Clear();
            }
        }
    }

    public SqlConnection GetConnection()
    {
        sqlConnection = new SqlConnection(string.Format("SERVER={0}; Database={1}; User Id={2}; Password={3}; Connect Timeout=30", ServerAddress, DatabaseName, UserName, Password));

        return sqlConnection;
    }
    public void CloseConnection()
    {
        try
        {
            if (SqlTrans != null)
            {
                SqlTrans.Commit();
            }
            if (sqlConnection != null)
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }
        catch (Exception ex)
        {
            ex.WriteException();
        }
    }
    public SqlCommand GetCommand()
    {
        if (sqlCommand == null)
        {
            sqlCommand = new SqlCommand();
        }
        if (sqlCommand.Connection == null)
        {
            sqlCommand.Connection = sqlConnection != null ? sqlConnection : GetConnection();
        }
        if (sqlCommand.Connection.State != ConnectionState.Open)
        {
            sqlCommand.Connection.Open();
        }

        return sqlCommand;
    }

    public SqlCommand GetCommand(string SqlQuery)
    {
        if (sqlCommand == null)
        {
            sqlCommand = new SqlCommand();
        }
        if (sqlCommand.Connection == null)
        {
            sqlCommand.Connection = sqlConnection != null ? sqlConnection : GetConnection();
        }

        sqlCommand.CommandText = SqlQuery;
        if (sqlCommand.Connection.State != ConnectionState.Open)
        {
            sqlCommand.Connection.Open();
        }

        return sqlCommand;
    }

    public SqlCommand GetCommand(string SqlQuery, params SqlParameter[] sqlParam)
    {
        if (sqlCommand == null)
        {
            sqlCommand = new SqlCommand();
        }
        if (sqlCommand.Connection == null)
        {
            sqlCommand.Connection = sqlConnection != null ? sqlConnection : GetConnection();
        }
        sqlCommand.CommandText = SqlQuery;
        if (sqlCommand.Connection.State != ConnectionState.Open)
        {
            sqlCommand.Connection.Open();
        }

        if (sqlParam != null && sqlParam.Length != 0)
        {
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddRange(sqlParam);
        }

        return sqlCommand;
    }

    public SqlCommand GetCommandWithTransaction(string SqlQuery)
    {
        if (sqlCommand == null)
        {
            sqlCommand = new SqlCommand();
        }
        if (sqlCommand.Connection == null)
        {
            sqlCommand.Connection = sqlConnection != null ? sqlConnection : GetConnection();
        }
        sqlCommand.CommandText = SqlQuery;
        if (sqlCommand.Connection.State != ConnectionState.Open)
        {
            sqlCommand.Connection.Open();
        }
        SqlTrans = sqlCommand.Connection.BeginTransaction();
        sqlCommand.Transaction = SqlTrans;
        return sqlCommand;
    }
    public void ChangeDatabaseContext(string context)
    {
        if (sqlConnection == null)
        {
            sqlConnection = GetConnection();
            sqlConnection.Open();
        }
        sqlConnection.ChangeDatabase(context);
    }
    public int GetCommandExecuteNoneQuery(string SqlQuery, params SqlParameter[] SqlParam)
    {
        if (sqlCommand == null)
        {
            sqlCommand = new SqlCommand();
        }
        if (sqlCommand.Connection == null)
        {
            sqlCommand.Connection = sqlConnection != null ? sqlConnection : GetConnection();
        }
        sqlCommand.CommandText = SqlQuery;
        if (sqlCommand.Connection.State != ConnectionState.Open)
        {
            sqlCommand.Connection.Open();
        }

        if (SqlParam != null && SqlParam.Length != 0)
        {
            sqlCommand.Parameters.AddRange(SqlParam);
        }

        int etkilenen = 0;
        try
        {
            etkilenen = sqlCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            ex.WriteException();
            etkilenen = 0;
        }
        finally
        {
            //if (sqlConnection.State == ConnectionState.Open)
            //{
            //    sqlConnection.Close();
            //}
        }
        return etkilenen;
    }

    public int GetCommandExecuteNoneQueryWithTransaction(string SqlQuery, bool closeConnection, params SqlParameter[] SqlParam)
    {
        if (sqlCommand == null)
        {
            sqlCommand = new SqlCommand();
        }
        if (sqlCommand.Connection == null)
        {
            sqlCommand.Connection = sqlConnection != null ? sqlConnection : GetConnection();
        }
        sqlCommand.CommandText = SqlQuery;
        if (sqlCommand.Connection.State != ConnectionState.Open)
        {
            sqlCommand.Connection.Open();
            SqlTrans = sqlCommand.Connection.BeginTransaction();
            sqlCommand.Transaction = SqlTrans;
        }
        if (SqlTrans == null)
        {
            SqlTrans = sqlCommand.Connection.BeginTransaction();
            sqlCommand.Transaction = SqlTrans;
        }

        if (SqlParam != null && SqlParam.Length != 0)
        {
            sqlCommand.Parameters.AddRange(SqlParam);
        }

        int etkilenen = 0;

        try
        {
            etkilenen = sqlCommand.ExecuteNonQuery();
            if (etkilenen == 0)
            {
                SqlTrans.Rollback();
            }
            else if (closeConnection)
            {
                SqlTrans.Commit();
            }
        }
        catch (Exception ex)
        {
            ex.WriteException();
            etkilenen = 0;
            if (SqlTrans != null)
            {
                SqlTrans.Rollback();
            }
        }
        finally
        {
            if (closeConnection)
            {
                if (sqlConnection.State == ConnectionState.Open)
                {
                    sqlConnection.Close();
                }
            }
        }


        return etkilenen;
    }

    /*
    public SqlDataReader GetDataReader(SqlCommand cmd)
    {
        using (SqlDataReader dr = cmd.ExecuteReader())
        {
            return dr;
        }
    }*/

    public DataTable GetDataTable(SqlCommand cmd)
    {
        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                return dt;
            }
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return new DataTable();
        }
    }

    public List<T> GetList<T>(SqlCommand cmd)
    {
        try
        {
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                return dr.CreateList<T>();
            }
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return new List<T>();
        }
    }

    public object GetCommandExecuteScalar(SqlCommand cmd)
    {
        try
        {
            return cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return null;
        }
    }

    public string GetCommandExecuteScalarString(SqlCommand cmd)
    {
        try
        {
            return cmd.ExecuteScalar() == null ? "" : cmd.ExecuteScalar().ToString();
        }
        catch (System.NullReferenceException)
        {
            return "";
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return "";
        }
    }

    public int GetCommandExecuteScalarInt(SqlCommand cmd)
    {
        try
        {
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return 0;
        }
    }

    public float GetCommandExecuteScalarFloat(SqlCommand cmd)
    {
        try
        {
            return Convert.ToSingle(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return 0;
        }
    }

    public DateTime GetCommandExecuteDate(SqlCommand cmd)
    {
        try
        {
            return Convert.ToDateTime(cmd.ExecuteScalar());
        }
        catch (Exception ex)
        {
            ex.WriteException();
            return DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Destructor
    /// </summary>
    ~MssqlEntityFramework()
    {
        this.Dispose(false);
    }

    public MssqlEntityFramework(string serverAddress, string databaseName, string userName, string password)
    {
        ServerAddress = serverAddress;
        DatabaseName = databaseName;
        UserName = userName;
        Password = password;
    }
    /// <summary>
    /// The dispose method that implements IDisposable.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// The virtual dispose method that allows
    /// classes inherithed from this one to dispose their resources.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                CloseConnection();
                // Dispose managed resources here.
            }

            // Dispose unmanaged resources here.
        }

        disposed = true;
    }


    #region Custom Creation

    /// <summary>
    /// Tabloyu temizlemek için SQL Sorgusu oluşturur.
    /// </summary>
    /// <typeparam name="T">Class tipi</typeparam>
    /// <returns></returns>
    public string SetTruncateTable<T>()
    {
        Type clsTip = typeof(T);
        string tableName = clsTip.CustomAttributes.Count() == 0 ? clsTip.Name : clsTip.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[0].Value.ToString();
        return string.Format($"TRUNCATE TABLE {tableName}");
    }

    public string ToSelectString<T>(params string[] disabledColumn)
    {
        Type deneme = typeof(T);

        string keyName = string.Empty;
        string objectName = string.Empty;

        System.Text.StringBuilder query = new System.Text.StringBuilder();

        //query.Append("USE [" + DbName + "] ");
        query.Append("SELECT ");

        //int sayi = deneme.GetProperties().Length;
        //if (disabledColumn != null)
        //{
        //    sayi = sayi - disabledColumn.Length;
        //}
        //int sira = 0;
        foreach (System.Reflection.PropertyInfo pi in deneme.GetProperties())
        {
            //sira++;
            if ((disabledColumn != null && disabledColumn.Contains(pi.Name)))
            {

            }
            else if (pi.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {

            }
            else
            {
                query.Append(pi.Name);
                query.Append(", ");
            }
        }
        query.Remove(query.Length - 2, 2);
        query.Append(" FROM ");

        if (deneme.CustomAttributes.Count() > 0)
        {
            objectName = deneme.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[0].Value.ToString();
            query.Append(objectName);
        }
        else
        {
            query.Append(deneme.Name);
        }

        return query.ToString();
    }

    public string ToCreateTable<T>(params string[] disabledColumn)
    {
        Type deneme = typeof(T);

        string keyName = string.Empty;
        string objectName = string.Empty;

        System.Text.StringBuilder query = new System.Text.StringBuilder();

        query.Append("IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='");

        if (deneme.CustomAttributes.Any(x => x.AttributeType == typeof(DisplayNameAttribute)))
        {
            objectName = deneme.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
            query.Append(objectName);
        }
        else
        {
            objectName = deneme.Name;
            query.Append(deneme.Name);
        }

        query.Append("' and xtype='U') BEGIN CREATE TABLE ");
        query.Append(objectName);
        query.Append(" ( ");
        foreach (PropertyInfo pi in deneme.GetProperties())
        {
            //sira++;
            if (disabledColumn != null && disabledColumn.Contains(pi.Name))
            {

            }
            else if (pi.CustomAttributes.Any(x => x.AttributeType == typeof(NotMappedAttribute)))
            {

            }
            else
            {
                SqlDbType dbType = GetDbType(pi.PropertyType);
                string typeString = "";

                switch (dbType)
                {
                    case SqlDbType.BigInt:
                        typeString = "bigint";
                        break;
                    case SqlDbType.Binary:
                        typeString = "binary(@length)";
                        break;
                    case SqlDbType.Bit:
                        typeString = "bit";
                        break;
                    case SqlDbType.Char:
                        typeString = "char(@length)";
                        break;
                    case SqlDbType.DateTime:
                        typeString = "datetime";
                        break;
                    case SqlDbType.Decimal:
                        typeString = "decimal(18, 5)";
                        break;
                    case SqlDbType.Float:
                        typeString = "float";
                        break;
                    case SqlDbType.Image:
                        typeString = "image";
                        break;
                    case SqlDbType.Int:
                        typeString = "int";
                        break;
                    case SqlDbType.Money:
                        typeString = "money";
                        break;
                    case SqlDbType.NChar:
                        typeString = "nchar(@length)";
                        break;
                    case SqlDbType.NText:
                        typeString = "ntext";
                        break;
                    case SqlDbType.NVarChar:
                        typeString = "nvarchar(@length)";
                        break;
                    case SqlDbType.Real:
                        typeString = "real";
                        break;
                    case SqlDbType.UniqueIdentifier:
                        typeString = "uniqueIdentifier";
                        break;
                    case SqlDbType.SmallDateTime:
                        typeString = "smalldatetime";
                        break;
                    case SqlDbType.SmallInt:
                        typeString = "smallint";
                        break;
                    case SqlDbType.SmallMoney:
                        typeString = "smallmoney";
                        break;
                    case SqlDbType.Text:
                        typeString = "text";
                        break;
                    case SqlDbType.Timestamp:
                        typeString = "timestamp";
                        break;
                    case SqlDbType.TinyInt:
                        typeString = "tinyint";
                        break;
                    case SqlDbType.VarBinary:
                        typeString = "varbinary(MAX)";
                        break;
                    case SqlDbType.VarChar:
                        typeString = "varchar(@length)";
                        break;
                    case SqlDbType.Variant:
                        typeString = "sql_variant";
                        break;
                    case SqlDbType.Xml:
                        typeString = "xml";
                        break;
                    case SqlDbType.Udt:
                        typeString = "udt";
                        break;
                    case SqlDbType.Structured:
                        typeString = "structured";
                        break;
                    case SqlDbType.Date:
                        typeString = "date";
                        break;
                    case SqlDbType.Time:
                        typeString = "time(7)";
                        break;
                    case SqlDbType.DateTime2:
                        typeString = "datetime2(7)";
                        break;
                    case SqlDbType.DateTimeOffset:
                        typeString = "datetimeoffset(7)";
                        break;
                    default:
                        break;
                }

                if (pi.CustomAttributes.Any(x => x.AttributeType == typeof(MaxLengthAttribute)))
                {
                    typeString = typeString.Replace("@length", pi.GetCustomAttribute<MaxLengthAttribute>().Length.ToString());
                }
                else
                {
                    if (dbType == SqlDbType.NVarChar)
                    {
                        typeString = typeString.Replace("@length", "max");
                    }
                    else
                    {
                        typeString = typeString.Replace("@length", "50");
                    }
                }

                query.Append(string.Format("{0} {1} {2} {3}, ", pi.Name, typeString, (pi.CustomAttributes.Any(x => x.AttributeType == typeof(RequiredAttribute)) ? "NOT NULL" : "NULL"), (pi.CustomAttributes.Any(x => x.AttributeType == typeof(DescriptionAttribute)) ? " DEFAULT " + pi.GetCustomAttribute<DescriptionAttribute>().Description : "")));
            }
        }
        query.Remove(query.Length - 2, 2);

        query.Append(" ) END");



        return query.ToString();
    }


    public string ToInsertString<T>(T element, out List<SqlParameter> list, params string[] disabledColumn)
    {
        Type deneme = typeof(T);
        list = new List<SqlParameter>() { };

        string keyName = string.Empty;
        string objectName = string.Empty;

        System.Text.StringBuilder query = new System.Text.StringBuilder();
        System.Text.StringBuilder parameters = new System.Text.StringBuilder();

        try
        {
            //query.Append("USE [" + DbName + "] ");
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
            // sayi = sayi - yasakliSayi;
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
                        if (ConversionEntity.Parse<DateTime>(item.GetValue(element)) == DateTime.MinValue || item.GetValue(element) == null)
                        {
                            deger = new DateTime(1899, 12, 30, 0, 0, 0);
                        }
                        else
                        {
                            deger = item.GetValue(element);
                        }
                    }
                    else if (dbType == SqlDbType.NVarChar)
                    {
                        if (item.CustomAttributes.Any(x => x.AttributeType == typeof(MaxLengthAttribute)))
                        {
                            MaxLengthAttribute sinir = item.GetCustomAttribute<MaxLengthAttribute>();

                            deger = item.GetValue(element) == null ? DBNull.Value : item.GetValue(element);

                            string ConversionedValue = ConversionEntity.Parse<string>(deger, "");

                            if (ConversionedValue.Length > sinir.Length)
                            {
                                ConversionedValue = ConversionedValue.Substring(0, sinir.Length);
                                deger = ConversionedValue;
                            }

                        }
                        else
                        {
                            deger = item.GetValue(element) == null ? DBNull.Value : item.GetValue(element);
                        }
                    }
                    else
                    {
                        deger = item.GetValue(element) == null ? DBNull.Value : item.GetValue(element);
                    }

                    list.Add(new SqlParameter() { ParameterName = "@" + item.Name, SqlDbType = dbType, Value = deger });
                }

            }
        }
        catch (Exception ex)
        {
            ex.WriteException();
        }
        return query.ToString();
    }

    public string ToUpdateString<T>(T element, string[] FilterColumn, out List<SqlParameter> list, params string[] disabledColumn)
    {
        Type deneme = typeof(T);
        list = new List<SqlParameter>() { };

        string keyName = string.Empty;
        string objectName = string.Empty;

        System.Text.StringBuilder query = new System.Text.StringBuilder();
        System.Text.StringBuilder where = new System.Text.StringBuilder();
        //query.Append("USE [" + DbName + "] ");
        query.Append("UPDATE ");
        if (deneme.CustomAttributes.Count() > 0)
        {
            objectName = deneme.CustomAttributes.ToArray()[0].ConstructorArguments.ToArray()[0].Value.ToString();
            query.Append(objectName + " ");
        }
        else
        {
            query.Append(deneme.Name);
        }
        query.Append(" SET ");

        foreach (System.Reflection.PropertyInfo pi in deneme.GetProperties())
        {
            //parameters.Append("@" + pi.Name);
            if (FilterColumn.Any(x => x == pi.Name))
            {
                where.Append(pi.Name + " = @" + pi.Name + " AND ");
            }
            else if (disabledColumn != null && disabledColumn.Contains(pi.Name))
            {

            }
            else if (pi.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {

            }
            else
            {
                query.Append(pi.Name + " = @" + pi.Name);
                query.Append(", ");
            }

        }
        query.Remove(query.Length - 2, 2);

        string whereClause = where.ToString();

        if (whereClause.Length > 5)
        {
            query.Append(" WHERE ");

            whereClause = where.ToString().Substring(0, where.Length - 5);
        }

        query.Append(whereClause);

        deneme = element.GetType();

        foreach (System.Reflection.PropertyInfo item in deneme.GetProperties())
        {
            if (disabledColumn != null && disabledColumn.Contains(item.Name))
            {

            }
            else if (item.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute"))
            {

            }
            else
            {
                SqlDbType dbType = GetDbType(item.PropertyType);
                object deger = DBNull.Value;

                if (dbType == SqlDbType.DateTime || dbType == SqlDbType.Date || dbType == SqlDbType.DateTime2)
                {
                    if (ConversionEntity.Parse<DateTime>(item.GetValue(element)) == DateTime.MinValue || item.GetValue(element) == null)
                    {
                        deger = new DateTime(1899, 12, 30, 0, 0, 0);
                    }
                    else
                    {
                        deger = item.GetValue(element);
                    }
                }
                else if (dbType == SqlDbType.NVarChar)
                {
                    if (item.CustomAttributes.Any(x => x.AttributeType == typeof(MaxLengthAttribute)))
                    {
                        MaxLengthAttribute sinir = item.GetCustomAttribute<MaxLengthAttribute>();

                        deger = item.GetValue(element) == null ? DBNull.Value : item.GetValue(element);

                        string ConversionedValue = ConversionEntity.Parse<string>(deger, "");

                        if (ConversionedValue.Length > sinir.Length)
                        {
                            ConversionedValue = ConversionedValue.Substring(0, sinir.Length);
                            deger = ConversionedValue;
                        }

                    }
                    else
                    {
                        deger = item.GetValue(element) == null ? DBNull.Value : item.GetValue(element);
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

    public SqlDbType GetDbType(Type type)
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

    //public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : System.Attribute
    //{
    //    var attrType = typeof(T);
    //    var property = instance.GetType().GetProperty(propertyName);
    //    return (T)property.GetCustomAttributes(attrType, false).First();
    //}

    #endregion


}
