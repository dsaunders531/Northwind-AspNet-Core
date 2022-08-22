using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace mezzanine
{
    /// <summary>
    /// Interface for a database client
    /// </summary>
    public interface IDbClient : IDisposable
    {
        T Fill<T>(DbCommand command);
        T Fill<T>(string strSql);
        DataTable GetDataTable(string strSql, string tableName);
        DataTable GetDataTable(DbCommand command, string tableName);
        T GetOneField<T>(string strSql, bool noLock);
        T GetOneField<T>(DbCommand command, bool noLock);
        int RunUpsert(string strSql);
        int RunUpsert(DbCommand command);
        int RunStoredProc(DbCommand command);
        int RunTransaction(List<string> strsSql, string transactionName);
        int RunTransaction(List<DbCommand> commands, string transactionName);
    }
}
