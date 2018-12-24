using System;
using System.Collections.Generic;
using System.Text;
using mezzanine.EF;

namespace mezzanine.Tests.Mock
{
    /// <summary>
    /// mock model to test the mock historic repository.
    /// </summary>
    public class MockHistoricModel : HistoricDbModel<long>, IMockModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
    }
}
