using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The region repository
    /// </summary>
    public sealed class RegionRepository : Repository<Region, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindContext Context { get; set; }

        public RegionRepository(NorthwindContext context) : base(context) { this.Context = context; }

        public override IQueryable<Region> FetchAll
        {
            get
            {
                return this.Context.Region
                        .Include(t => t.Territories)
                            .ThenInclude(e => e.EmployeeTerritories)
                                .ThenInclude(m => m.Employee);
            }
        }

        public override void Create(Region item)
        {
            this.Context.Add(item);
        }

        public override void Update(Region item)
        {
            this.Context.AttachRange(item.Territories);
            this.Context.AttachRange(item.Territories.Select(e => e.EmployeeTerritories));
            this.Context.AttachRange(item.Territories.Select(e => e.EmployeeTerritories.Select(a => a.Employee)));

            this.Context.Update(item);
        }

        public override void Delete(Region item)
        {
            this.Context.Remove(item);
        }

        public override Region Fetch(int id)
        {
            return (from Region r in this.FetchAll where r.RegionId == id select r).FirstOrDefault();
        }
    }
}
