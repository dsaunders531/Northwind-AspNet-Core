using System;
using System.Collections.Generic;
using System.Text;
using mezzanine.EF;

namespace mezzanine.Tests.Mock
{
    public interface IMockModel
    {
        string Name { get; set; }

        string Address { get; set; }

        string PostCode { get; set; }
    }

    /// <summary>
    /// model for testing the mock repository.
    /// </summary>
    public class MockModel : DbModel<long>, IMockModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string PostCode { get; set; }
    }
}
