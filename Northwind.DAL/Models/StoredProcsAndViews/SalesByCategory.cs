namespace Northwind.DAL.Models
{
    /// <summary>
    /// Model for the SalesByCategory stored procedure.
    /// </summary>
    public class SalesByCategory
    {
        public string ProductName { get; set; }
        public decimal TotalPurchase { get; set; }
    }
}
