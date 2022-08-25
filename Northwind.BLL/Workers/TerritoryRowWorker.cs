using Northwind.BLL.Models;
using Northwind.DAL.Models;
using System.Collections.Generic;
using tools.EF;
using tools.WorkerPattern;

namespace Northwind.BLL.Workers
{
    public class TerritoryRowWorker : GenericWorker<Territory, int, TerritoryRowApiO, string>
    {
        public TerritoryRowWorker(IRepository<Territory, int> repository) : base(repository)
        {
        }

        public override TerritoryRowApiO Create(TerritoryRowApiO apiRowModel)
        {
            return base.Create(apiRowModel, model => model.RegionId == apiRowModel.RegionId
                                                    && model.TerritoryDescription == apiRowModel.TerritoryDescription);
        }

        public override List<TerritoryRowApiO> FetchAll()
        {
            return base.FetchAll(t => t.TerritoryDescription);
        }

        public override TerritoryRowApiO Update(TerritoryRowApiO apiRowModel)
        {
            return base.Update(apiRowModel, model => model.TerritoryId == apiRowModel.TerritoryId);
        }
    }
}
