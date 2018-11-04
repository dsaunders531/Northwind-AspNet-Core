using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace mezzanine.DbClient
{
    /// <summary>
    /// The generic DbClient. You will need to implement it for database providers.
    /// </summary>
    public abstract class DbClient : IDbClient
    {
        internal string ConnectionString { set;  get; }

        public DbClient(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public DbClient(DbConnection connection)
        {
            this.ConnectionString = connection.ConnectionString;
        }

        public virtual void Dispose()
        {
            // There are no objects to destroy. But you can use the client in a using statement.
        }

        /// <summary>
        /// Map a datatable to an object. A generic list (List of T) or object can be used. Fields are mapped using the property names.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="table"></param>
        /// <returns></returns>
        public T DataTableToT<T>(DataTable table)
        {
            T result = default(T);
            
            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    result = Activator.CreateInstance<T>();
                    Type typeOfT = result.GetType();

                    if (typeOfT.IsSerializable == true && typeOfT.GetInterface("IList") == typeof(IList))
                    {
                        // create an instance of each row type

                        // find underlying type
                        Type rowType = typeOfT.GenericTypeArguments[0];

                        // fill it
                        foreach (DataRow row in table.Rows)
                        {
                            // create object
                            dynamic rowItem = Activator.CreateInstance(rowType);
                            rowItem = this.DataRowToT(row, table.Columns, rowItem);
                            // add to list
                            ((IList)result).Add(rowItem);
                        }
                    }
                    else
                    {
                        result = this.DataRowToT(table.Rows[0], table.Columns, result);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Map a data row to an objects fields
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="columns"></param>
        /// <param name="inputObject"></param>
        /// <returns></returns>
        private T DataRowToT<T>(DataRow row, DataColumnCollection columns, T inputObject)
        {
            Type outputType = inputObject.GetType();

            foreach (DataColumn column in columns)
            {
                if (row[column] != null)
                {
                    // set the values - note not using case sensitive field names.
                    foreach (PropertyInfo item in outputType.GetProperties())
                    {
                        if (item.Name.ToLower() == column.ColumnName.ToLower())
                        {
                            item.SetValue(inputObject, row[column]);
                            // match found so finish this loop.
                            break;
                        }
                    }
                }
            }

            return inputObject;
        }

        #region "abstract methods"
        public abstract T Fill<T>(DbCommand command);
        public abstract T Fill<T>(string strSql);
        public abstract DataTable GetDataTable(string strSql, string tableName);
        public abstract DataTable GetDataTable(DbCommand command, string tableName);
        public abstract T GetOneField<T>(string strSql, bool noLock);
        public abstract T GetOneField<T>(DbCommand command, bool noLock);
        public abstract int RunStoredProc(DbCommand command);
        public abstract int RunTransaction(List<string> strsSql, string transactionName);
        public abstract int RunTransaction(List<DbCommand> commands, string transactionName);
        public abstract int RunUpsert(string strSql);
        public abstract int RunUpsert(DbCommand command);
        #endregion


    }
}
