using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The territory repository.
    /// </summary>
    public sealed class TerritoryRepository : Repository<Territory, string>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public TerritoryRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Territory> FetchAll
        {
            get
            {
                return this.Context.Territories
                            .Include(r => r.Region)
                            .Include(e => e.EmployeeTerritories)
                                .ThenInclude(t => t.Territory);
            }
        }

        public override void Create(Territory item)
        {
            this.Context.Add(item);
        }

        public override void Update(Territory item)
        {
            this.Context.Attach(item.Region);
            this.Context.AttachRange(item.EmployeeTerritories);
            this.Context.AttachRange(item.EmployeeTerritories.Select(t => t.Territory));

            this.Context.Update(item);
        }

        public override void Delete(Territory item)
        {
            this.Context.Remove(item);
        }

        public override Territory Fetch(string territoryId)
        {
            return (from Territory t in this.FetchAll where t.TerritoryId == territoryId select t).FirstOrDefault();
        }

        public IQueryable<Territory> FetchByRegionId(int regionId)
        {
            return (from Territory t in this.FetchAll where t.RegionId == regionId select t);
        }
    }
}
