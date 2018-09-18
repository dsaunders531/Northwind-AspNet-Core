using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The order repository
    /// </summary>
    public sealed class OrderRepository : Repository<Order, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public OrderRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Order> FetchAll
        {
            get
            {
                return this.Context.Orders
                            .Include(d => d.OrderDetails)
                                .ThenInclude(p => p.Product)
                            .Include(c => c.Customer)
                                .ThenInclude(e => e.CustomerCustomerDemo)
                            .Include(m => m.Employee)
                            .Include(s => s.ShipViaNavigation);
            }
        }

        public override void Create(Order item)
        {
            this.Context.Add(item);
        }

        public override void Delete(Order item)
        {
            this.Context.Remove(item);
        }

        public override Order Fetch(int id)
        {
            return (from Order o in this.FetchAll where o.OrderId == id select o).FirstOrDefault();
        }

        public IQueryable<Order> FetchByCustomerId(string customerId)
        {
            return (from Order o in this.FetchAll where o.CustomerId == customerId select o);
        }

        public IQueryable<Order> FetchByEmployeeId(int employeeId)
        {
            return (from Order o in this.FetchAll where o.EmployeeId == employeeId select o);
        }

        public override void Update(Order item)
        {
            this.Context.AttachRange(item.OrderDetails);
            this.Context.AttachRange(item.OrderDetails.Select(p => p.Product));
            this.Context.Attach(item.Customer);
            this.Context.AttachRange(item.Customer.CustomerCustomerDemo.Select(d => d.Customer));
            this.Context.Attach(item.Employee);
            this.Context.Attach(item.ShipViaNavigation);

            this.Context.Update(item);
        }
    }
}
