// <copyright file="MSSQLDbClient.cs" company="Duncan Saunders">
// Copyright (c) Duncan Saunders. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace duncans.DbClient
{
    /// <summary>
    /// A Db client for MS SQL database provider.
    /// </summary>
    public class MSSQLDbClient : DbClient
    {
        public MSSQLDbClient(string connectionString)
            : base(connectionString)
        {
        }

        public MSSQLDbClient(DbConnection connection)
            : base(connection)
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
        /// <param name="command"></param>
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
        /// <param name="transactionName"></param>
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

        private object GetOneField(string sqlText, bool noLock = true)
        {
            object result = null;

            using (SqlCommand com = new SqlCommand(sqlText) { CommandType = CommandType.Text })
            {
                result = this.GetOneField(com, noLock);
            }

            return result;
        }

        private string GenerateTransactionName()
        {
            string result = this.CreateConnection.WorkstationId + DateTime.UtcNow.Ticks.ToString();
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
                        if ((item.SqlDbType == SqlDbType.NText || item.SqlDbType == SqlDbType.NVarChar
                            || item.SqlDbType == SqlDbType.Text || item.SqlDbType == SqlDbType.VarChar)
                            && item.Value != null)
                        {
                            result = this.IsValidSqlParameter((string)item.Value, true);
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
        /// <param name="runningAsCommand">When running as a command ie: with parameters you can have an apos.</param>
        /// <returns></returns>
        private bool IsValidSqlParameter(string value, bool runningAsCommand)
        {
            bool result = true;

            if (!value.IsNullOrEmpty())
            {
                if (value.Contains(@"'") && runningAsCommand == false)
                {
                    // there is a single apostrophe, double apos is allowed.
                    result = value.Contains(@"''");
                }

                if (result == true)
                {
                    result = this.IsValidStatement(value);
                }
            }

            return result;
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
    }
}