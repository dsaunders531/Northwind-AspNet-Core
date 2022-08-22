using mezzanine.Extensions;
using mezzanine.Utility;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Northwind.DAL
{
    /// <summary>
    /// The extention methods for views and stored procs for the northwind context.
    /// </summary>
    public partial class NorthwindContext
    {
        /// <summary>
        /// Get the results of the TenMostExpensiveProducts stored procedure.
        /// </summary>
        /// <returns>A complex type table a list of most expensive product.</returns>
        public List<MostExpensiveProduct> TenMostExpensiveProducts()
        {
            List<MostExpensiveProduct> result = new List<MostExpensiveProduct>();

            using (DbCommand command = this.CreateCommand("exec [Ten Most Expensive Products]"))
            {
                using (DbClient client = this.GetAdvancedClient())
                {
                    DataTable table = client.GetDataTable(command, "TenMostExpensiveProducts");
                    result = client.DataTableToT<List<MostExpensiveProduct>>(table);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the results of the SalesByCategory stored procedure.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="ordYear"></param>
        /// <returns></returns>
        public List<SalesByCategory> SalesByCategory(string categoryName, short ordYear)
        {
            List<SalesByCategory> result = new List<SalesByCategory>();

            using (DbCommand command = this.CreateCommand("exec [SalesByCategory] @categoryName, @ordYear"))
            {
                command.Parameters.Add(this.CreateParameter("@categoryName", DbType.String, 15, categoryName));
                command.Parameters.Add(this.CreateParameter("@ordYear", DbType.String, 4, ordYear.ToString()));

                using (DbClient client = this.GetAdvancedClient())
                {
                    DataTable table = client.GetDataTable(command, "SalesByCategory");
                    result = client.DataTableToT<List<SalesByCategory>>(table);
                }
            }

            return result;
        }

        /// <summary>
        /// Get the results of the Sales by Year stored procedure.
        /// </summary>
        /// <param name="beginningDate"></param>
        /// <param name="endingDate"></param>
        /// <returns></returns>
        public List<SalesByYear> SalesByYear(DateTime beginningDate, DateTime endingDate)
        {
            List<SalesByYear> result = new List<SalesByYear>();

            using (DbCommand command = this.CreateCommand("exec [Sales by Year] @beginningDate, @endingDate"))
            {
                command.Parameters.Add(this.CreateParameter("@beginningDate", DbType.DateTime, beginningDate.Date));
                command.Parameters.Add(this.CreateParameter("@endingDate", DbType.DateTime, endingDate.Date));

                using (DbClient client = this.GetAdvancedClient())
                {
                    DataTable table = client.GetDataTable(command, "SalesByYear");
                    result = client.DataTableToT<List<SalesByYear>>(table);
                }
            }

            return result;
        }
    }
}
