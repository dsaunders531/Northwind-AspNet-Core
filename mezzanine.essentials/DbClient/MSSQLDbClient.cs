using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Xml;

namespace mezzanine.DbClient
{
    /// <summary>
    /// A Db client for MS SQL database provider.
    /// </summary>
    public class MSSQLDbClient : DbClient
    {
        public MSSQLDbClient(string connectionString) : base(connectionString)
        {
        }

        public MSSQLDbClient(DbConnection connection) : base(connection)
        {
        }

        private SqlConnection CreateConnection
        {
           get
           {
                return new SqlConnection(base.ConnectionString);
           }
        }

        /// <summary>
        /// When a SqlCommand has been used once. It can't be used again.
        /// Create a new command based on the existing one.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public SqlCommand CloneCommand(SqlCommand command)
        {
            return command.Clone();
        }

        /// <summary>
        /// Emit T from a query. If you want a list of objects use a generic list as the type.        
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public T Fill<T>(SqlCommand command)
        {
            T result = default(T);

            if (this.IsValidStatement(command.CommandText) == true)
            {
                DataTable table = this.GetDataTable(command, "result");

                result = base.DataTableToT<T>(table);

                table = null;
            }
            else
            {
                throw new ArgumentException("The command contained invalid text");
            }

            return result;        
        }

        public override T Fill<T>(string sqlText)
        {
            using (SqlCommand com = new SqlCommand(sqlText) { CommandType = CommandType.Text })
            {
                return this.Fill<T>(com);
            }
        }

        public override T Fill<T>(DbCommand command)
        {
            return this.Fill<T>((SqlCommand)command);
        }

        public override DataTable GetDataTable(string sqlText, string tableName)
        {
            DataTable result = null;

            using (SqlCommand com = new SqlCommand(sqlText) { CommandType = CommandType.Text })
            {
                result = this.GetDataTable(com, tableName);
            }
            
            return result;
        }

