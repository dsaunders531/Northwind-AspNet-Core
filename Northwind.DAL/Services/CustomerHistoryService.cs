using duncans.EF;
using Northwind.DAL.Models;

namespace Northwind.DAL.Services
{
    public class CustomerHistoryService : HistoricRecordsService<CustomerDbModel, int, CustomerHistoryDbModel>
    {
        public CustomerHistoryService(IRepository<CustomerDbModel, int> currentRecordRepository, IRepository<CustomerHistoryDbModel, long> historicRecordRepository) : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }
}
