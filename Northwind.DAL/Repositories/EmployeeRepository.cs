using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;
using tools.EF;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The employee repository
    /// </summary>
    public sealed class EmployeeRepository : Repository<Employee, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public EmployeeRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Employee> FetchAll
        {
            get
            {
                return Context.Employees
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

        public override void Create(Employee item)
        {
            Context.Add(item);
        }

        public override void Delete(Employee item)
        {
            Context.Remove(item);
        }

        public override Employee Fetch(int id)
        {
            return (from Employee e in FetchAll where e.EmployeeId == id select e).FirstOrDefault();
        }

        public override void Update(Employee item)
        {
            // Update the database but ignore all the linked data(Includes and ThenIncludes )
            Context.Attach(item.ReportsToNavigation);
            Context.AttachRange(item.EmployeeTerritories);
            Context.AttachRange(item.EmployeeTerritories.Select(T => T.Territory));
            Context.AttachRange(item.InverseReportsToNavigation);
            Context.AttachRange(item.Orders);
            Context.AttachRange(item.Orders.Select(d => d.OrderDetails));
            Context.AttachRange(item.Orders.Select(d => d.OrderDetails.Select(p => p.Product)));
            Context.AttachRange(item.Orders.Select(c => c.Customer));
            Context.AttachRange(item.Orders.Select(n => n.ShipViaNavigation));

            Context.Update(item);
        }
    }
}
