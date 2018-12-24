using mezzanine.Tests.Mock;
using System;
using System.Linq;
using Xunit;

namespace mezzanine.Tests
{
    public class MockRepositoryTests
    {
        /// <summary>
        /// Basic repository tests.
        /// </summary>
        [Fact]
        public void TestMockRepository()
        {
            MockModelRepository repo = new MockModelRepository();

            // Create a model
            MockModel test1 = new MockModel()
            {
                Name = "Barry Test",
                Address = "Test address",
                PostCode = "AA1 1AA"
            };

            // Save it
            repo.Create(test1);
            repo.Commit();

            Assert.True(repo.FetchAll.Count() == 1, "The record was not saved.");
            Assert.True(repo.FetchAll.First().RowId != default(long), "A record Id was not created");
            Assert.True(repo.FetchAll.First().RowVersion == 1, "A row version was not created");

            // Change it
            long rowId = test1.RowId;
            test1.Name = "Berlinda Test";
            repo.Update(test1);
            repo.Commit();

            Assert.True(repo.FetchAll.First().RowId == rowId, "The row Id should not have changed");
            Assert.True(repo.Fetch(rowId).Name == "Berlinda Test", "The name was not updated");
            Assert.True(repo.Fetch(rowId).RowVersion == 2, "The row version was not updated.");

            // Delete it
            repo.Delete(test1);
            repo.Commit();

            Assert.True(repo.FetchAll.Count() == 0, "The model was not deleted.");

            repo = null;
        }

        /// <summary>
        /// Tests for repository with multiple items.
        /// </summary>
        [Fact]
        public void TestMockMultipleItemsTest()
        {
            MockModelRepository repo = new MockModelRepository();

            // Create a model
            MockModel test1 = new MockModel()
            {
                Name = "Barry Test",
                Address = "Test address",
                PostCode = "AA1 1AA"
            };

            // Store it
            repo.Create(test1);

            // create another
            MockModel test2 = new MockModel()
            {
                Name = "Keith Test",
                Address = "Another Address",
                PostCode = "2AA 2AA"
            };
            repo.Create(test2);

            // Save them
            repo.Commit();

            Assert.True(repo.FetchAll.Count() == 2, "The record was not saved.");
            Assert.True(repo.FetchAll.Min(r => r.RowId) != default(long), "A record Id was not created");

            long rowCounter = 1;

            foreach (MockModel item in repo.FetchAll.OrderBy(m => m.RowId))
            {
                Assert.True(item.RowId == rowCounter, "The row counter was not the expected value");
                rowCounter++;
            }

            // change one
            test2.Name = "Kelisha Test";
            repo.Update(test2);
            repo.Commit();

            // see the changes have been made and did not affect the other.
            Assert.True(repo.FetchAll.Count() == 2, "There are an incorrect quantity of records.");

            Assert.True(repo.Fetch(1).Name == "Barry Test" 
                        && repo.Fetch(1).Address == "Test address" 
                        && repo.Fetch(1).PostCode == "AA1 1AA", "A record which should not have been changed has been");

            Assert.True(repo.Fetch(2).Name == "Kelisha Test"
                        && repo.Fetch(2).Address == "Another Address"
                        && repo.Fetch(2).PostCode == "2AA 2AA", "A record which should have changed has not");

            // delete one.
            repo.Delete(test1);
            repo.Commit();

            // see there is 1 less item and the correct one was deleted.
            Assert.True(repo.FetchAll.Count() == 1, "A record was not deleted.");
            Assert.True(repo.Fetch(2).Name == "Kelisha Test", "The wrong record was deleted");

            repo = null;
        }

