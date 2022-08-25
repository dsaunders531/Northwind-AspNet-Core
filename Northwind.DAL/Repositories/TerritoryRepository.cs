using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;
using tools.EF;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The territory repository.
    /// </summary>
    public sealed class TerritoryRepository : Repository<Territory, string>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public TerritoryRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Territory> FetchAll
        {
            get
            {
                return Context.Territories
                            .Include(r => r.Region)
                            .Include(e => e.EmployeeTerritories)
                                .ThenInclude(t => t.Territory);
            }
        }

        public override void Create(Territory item)
        {
            Context.Add(item);
        }

        public override void Update(Territory item)
        {
            Context.Attach(item.Region);
            Context.AttachRange(item.EmployeeTerritories);
            Context.AttachRange(item.EmployeeTerritories.Select(t => t.Territory));

            Context.Update(item);
        }

        public override void Delete(Territory item)
        {
            Context.Remove(item);
        }

        public override Territory Fetch(string territoryId)
        {
            return (from Territory t in FetchAll where t.TerritoryId == territoryId select t).FirstOrDefault();
        }

        public IQueryable<Territory> FetchByRegionId(int regionId)
        {
            return (from Territory t in FetchAll where t.RegionId == regionId select t);
        }
    }
}
