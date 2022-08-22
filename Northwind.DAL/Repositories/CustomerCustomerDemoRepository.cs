using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The customer demographic repository
    /// </summary>
    public sealed class CustomerCustomerDemoRepository : Repository<CustomerCustomerDemo, string>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public CustomerCustomerDemoRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<CustomerCustomerDemo> FetchAll
        {
            get
            {
                return Context.CustomerCustomerDemo.Include(c => c.Customer)
                                                            .ThenInclude(o => o.Orders)
                                                            .ThenInclude(a => a.OrderDetails)
                                                        .Include(d => d.CustomerType);
            }
        }

        public override void Create(CustomerCustomerDemo item)
        {
            Context.Add(item);
        }

        public override void Delete(CustomerCustomerDemo item)
        {
            Context.Remove(item);
        }

        public override CustomerCustomerDemo Fetch(string id)
        {
            return (from CustomerCustomerDemo c in FetchAll where c.CustomerId == id select c).FirstOrDefault();
        }

        public IQueryable<CustomerCustomerDemo> FetchByCustomerId(string customerId)
        {
            return from CustomerCustomerDemo c in FetchAll where c.CustomerId == customerId select c;
        }

        public IQueryable<CustomerCustomerDemo> FetchByCustomerTypeId(string customerTypeId)
        {
            return (from CustomerCustomerDemo c in FetchAll where c.CustomerTypeId == customerTypeId select c);
        }

        public override void Update(CustomerCustomerDemo item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            Context.Attach(item.Customer);
            Context.AttachRange(item.Customer.Orders);
            Context.AttachRange(item.Customer.Orders.Select(o => o.OrderDetails));
            Context.Attach(item.CustomerType);

            Context.Update(item);
        }
    }
}