        /// <summary>
        /// See if adding and changing records creates the appropriate date track record.
        /// </summary>
        [Fact]
        public void TestMockHistoricRepository()
        {
            MockModelRepository mockRepository = new MockModelRepository();
            MockHistoricModelRepository mockHistoricModelRepostitory = new MockHistoricModelRepository();
            MockHistoricModelService historyService = new MockHistoricModelService(mockRepository, mockHistoricModelRepostitory);

            MockModel test1 = new MockModel()
            {
                Name = "Barry Test",
                Address = "Test address",
                PostCode = "AA1 1AA"
            };

            historyService.Create(test1);
            historyService.Commit();

            Assert.True(historyService.FetchAll.Count() == 1, "The record was not saved.");
            Assert.True(historyService.FetchAll.First().RowId != default(long), "A record Id was not created");
            Assert.True(historyService.FetchAll.First().RowVersion == 1, "A row version was not created");
            Assert.True(historyService.FetchHistory(historyService.FetchAll.First().RowId).Count() == 1, "The wrong quantity of historic records was created");

            // Update record
            long rowId = historyService.FetchAll.First().RowId;
            test1.Name = "Berlinda Test";
            historyService.Update(test1);
            historyService.Commit();

            Assert.True(historyService.FetchAll.First().RowId == rowId, "The row Id should not have changed");
            Assert.True(historyService.Fetch(rowId).Name == "Berlinda Test", "The name was not updated");
            Assert.True(historyService.Fetch(rowId).RowVersion == 2, "The row version was not updated.");
            Assert.True(historyService.FetchHistory(rowId).Count() == 2, "A new history record was not created");

            MockHistoricModel historicRecord = historyService.FetchHistoric(rowId, DateTime.Now);
            Assert.True(historicRecord.Action == EFActionType.Update, "The historic record does not have a action type of updated.");
            Assert.True(historicRecord.Name == test1.Name, "The historic record is not correct.");

            // Delete record
            historyService.Delete(test1);
            historyService.Commit();

            Assert.True(historyService.FetchAll.Count() == 1, "The model was not deleted.");
            Assert.True(historyService.FetchHistory(rowId).Count() == 3, "A historic record was not created.");

            historicRecord = historyService.FetchHistoric(rowId, DateTime.Now);
            Assert.True(historicRecord.Action == EFActionType.Delete, "The historic record does not have a action type of Delete.");
            Assert.True(historicRecord.Name == test1.Name, "The historic record is not correct.");

            historyService = null;
            mockRepository = null;
            mockHistoricModelRepostitory = null;
        }

        [Fact]
        public void TestMockHistoricRepostitoryWithMultiples()
        {
            MockModelRepository mockRepository = new MockModelRepository();
            MockHistoricModelRepository mockHistoricModelRepostitory = new MockHistoricModelRepository();
            MockHistoricModelService historyService = new MockHistoricModelService(mockRepository, mockHistoricModelRepostitory);

            // Create a model
            MockModel test1 = new MockModel()
            {
                Name = "Barry Test",
                Address = "Test address",
                PostCode = "AA1 1AA"
            };

            // Store it
            historyService.Create(test1);
            historyService.Commit(); // have to commit each one because of constraints with the history service.

            // create another
            MockModel test2 = new MockModel()
            {
                Name = "Keith Test",
                Address = "Another Address",
                PostCode = "2AA 2AA"
            };
            historyService.Create(test2);
            historyService.Commit();

            Assert.True(historyService.FetchAll.Count() == 2, "The record was not saved.");
            Assert.True(historyService.FetchAll.Min(r => r.RowId) != default(long), "A record Id was not created");

            Assert.True(historyService.FetchHistory(1).Count() == 1, "A history record was not created.");
            Assert.True(historyService.FetchHistory(2).Count() == 1, "A history record was not created.");
            Assert.True(mockHistoricModelRepostitory.FetchAll.Count() == 2, "Wrong quantity of historic records.");

            long rowCounter = 1;

            foreach (MockModel item in historyService.FetchAll.OrderBy(m => m.RowId))
            {
                Assert.True(item.RowId == rowCounter, "The row counter was not the expected value");
                rowCounter++;
            }

            // change one
            test2.Name = "Kelisha Test";
            historyService.Update(test2);
            historyService.Commit();

            // see the changes have been made and did not affect the other.
            Assert.True(historyService.FetchAll.Count() == 2, "There are an incorrect quantity of records.");

            Assert.True(historyService.Fetch(1).Name == "Barry Test"
                        && historyService.Fetch(1).Address == "Test address"
                        && historyService.Fetch(1).PostCode == "AA1 1AA", "A record which should not have been changed has been");

            Assert.True(historyService.Fetch(2).Name == "Kelisha Test"
                        && historyService.Fetch(2).Address == "Another Address"
                        && historyService.Fetch(2).PostCode == "2AA 2AA", "A record which should have changed has not");

            // see the history record has been created.
            Assert.True(historyService.FetchHistoric(2, DateTime.Now).Name == "Kelisha Test", "The historic record has not been created");
            Assert.True(historyService.FetchHistory(2).Count() == 2, "The historic record was not created.");
            Assert.True(mockHistoricModelRepostitory.FetchAll.Count() == 3, "Historic records were not created.");


            // delete one.
            historyService.Delete(test1);
            historyService.Commit();

            // see there is 1 less item and the correct one was deleted.
            Assert.True(historyService.FetchAll.Count() == 1, "A record was not deleted.");
            Assert.True(historyService.Fetch(2).Name == "Kelisha Test", "The wrong record was deleted");

            // see a history record has been created.
            Assert.True(mockHistoricModelRepostitory.FetchAll.Count() == 4, "A historic record for the delete was not created.");

            MockHistoricModel historyRecord = historyService.FetchHistoric(test1.RowId, DateTime.Now);
            Assert.True(historyRecord.Action == EFActionType.Delete, "The history record for the delete was not created");

            historyService = null;
            mockRepository = null;
            mockHistoricModelRepostitory = null;
        }
    }
}
