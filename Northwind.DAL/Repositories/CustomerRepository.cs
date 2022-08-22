using tools.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The customer repository
    /// </summary>
    public sealed class CustomerRepository : Repository<Customer, string>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public CustomerRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Customer> FetchAll
        {
            get
            {
                IQueryable<Customer> result = Context.Customers
                                                    .Include(c => c.CustomerCustomerDemo)
                                                        .ThenInclude(t => t.CustomerType)
                                                    .Include(o => o.Orders)
                                                        .ThenInclude(d => d.OrderDetails)
                                                            .ThenInclude(p => p.Product)
                                                    .Include(o => o.Orders)
                                                        .ThenInclude(s => s.ShipViaNavigation)
                                                    .Include(o => o.Orders)
                                                        .ThenInclude(e => e.Employee);

                return result;
            }
        }

        public override void Create(Customer item)
        {
            Context.Add(item);
        }

        public override void Delete(Customer item)
        {
            Context.Remove(item);
        }

        public override Customer Fetch(string id)
        {
            return (from Customer c in FetchAll where c.CustomerId == id select c).FirstOrDefault();
        }

        public override void Update(Customer item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            Context.AttachRange(item.CustomerCustomerDemo);
            Context.AttachRange(item.CustomerCustomerDemo.Select(t => t.CustomerType));
            Context.AttachRange(item.Orders);
            Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));
            Context.AttachRange(item.Orders.Select(s => s.ShipViaNavigation));
            Context.AttachRange(item.Orders.Select(e => e.Employee));

            Context.Update(item);
        }
    }
}
