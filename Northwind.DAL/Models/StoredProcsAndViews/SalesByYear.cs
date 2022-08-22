using System;

namespace Northwind.DAL.Models
{
    /// <summary>
    /// Model for the SalesByYear stored procedure
    /// </summary>
    public class SalesByYear
    {
        public DateTime ShippedDate { get; set; }
        public int OrderID { get; set; }
        public decimal Subtotal { get; set; }
        public short Year { get; set; }
    }
}
