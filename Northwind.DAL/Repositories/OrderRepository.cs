using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The order repository
    /// </summary>
    public sealed class OrderRepository : Repository<OrderDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public OrderRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<OrderDbModel> FetchAll
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

        public override void Create(OrderDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(OrderDbModel item)
        {
            this.Context.Remove(item);
        }

        public override OrderDbModel Fetch(int id)
        {
            return (from OrderDbModel o in this.FetchAll where o.RowId == id select o).FirstOrDefault();
        }

        public IQueryable<OrderDbModel> FetchByCustomerId(int customerId)
        {
            return (from OrderDbModel o in this.FetchAll where o.CustomerId == customerId select o);
        }

        public IQueryable<OrderDbModel> FetchByEmployeeId(int employeeId)
        {
            return (from OrderDbModel o in this.FetchAll where o.EmployeeId == employeeId select o);
        }

        public override void Update(OrderDbModel item)
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
