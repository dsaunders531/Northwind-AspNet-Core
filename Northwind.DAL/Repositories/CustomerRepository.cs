using duncans.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The customer repository
    /// </summary>
    internal sealed class CustomerRepository : EFRepositoryBase<CustomerDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public CustomerRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<CustomerDbModel> FetchAll
        {
            get
            {
                IQueryable<CustomerDbModel> result = Context.Customers
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

        public override IQueryable<CustomerDbModel> FetchRaw => throw new System.NotImplementedException();

        public override void Create(CustomerDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(CustomerDbModel item)
        {
            this.IgnoreRelations(item);
            this.Context.Remove(item);
        }

        public override CustomerDbModel Fetch(int id)
        {
            return (from CustomerDbModel c in this.FetchAll where c.RowId == id select c).FirstOrDefault();
        }
        
        public override void Update(CustomerDbModel item)
        {
            // Update the database but ignore all the linked data ( Includes and ThenIncludes )
            this.IgnoreRelations(item);

            this.Context.Update(item);
        }

        public override void Ignore(CustomerDbModel item)
        {
            this.Context.Attach(item);
        }

        protected override void IgnoreRelations(CustomerDbModel item)
        {
            this.Context.AttachRange(item.CustomerCustomerDemo);
            this.Context.AttachRange(item.CustomerCustomerDemo.Select(t => t.CustomerType));
            this.Context.AttachRange(item.Orders);
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));
            this.Context.AttachRange(item.Orders.Select(s => s.ShipViaNavigation));
            this.Context.AttachRange(item.Orders.Select(e => e.Employee));
        }
    }
}
