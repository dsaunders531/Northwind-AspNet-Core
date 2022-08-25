using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;
using tools.EF;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The order repository
    /// </summary>
    public sealed class OrderRepository : Repository<Order, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public OrderRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Order> FetchAll
        {
            get
            {
                return Context.Orders
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
            Context.Add(item);
        }

        public override void Delete(Order item)
        {
            Context.Remove(item);
        }

        public override Order Fetch(int id)
        {
            return (from Order o in FetchAll where o.OrderId == id select o).FirstOrDefault();
        }

        public IQueryable<Order> FetchByCustomerId(string customerId)
        {
            return (from Order o in FetchAll where o.CustomerId == customerId select o);
        }

        public IQueryable<Order> FetchByEmployeeId(int employeeId)
        {
            return (from Order o in FetchAll where o.EmployeeId == employeeId select o);
        }

        public override void Update(Order item)
        {
            Context.AttachRange(item.OrderDetails);
            Context.AttachRange(item.OrderDetails.Select(p => p.Product));
            Context.Attach(item.Customer);
            Context.AttachRange(item.Customer.CustomerCustomerDemo.Select(d => d.Customer));
            Context.Attach(item.Employee);
            Context.Attach(item.ShipViaNavigation);

            Context.Update(item);
        }
    }
}
