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

        public RegionRepository(NorthwindContext context) : base(context) { Context = context; }

        public override IQueryable<Region> FetchAll
        {
            get
            {
                return Context.Region
                        .Include(t => t.Territories)
                            .ThenInclude(e => e.EmployeeTerritories)
                                .ThenInclude(m => m.Employee);
            }
        }

        public override void Create(Region item)
        {
            Context.Add(item);
        }

        public override void Update(Region item)
        {
            Context.AttachRange(item.Territories);
            Context.AttachRange(item.Territories.Select(e => e.EmployeeTerritories));
            Context.AttachRange(item.Territories.Select(e => e.EmployeeTerritories.Select(a => a.Employee)));

            Context.Update(item);
        }

        public override void Delete(Region item)
        {
            Context.Remove(item);
        }

        public override Region Fetch(int id)
        {
            return (from Region r in FetchAll where r.RegionId == id select r).FirstOrDefault();
        }
    }
}