        public DataTable GetDataTable(SqlCommand command, string tableName)
        {
            //DataSet dataSet = new DataSet(tableName);
            DataTable result = new DataTable(tableName); //dataSet.Tables.Add(tableName);
            string transactionName = this.GenerateTransactionName();

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command)
            {
                FillLoadOption = LoadOption.OverwriteChanges,
                AcceptChangesDuringFill = false,
                AcceptChangesDuringUpdate = false,
                MissingMappingAction = MissingMappingAction.Passthrough,
                MissingSchemaAction = MissingSchemaAction.Add,
                ReturnProviderSpecificTypes = false
            })
            {
                // Note you can use text right to make the update, insert, delete statements in .Net proper (its not in Core) System.Data.SqlClient.SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);

                if (this.IsValidStatement(command.CommandText, false)
                    && (command.CommandType == CommandType.Text || command.CommandType == CommandType.TableDirect)
                    && this.IsValidSqlParameters(command.Parameters) == true)
                {
                    using (SqlConnection con = this.CreateConnection)
                    {
                        try
                        {
                            command.Connection = con;
                            con.Open();

                            using (SqlTransaction tran = con.BeginTransaction(System.Data.IsolationLevel.Snapshot, transactionName))
                            {
                                try
                                {
                                    command.Transaction = tran;
                                    //dataAdapter.Fill(dataSet, tableName);
                                    dataAdapter.Fill(result);
                                    tran.Commit();
                                }
                                catch (Exception e)
                                {
                                    tran.Rollback();
                                    throw new ApplicationException("Transaction failed. Commands have been rolled back. See inner exception for more details.", e);
                                }
                            }

                            con.Close();

                            //result = dataSet.Tables[tableName];
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("Transaction failed. See inner exception for more details.", ex);
                        }
                        finally
                        {
                            if (con.State != System.Data.ConnectionState.Closed)
                            {
                                con.Close();
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("The command contains and invalid statement, paramters or is the wrong type.");
                }
            }

            return result;
        }

        public override DataTable GetDataTable(DbCommand command, string tableName)
        {
            return this.GetDataTable((SqlCommand)command, tableName);
        }

        /// <summary>
        /// Return the first field from the first row of a query.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sqlText"></param>
        /// <param name="noLock"></param>
        /// <returns></returns>
        public override T GetOneField<T>(string sqlText, bool noLock = true)
        {
            T result = default(T);
            using (SqlCommand com = new SqlCommand(sqlText) { CommandType = CommandType.Text })
            {
                result = this.GetOneField<T>(com, noLock);
            }
            return result;
        }

        public T GetOneField<T>(SqlCommand command, bool noLock = true)
        {
            T result;
            object value = this.GetOneField(command, noLock);

            if (typeof(T).IsValueType == true && value == null)
            {
                // These things cannot be null so return the default value.
                result = default(T);
            }
            else
            {
                result = (T)Convert.ChangeType(value, typeof(T));
            }
            
            return result;
        }

        public override T GetOneField<T>(DbCommand command, bool noLock)
        {
            return this.GetOneField<T>((SqlCommand)command, noLock);
        }

        private object GetOneField(string sqlText, bool noLock = true)
        {
            object result = null;

            using (SqlCommand com = new SqlCommand(sqlText) { CommandType = CommandType.Text })
            {
                result = this.GetOneField(com, noLock);
            }

            return result;
        }

       /// <summary>
       /// Return 1 value (first row, first column).
       /// </summary>
       /// <param name="command">The command you want to run.</param>
       /// <param name="noLock">Lock the database when doing the read.</param>
       /// <returns>An object representing the result.</returns>
        private object GetOneField(SqlCommand command, bool noLock = true)
        {
            object result = null;
            string transactionName = this.GenerateTransactionName();

            System.Data.IsolationLevel dataIsolocationLevel = System.Data.IsolationLevel.Snapshot;

            if (noLock == false)
            {
                dataIsolocationLevel = System.Data.IsolationLevel.Serializable;
            }

            if (this.IsValidStatement(command.CommandText) && command.CommandType == CommandType.Text)
            {
                if (this.IsValidSqlParameters(command.Parameters) == true)
                {
                    using (SqlConnection con = this.CreateConnection)
                    {
                        try
                        {
                            command.Connection = con;
                            con.Open();

                            using (SqlTransaction tran = con.BeginTransaction(dataIsolocationLevel, transactionName))
                            {
                                try
                                {
                                    command.Transaction = tran;
                                    result = command.ExecuteScalar();
                                    tran.Commit();
                                }
                                catch (Exception e)
                                {
                                    tran.Rollback();
                                    throw new ApplicationException("Transaction failed. Commands have been rolled back. See inner exception for more details.", e);
                                }
                            }

                            con.Close();
                        }
                        catch (Exception ex)
                        {                           
                            throw new ApplicationException("Transaction failed. See inner exception for more details.", ex);
                        }
                        finally
                        {
                            if (con.State != System.Data.ConnectionState.Closed)
                            {
                                con.Close();
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("The parameters contained invalid text.");
                }                
            }
            else
            {
                throw new ArgumentException("The sqlText contained an invalid command.");
            }

            command.Dispose();
            command = null;

            return result;
        }
        
        /// <summary>
        /// Run a database update, insert or delete command.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public int RunUpsert(SqlCommand command)
        {
            int result = default(int);
            string transactionName = this.GenerateTransactionName();

            if (this.IsValidStatement(command.CommandText, true))
            {
                if (this.IsValidSqlParameters(command.Parameters) == true)
                {
                    using (SqlConnection con = this.CreateConnection)
                    {
                        try
                        {
                            command.Connection = con;
                            con.Open();

                            using (SqlTransaction tran = con.BeginTransaction(System.Data.IsolationLevel.Serializable, transactionName))
                            {
                                try
                                {
                                    command.Transaction = tran;
                                    result = command.ExecuteNonQuery();
                                    tran.Commit();
                                }
                                catch (Exception e)
                                {
                                    tran.Rollback();
                                    throw new ApplicationException("Transaction failed. Commands have been rolled back. See inner exception for more details.", e);
                                }

                            }

                            con.Close();
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("Transaction failed. See inner exception for more details.", ex);
                        }
                        finally
                        {
                            if (con.State != System.Data.ConnectionState.Closed)
                            {
                                con.Close();
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("The sqlText contained an invalid string paramter.");
                }                
            }
            else
            {
                throw new ArgumentException("The sqlText contained an invalid command.");
            }

            command.Dispose();
            command = null;

            return result;
        }

        public override int RunUpsert(DbCommand command)
        {
            return this.RunUpsert((SqlCommand)command);
        }

        public override int RunUpsert(string sqlText)
        {
            int result = default(int);

            using (SqlCommand com = new SqlCommand(sqlText) { CommandType = CommandType.Text })
            {
                result = this.RunUpsert(com);
            }

            return result;
        }

        /// <summary>
        /// Run a stored proc.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <remarks>Returns the rows affected.</remarks>
        public int RunStoredProc(SqlCommand command)
        {
            int result = default(int);

            if (command.CommandText.ToUpper().StartsWith("EXEC"))
            {
                // Running a stored proc using exec
                if (this.IsValidStatement(command.CommandText) == false)
                {
                    throw new ArgumentException("The command contains invalid text.");
                }
            }
            else
            {
                // Run standard checks.
                if (command.CommandType != CommandType.StoredProcedure)
                {
                    throw new ArgumentException("The command is not a stored procedure type.");
                }

                // the command text should be 1 word for a stored proc.
                if (this.IsValidStatement(command.CommandText) == false)
                {
                    throw new ArgumentException("The command is not a stored procedure.");
                }

                if (command.CommandText.Split(Convert.ToChar(" ")).GetUpperBound(0) > 0)
                {
                    // the command is more than 1 word - which is not a stored proc.
                    throw new ArgumentException("The command is not a stored procedure.");
                }
            }
          
            if (this.IsValidSqlParameters(command.Parameters) == false)
            {
                // the command is more than 1 word - which is not a stored proc.
                throw new ArgumentException("The command has a parameter with invalid values.");
            }

            result = this.RunUpsert(command);

            return result;
        }

        public override int RunStoredProc(DbCommand command)
        {
            return this.RunStoredProc((SqlCommand)command);
        }

        public override int RunTransaction(List<string> sqlTexts, string transactionName)
        {
            List<SqlCommand> commands = new List<SqlCommand>();

            foreach (string item in sqlTexts)
            {
                commands.Add(new SqlCommand(item));
            }

            return this.RunTransaction(commands, transactionName);
        }

        /// <summary>
        /// Run a series of commands together in a transaction. The queries should not produce as these cannot be returned.
        /// </summary>
        /// <param name="commands">A list of commands to run together.</param>
        /// <returns>total quantity of rows changed by each command.</returns>
        /// <remarks>If any of the commands fails the previous commands will be rolled back.</remarks>
        public int RunTransaction(List<SqlCommand> commands, string transactionName)
        {
            int retVal = 0;            

            if (transactionName == string.Empty)
            {
                transactionName = this.GenerateTransactionName();
            }

            try
            {
                using (SqlConnection con = this.CreateConnection)
                {
                    con.Open();

                    using (SqlTransaction tran = con.BeginTransaction(System.Data.IsolationLevel.Serializable, transactionName))
                    {
                        try
                        {
                            foreach (SqlCommand command in commands)
                            {
                                if (command != null)
                                {
                                    if (this.IsValidStatement(command.CommandText, true) == true)
                                    {
                                        if (this.IsValidSqlParameters(command.Parameters) == true)
                                        {
                                            command.Connection = con;
                                            command.Transaction = tran;

                                            retVal += command.ExecuteNonQuery();
                                        }
                                        else
                                        {
                                            throw new ArgumentException("The command contains a parameter with text which is not allowed. " + command.CommandText);
                                        }
                                    }
                                    else
                                    {
                                        throw new ArgumentException("The command contains text which is not allowed. " + command.CommandText);
                                    }
                                }
                            }
                            tran.Commit();
                        }
                        catch (Exception e)
                        {
                            tran.Rollback();
                            throw new ApplicationException("Transaction failed. Commands have been rolled back. See inner exception for more details.", e);
                        }
                    }

                    if (con.State != System.Data.ConnectionState.Closed)
                    {
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Transaction failed. See inner exception for more details.", ex);
            }
            finally
            {
                commands.Clear();
                commands = null;
            }

            return retVal;
        }

        public override int RunTransaction(List<DbCommand> commands, string transactionName)
        {
            List<SqlCommand> sqlCommands = new List<SqlCommand>();

            foreach (DbCommand command in commands)
            {
                sqlCommands.Add((SqlCommand)command);
            }

            return this.RunTransaction(sqlCommands, transactionName);
        }

        private string GenerateTransactionName()
        {
            string result = this.CreateConnection.WorkstationId + DateTime.Now.Ticks.ToString();
            const int maxLen = 32;

            if (result.Length > maxLen)
            {
                // Keep the leftmost characters as these will be unique.
                result = result.Substring(result.Length - maxLen, maxLen);
            }

            return result;
        }

        /// <summary>
        /// Check the string parameters for bad values.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private bool IsValidSqlParameters(SqlParameterCollection parameters)
        {
            bool result = true;

            if (parameters != null)
            {
                if (parameters.Count > 0)
                {
                    foreach (SqlParameter item in parameters)
                    {
                        if (item.SqlDbType == SqlDbType.NText || item.SqlDbType == SqlDbType.NVarChar 
                            || item.SqlDbType == SqlDbType.Text || item.SqlDbType == SqlDbType.VarChar)
                        {
                            result = this.IsValidSqlParameter((string)item.Value);
                            if (result == false)
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Validate parameter text for bad values.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool IsValidSqlParameter(string value)
        {
            bool result = true; 

            if (value.Contains(@"'"))
            {
                // there is a single apostrophe, double apos is allowed.
                result = value.Contains(@"''");
            }            

            if (result == true)
            {
                result = this.IsValidStatement(value);
            }

            return result; // NB the reversal of result value
        }

        /// <summary>
        /// Check to see if a statement is valid.
        /// </summary>
        /// <param name="sqlText">The sql to examine.</param>
        /// <param name="allowUpdates">Allow the update keywords.</param>
        /// <returns></returns>
        private bool IsValidStatement(string sqlText, bool allowUpdates = false)
        {
            // ALTER, CREATE, DROP, GRANT, ; (DELETE, INSERT, UPDATE)
            sqlText = sqlText.Trim().Replace(" ", string.Empty).ToUpper();

            bool result = true;

            // cannot have a ; followed by ALTER, CREATE etc.
            result = ! (sqlText.Contains(";ALTER") || sqlText.StartsWith("ALTER")
                     || sqlText.Contains(";CREATE") || sqlText.StartsWith("CREATE")
                     || sqlText.Contains(";DROP") || sqlText.StartsWith("DROP")
                     || sqlText.Contains(";GRANT") || sqlText.StartsWith("GRANT")
                     || sqlText.Contains(";SELECT") || sqlText.Contains(";EXEC"));

            if (result == true && allowUpdates == false)
            {
                result = ! (sqlText.Contains(";DELETE") || sqlText.StartsWith("DELETE") 
                            || sqlText.Contains(";INSERT") || sqlText.StartsWith("INSERT") 
                            || sqlText.Contains(";UPDATE") || sqlText.StartsWith("UPDATE"));
            }

            return result;
        }
    }
}

/* SQL table mapping see https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
 * 
 * SQL Server Database Engine type 	.NET Framework type 	SqlDbType enumeration 	SqlDataReader SqlTypes typed accessor 	DbType enumeration 	SqlDataReader DbType typed accessor
bigint 	Int64 	BigInt 	GetSqlInt64 	Int64 	GetInt64
binary 	Byte[] 	VarBinary 	GetSqlBinary 	Binary 	GetBytes
bit 	Boolean 	Bit 	GetSqlBoolean 	Boolean 	GetBoolean
char 	String

Char[] 	Char 	GetSqlString 	AnsiStringFixedLength,

String 	GetString

GetChars
date 1

(SQL Server 2008 and later) 	DateTime 	Date 1 	GetSqlDateTime 	Date 1 	GetDateTime
datetime 	DateTime 	DateTime 	GetSqlDateTime 	DateTime 	GetDateTime
datetime2

(SQL Server 2008 and later) 	DateTime 	DateTime2 	None 	DateTime2 	GetDateTime
datetimeoffset

(SQL Server 2008 and later) 	DateTimeOffset 	DateTimeOffset 	none 	DateTimeOffset 	GetDateTimeOffset
decimal 	Decimal 	Decimal 	GetSqlDecimal 	Decimal 	GetDecimal
FILESTREAM attribute (varbinary(max)) 	Byte[] 	VarBinary 	GetSqlBytes 	Binary 	GetBytes
float 	Double 	Float 	GetSqlDouble 	Double 	GetDouble
image 	Byte[] 	Binary 	GetSqlBinary 	Binary 	GetBytes
int 	Int32 	Int 	GetSqlInt32 	Int32 	GetInt32
money 	Decimal 	Money 	GetSqlMoney 	Decimal 	GetDecimal
nchar 	String

Char[] 	NChar 	GetSqlString 	StringFixedLength 	GetString

GetChars
ntext 	String

Char[] 	NText 	GetSqlString 	String 	GetString

GetChars
numeric 	Decimal 	Decimal 	GetSqlDecimal 	Decimal 	GetDecimal
nvarchar 	String

Char[] 	NVarChar 	GetSqlString 	String 	GetString

GetChars
real 	Single 	Real 	GetSqlSingle 	Single 	GetFloat
rowversion 	Byte[] 	Timestamp 	GetSqlBinary 	Binary 	GetBytes
smalldatetime 	DateTime 	DateTime 	GetSqlDateTime 	DateTime 	GetDateTime
smallint 	Int16 	SmallInt 	GetSqlInt16 	Int16 	GetInt16
smallmoney 	Decimal 	SmallMoney 	GetSqlMoney 	Decimal 	GetDecimal
sql_variant 	Object 2 	Variant 	GetSqlValue 2 	Object 	GetValue 2
text 	String

Char[] 	Text 	GetSqlString 	String 	GetString

GetChars
time

(SQL Server 2008 and later) 	TimeSpan 	Time 	none 	Time 	GetDateTime
timestamp 	Byte[] 	Timestamp 	GetSqlBinary 	Binary 	GetBytes
tinyint 	Byte 	TinyInt 	GetSqlByte 	Byte 	GetByte
uniqueidentifier 	Guid 	UniqueIdentifier 	GetSqlGuid 	Guid 	GetGuid
varbinary 	Byte[] 	VarBinary 	GetSqlBinary 	Binary 	GetBytes
varchar 	String

Char[] 	VarChar 	GetSqlString 	AnsiString, String 	GetString

GetChars
xml 	Xml 	Xml 	GetSqlXml 	Xml 	none
 * 
 * 
 * 
 * 
<thead>
<tr>
<th class="x-hidden-focus">SQL Server Database Engine type</th>
<th>.NET Framework type</th>
<th>SqlDbType enumeration</th>
<th>SqlDataReader SqlTypes typed accessor</th>
<th>DbType enumeration</th>
<th>SqlDataReader DbType typed accessor</th>
</tr>
</thead>
<tbody>
<tr>
<td>bigint</td>
<td>Int64</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_BigInt" data-linktype="relative-path">BigInt</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlint64" data-linktype="relative-path">GetSqlInt64</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Int64" data-linktype="relative-path">Int64</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getint64" data-linktype="relative-path">GetInt64</a></td>
</tr>
<tr>
<td>binary</td>
<td>Byte[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_VarBinary" data-linktype="relative-path">VarBinary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbinary" data-linktype="relative-path">GetSqlBinary</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbytes" data-linktype="relative-path">GetBytes</a></td>
</tr>
<tr>
<td>bit</td>
<td>Boolean</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Bit" data-linktype="relative-path">Bit</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlboolean" data-linktype="relative-path">GetSqlBoolean</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Boolean" data-linktype="relative-path">Boolean</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getboolean" data-linktype="relative-path">GetBoolean</a></td>
</tr>
<tr>
<td>char</td>
<td>String<br><br> Char[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Char" data-linktype="relative-path">Char</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlstring" data-linktype="relative-path">GetSqlString</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_AnsiStringFixedLength" data-linktype="relative-path">AnsiStringFixedLength</a>,<br><br> <a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_String" data-linktype="relative-path">String</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getstring" data-linktype="relative-path">GetString</a><br><br> <a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getchars" data-linktype="relative-path">GetChars</a></td>
</tr>
<tr>
<td>date <sup>1</sup><br><br> (SQL Server 2008 and later)</td>
<td>DateTime</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Date" data-linktype="relative-path">Date</a> <sup>1</sup></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqldatetime" data-linktype="relative-path">GetSqlDateTime</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Date" data-linktype="relative-path">Date</a> <sup>1</sup></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdatetime" data-linktype="relative-path">GetDateTime</a></td>
</tr>
<tr>
<td>datetime</td>
<td>DateTime</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_DateTime" data-linktype="relative-path">DateTime</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqldatetime" data-linktype="relative-path">GetSqlDateTime</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_DateTime" data-linktype="relative-path">DateTime</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdatetime" data-linktype="relative-path">GetDateTime</a></td>
</tr>
<tr>
<td>datetime2<br><br> (SQL Server 2008 and later)</td>
<td>DateTime</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_DateTime2" data-linktype="relative-path">DateTime2</a></td>
<td>None</td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_DateTime2" data-linktype="relative-path">DateTime2</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdatetime" data-linktype="relative-path">GetDateTime</a></td>
</tr>
<tr>
<td>datetimeoffset<br><br> (SQL Server 2008 and later)</td>
<td>DateTimeOffset</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_DateTimeOffset" data-linktype="relative-path">DateTimeOffset</a></td>
<td>none</td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_DateTimeOffset" data-linktype="relative-path">DateTimeOffset</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdatetimeoffset" data-linktype="relative-path">GetDateTimeOffset</a></td>
</tr>
<tr>
<td>decimal</td>
<td>Decimal</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Decimal" data-linktype="relative-path">Decimal</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqldecimal" data-linktype="relative-path">GetSqlDecimal</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Decimal" data-linktype="relative-path">Decimal</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdecimal" data-linktype="relative-path">GetDecimal</a></td>
</tr>
<tr>
<td>FILESTREAM attribute (varbinary(max))</td>
<td>Byte[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_VarBinary" data-linktype="relative-path">VarBinary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbytes" data-linktype="relative-path">GetSqlBytes</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbytes" data-linktype="relative-path">GetBytes</a></td>
</tr>
<tr>
<td>float</td>
<td>Double</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Float" data-linktype="relative-path">Float</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqldouble" data-linktype="relative-path">GetSqlDouble</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Double" data-linktype="relative-path">Double</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdouble" data-linktype="relative-path">GetDouble</a></td>
</tr>
<tr>
<td>image</td>
<td>Byte[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbinary" data-linktype="relative-path">GetSqlBinary</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbytes" data-linktype="relative-path">GetBytes</a></td>
</tr>
<tr>
<td>int</td>
<td>Int32</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Int" data-linktype="relative-path">Int</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlint32" data-linktype="relative-path">GetSqlInt32</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Int32" data-linktype="relative-path">Int32</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getint32" data-linktype="relative-path">GetInt32</a></td>
</tr>
<tr>
<td>money</td>
<td>Decimal</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Money" data-linktype="relative-path">Money</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlmoney" data-linktype="relative-path">GetSqlMoney</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Decimal" data-linktype="relative-path">Decimal</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdecimal" data-linktype="relative-path">GetDecimal</a></td>
</tr>
<tr>
<td>nchar</td>
<td>String<br><br> Char[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_NChar" data-linktype="relative-path">NChar</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlstring" data-linktype="relative-path">GetSqlString</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_StringFixedLength" data-linktype="relative-path">StringFixedLength</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getstring" data-linktype="relative-path">GetString</a><br><br> <a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getchars" data-linktype="relative-path">GetChars</a></td>
</tr>
<tr>
<td>ntext</td>
<td>String<br><br> Char[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_NText" data-linktype="relative-path">NText</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlstring" data-linktype="relative-path">GetSqlString</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_String" data-linktype="relative-path">String</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getstring" data-linktype="relative-path">GetString</a><br><br> <a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getchars" data-linktype="relative-path">GetChars</a></td>
</tr>
<tr>
<td>numeric</td>
<td>Decimal</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Decimal" data-linktype="relative-path">Decimal</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqldecimal" data-linktype="relative-path">GetSqlDecimal</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Decimal" data-linktype="relative-path">Decimal</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdecimal" data-linktype="relative-path">GetDecimal</a></td>
</tr>
<tr>
<td>nvarchar</td>
<td>String<br><br> Char[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_NVarChar" data-linktype="relative-path">NVarChar</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlstring" data-linktype="relative-path">GetSqlString</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_String" data-linktype="relative-path">String</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getstring" data-linktype="relative-path">GetString</a><br><br> <a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getchars" data-linktype="relative-path">GetChars</a></td>
</tr>
<tr>
<td>real</td>
<td>Single</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Real" data-linktype="relative-path">Real</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlsingle" data-linktype="relative-path">GetSqlSingle</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Single" data-linktype="relative-path">Single</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getfloat" data-linktype="relative-path">GetFloat</a></td>
</tr>
<tr>
<td>rowversion</td>
<td>Byte[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Timestamp" data-linktype="relative-path">Timestamp</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbinary" data-linktype="relative-path">GetSqlBinary</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbytes" data-linktype="relative-path">GetBytes</a></td>
</tr>
<tr>
<td>smalldatetime</td>
<td>DateTime</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_DateTime" data-linktype="relative-path">DateTime</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqldatetime" data-linktype="relative-path">GetSqlDateTime</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_DateTime" data-linktype="relative-path">DateTime</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdatetime" data-linktype="relative-path">GetDateTime</a></td>
</tr>
<tr>
<td>smallint</td>
<td>Int16</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_SmallInt" data-linktype="relative-path">SmallInt</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlint16" data-linktype="relative-path">GetSqlInt16</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Int16" data-linktype="relative-path">Int16</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getint16" data-linktype="relative-path">GetInt16</a></td>
</tr>
<tr>
<td>smallmoney</td>
<td>Decimal</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_SmallMoney" data-linktype="relative-path">SmallMoney</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlmoney" data-linktype="relative-path">GetSqlMoney</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Decimal" data-linktype="relative-path">Decimal</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdecimal" data-linktype="relative-path">GetDecimal</a></td>
</tr>
<tr>
<td>sql_variant</td>
<td>Object <sup>2</sup></td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Variant" data-linktype="relative-path">Variant</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlvalue" data-linktype="relative-path">GetSqlValue</a> <sup>2</sup></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Object" data-linktype="relative-path">Object</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getvalue" data-linktype="relative-path">GetValue</a> <sup>2</sup></td>
</tr>
<tr>
<td>text</td>
<td>String<br><br> Char[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Text" data-linktype="relative-path">Text</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlstring" data-linktype="relative-path">GetSqlString</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_String" data-linktype="relative-path">String</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getstring" data-linktype="relative-path">GetString</a><br><br> <a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getchars" data-linktype="relative-path">GetChars</a></td>
</tr>
<tr>
<td>time<br><br> (SQL Server 2008 and later)</td>
<td>TimeSpan</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Time" data-linktype="relative-path">Time</a></td>
<td>none</td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Time" data-linktype="relative-path">Time</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getdatetime" data-linktype="relative-path">GetDateTime</a></td>
</tr>
<tr>
<td>timestamp</td>
<td>Byte[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Timestamp" data-linktype="relative-path">Timestamp</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbinary" data-linktype="relative-path">GetSqlBinary</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbytes" data-linktype="relative-path">GetBytes</a></td>
</tr>
<tr>
<td>tinyint</td>
<td>Byte</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_TinyInt" data-linktype="relative-path">TinyInt</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbyte" data-linktype="relative-path">GetSqlByte</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Byte" data-linktype="relative-path">Byte</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbyte" data-linktype="relative-path">GetByte</a></td>
</tr>
<tr>
<td>uniqueidentifier</td>
<td>Guid</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_UniqueIdentifier" data-linktype="relative-path">UniqueIdentifier</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlguid" data-linktype="relative-path">GetSqlGuid</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Guid" data-linktype="relative-path">Guid</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getguid" data-linktype="relative-path">GetGuid</a></td>
</tr>
<tr>
<td>varbinary</td>
<td>Byte[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_VarBinary" data-linktype="relative-path">VarBinary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlbinary" data-linktype="relative-path">GetSqlBinary</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Binary" data-linktype="relative-path">Binary</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getbytes" data-linktype="relative-path">GetBytes</a></td>
</tr>
<tr>
<td>varchar</td>
<td>String<br><br> Char[]</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_VarChar" data-linktype="relative-path">VarChar</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlstring" data-linktype="relative-path">GetSqlString</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_AnsiString" data-linktype="relative-path">AnsiString</a>, <a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_String" data-linktype="relative-path">String</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getstring" data-linktype="relative-path">GetString</a><br><br> <a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getchars" data-linktype="relative-path">GetChars</a></td>
</tr>
<tr>
<td>xml</td>
<td>Xml</td>
<td><a class="xref" href="../../../api/system.data.sqldbtype#System_Data_SqlDbType_Xml" data-linktype="relative-path">Xml</a></td>
<td><a class="xref" href="../../../api/system.data.sqlclient.sqldatareader.getsqlxml" data-linktype="relative-path">GetSqlXml</a></td>
<td><a class="xref" href="../../../api/system.data.dbtype#System_Data_DbType_Xml" data-linktype="relative-path">Xml</a></td>
<td>none</td>
</tr>
</tbody>
*/
