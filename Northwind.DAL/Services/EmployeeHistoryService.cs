using duncans.EF;
using Northwind.DAL.Models;

namespace Northwind.DAL.Services
{
    public class EmployeeHistoryService : HistoricRecordsService<EmployeeDbModel, int, EmployeeHistoryDbModel>
    {
        public EmployeeHistoryService(IRepository<EmployeeDbModel, int> currentRecordRepository, IRepository<EmployeeHistoryDbModel, long> historicRecordRepository) : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }
}
