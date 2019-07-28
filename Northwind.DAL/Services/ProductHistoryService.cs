using duncans.EF;
using Northwind.DAL.Models;

namespace Northwind.DAL.Services
{
    public class ProductHistoryService : HistoricRecordsService<ProductDbModel, int, ProductHistoryDbModel>
    {
        public ProductHistoryService(IRepository<ProductDbModel, int> currentRecordRepository, IRepository<ProductHistoryDbModel, long> historicRecordRepository) : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }
}
