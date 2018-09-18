using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// Result for TenMostExpensiveProducts Stored Procedure.
    /// </summary>
    public class MostExpensiveProduct
    {
        public string TenMostExpensiveProducts { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
