using mezzanine.EF;
using Microsoft.EntityFrameworkCore;
using Northwind.DAL.Models;
using System.Linq;

namespace Northwind.DAL.Repositories
{
    /// <summary>
    /// The territory repository.
    /// </summary>
    public sealed class TerritoryRepository : Repository<TerritoryDbModel, int>
    {
        // Override the context so the DbSet tables are visible.
        private new NorthwindDbContext Context { get; set; }

        public TerritoryRepository(NorthwindDbContext context) : base(context) { this.Context = context; }

        public override IQueryable<TerritoryDbModel> FetchAll
        {
            get
            {
                return this.Context.Territories
                            .Include(r => r.Region)
                            .Include(e => e.EmployeeTerritories)
                                .ThenInclude(t => t.Territory);
            }
        }

        public override void Create(TerritoryDbModel item)
        {
            this.Context.Add(item);
        }

        public override void Update(TerritoryDbModel item)
        {
            this.Context.Attach(item.Region);
            this.Context.AttachRange(item.EmployeeTerritories);
            this.Context.AttachRange(item.EmployeeTerritories.Select(t => t.Territory));

            this.Context.Update(item);
        }

        public override void Delete(TerritoryDbModel item)
        {
            this.Context.Remove(item);
        }

        public override TerritoryDbModel Fetch(int territoryId)
        {
            return (from TerritoryDbModel t in this.FetchAll where t.RowId == territoryId select t).FirstOrDefault();
        }

        public IQueryable<TerritoryDbModel> FetchByRegionId(int regionId)
        {
            return (from TerritoryDbModel t in this.FetchAll where t.RegionId == regionId select t);
        }
    }
}
