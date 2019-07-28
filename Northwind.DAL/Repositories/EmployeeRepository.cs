using duncans.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The employee repository
    /// </summary>
    internal sealed class EmployeeRepository : EFRepositoryBase<EmployeeDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public EmployeeRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<EmployeeDbModel> FetchAll
        {
            get
            {
                return this.Context.Employees
                            .Include(r => r.ReportsToNavigation)
                            .Include(t => t.EmployeeTerritories)
                                .ThenInclude(T => T.Territory)
                            .Include(n => n.InverseReportsToNavigation)
                            .Include(o => o.Orders)
                                .ThenInclude(d => d.OrderDetails)
                                    .ThenInclude(p => p.Product)
                            .Include(a => a.Orders)
                                .ThenInclude(c => c.Customer)
                            .Include(b => b.Orders)
                                .ThenInclude(s => s.ShipViaNavigation);

            }
        }

        public override IQueryable<EmployeeDbModel> FetchRaw => throw new System.NotImplementedException();

        public override void Create(EmployeeDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(EmployeeDbModel item)
        {
            this.IgnoreRelations(item);
            this.Context.Remove(item);
        }

        public override EmployeeDbModel Fetch(int id)
        {
            return (from EmployeeDbModel e in this.FetchAll where e.RowId == id select e).FirstOrDefault();
        }

        public override void Update(EmployeeDbModel item)
        {
            // Update the database but ignore all the linked data(Includes and ThenIncludes )
            this.IgnoreRelations(item);
            this.Context.Update(item);
        }

        public override void Ignore(EmployeeDbModel item)
        {
            this.Context.Attach(item);
        }

        protected override void IgnoreRelations(EmployeeDbModel item)
        {
            this.Context.Attach(item.ReportsToNavigation);
            this.Context.AttachRange(item.EmployeeTerritories);
            this.Context.AttachRange(item.EmployeeTerritories.Select(T => T.Territory));
            this.Context.AttachRange(item.InverseReportsToNavigation);
            this.Context.AttachRange(item.Orders);
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            this.Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));
            this.Context.AttachRange(item.Orders.Select(c => c.Customer));
            this.Context.AttachRange(item.Orders.Select(n => n.ShipViaNavigation));
        }
    }
}
