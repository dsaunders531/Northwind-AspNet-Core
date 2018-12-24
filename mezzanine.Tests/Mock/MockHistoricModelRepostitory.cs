using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mezzanine.EF;
using mezzanine.Test;

namespace mezzanine.Tests.Mock
{
    /// <summary>
    /// The mock historic repository for the MockModel and MockHistoricModel
    /// </summary>
    public class MockHistoricModelService : HistoricRecordsService<MockModel, long, MockHistoricModel>
    {
        public MockHistoricModelService(
            IRepository<MockModel, long> currentRecordRepository, 
            IRepository<MockHistoricModel, long> historicRecordRepository) 
            : base(currentRecordRepository, historicRecordRepository)
        {
        }
    }
}
