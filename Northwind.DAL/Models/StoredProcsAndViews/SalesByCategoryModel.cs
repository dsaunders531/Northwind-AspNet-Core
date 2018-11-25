using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// Model for the SalesByCategory stored procedure.
    /// </summary>
    public class SalesByCategoryModel
    {
        public string ProductName { get; set; }
        public decimal TotalPurchase { get; set; }
    }
}
