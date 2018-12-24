using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mezzanine.EF;
using mezzanine.Test;

namespace mezzanine.Tests.Mock
{
    /// <summary>
    /// The mock repository for the MockModel model.
    /// </summary>
    public class MockModelRepository : MockRepository<MockModel, long>
    {
    }

    /// <summary>
    /// The mock repository for the MockHistoricModel.
    /// </summary>
    public class MockHistoricModelRepository : MockRepository<MockHistoricModel, long>
    {
    }
}
