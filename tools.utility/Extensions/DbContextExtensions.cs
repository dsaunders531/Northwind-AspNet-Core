using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Data.Common;
using tools.Utility;

namespace tools.Extensions
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// Create and return a DbClient appropriate for the provider.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static DbClient GetAdvancedClient(this DbContext context)
        {
            DbClient result = default(DbClient);

            switch (context.Database.ProviderName)
            {
                case "Microsoft.EntityFrameworkCore.SqlServer":
                    result = new MSSQLDbClient(context.Database.GetDbConnection());
                    break;
                default:
                    throw new NotImplementedException(string.Format("{0} has not been implemented", context.Database.ProviderName));
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates a dbCommand with the specified command text.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static DbCommand CreateCommand(this DbContext context, string commandText)
        {
            DbCommand result = context.Database.GetDbConnection().CreateCommand();
            result.CommandText = commandText;
            return result;
        }

        public static DbParameter CreateParameter(this DbContext context, string parameterName, DbType dbType, object value)
        {
            DbParameter result = context.Database.GetDbConnection().CreateCommand().CreateParameter();

            result.ParameterName = parameterName;
            result.DbType = dbType;
            result.Value = value;

            return result;
        }

        public static DbParameter CreateParameter(this DbContext context, string parameterName, DbType dbType, int size, object value)
        {
            DbParameter result = context.Database.GetDbConnection().CreateCommand().CreateParameter();

            result.ParameterName = parameterName;
            result.DbType = dbType;
            result.Size = size;
            result.Value = value;

            return result;
        }

        public static DbParameter CreateParameter(this DbContext context, string parameterName, DbType dbType, byte precision, byte scale, object value)
        {
            DbParameter result = context.Database.GetDbConnection().CreateCommand().CreateParameter();

            result.ParameterName = parameterName;
            result.DbType = dbType;
            result.Scale = scale;
            result.Precision = precision;
            result.Value = value;

            return result;
        }
    }
}
