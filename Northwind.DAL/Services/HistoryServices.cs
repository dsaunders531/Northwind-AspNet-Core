using mezzanine.EF;
using Northwind.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Northwind.DAL.Services
{
    public class CustomerHistoryService : HistoricRecordsService<CustomerDbModel, int, CustomerHistoryDbModel>
    {
        public CustomerHistoryService(IRepository<CustomerDbModel, int> currentRecordRepository, IRepository<CustomerHistoryDbModel, long> historicRecordRepository) : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }

    public class ProductHistoryService : HistoricRecordsService<ProductDbModel, int, ProductHistoryDbModel>
    {
        public ProductHistoryService(IRepository<ProductDbModel, int> currentRecordRepository, IRepository<ProductHistoryDbModel, long> historicRecordRepository) : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }

    public class EmployeeHistoryService : HistoricRecordsService<EmployeeDbModel, int, EmployeeHistoryDbModel>
    {
        public EmployeeHistoryService(IRepository<EmployeeDbModel, int> currentRecordRepository, IRepository<EmployeeHistoryDbModel, long> historicRecordRepository) : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }
}
