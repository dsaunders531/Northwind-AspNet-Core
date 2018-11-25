using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    public sealed class EmployeeTerritoryRepository : Repository<EmployeeTerritoryDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public EmployeeTerritoryRepository(NorthwindDbContext context) : base(context) { this.Context = context;  }

        public override IQueryable<EmployeeTerritoryDbModel> FetchAll
        {
            get
            {
                return this.Context.EmployeeTerritories
                                .Include(e => e.Employee)
                                .Include(t => t.Territory);
            }
        }

        public override void Create(EmployeeTerritoryDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Delete(EmployeeTerritoryDbModel item)
        {
            this.Context.Remove(item);
        }

        public override EmployeeTerritoryDbModel Fetch(int id)
        {
            return (from EmployeeTerritoryDbModel t in this.FetchAll where id == t.EmployeeId select t).FirstOrDefault();
        }

        public IQueryable<EmployeeTerritoryDbModel> FetchByEmployeeId(int employeeId)
        {
            return from EmployeeTerritoryDbModel t in this.FetchAll where employeeId == t.EmployeeId select t;
        }

        public IQueryable<EmployeeTerritoryDbModel> FetchByTerritoryId(string territoryId)
        {
            return from EmployeeTerritoryDbModel t in this.FetchAll where territoryId == t.TerritoryId select t;
        }

        public override void Update(EmployeeTerritoryDbModel item)
        {
            this.Context.Attach(item.Employee);
            this.Context.Attach(item.Territory);

            this.Context.Update(item);
        }
    }
}
