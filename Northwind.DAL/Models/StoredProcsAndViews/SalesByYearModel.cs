using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// Model for the SalesByYear stored procedure
    /// </summary>
    public class SalesByYearModel
    {
        public DateTime ShippedDate { get; set; }
        public int OrderID { get; set; }
        public decimal Subtotal { get; set; }
        public short Year { get; set; }
    }
}
