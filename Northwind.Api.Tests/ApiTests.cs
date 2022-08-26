

using Northwind.BLL.Models;

namespace Northwind.Api.Tests
{
    public class ApiTests
    {
        private HttpClient _client;
        private TestServer _server;

        [SetUp]
        public void Setup()
        {
            _server = new TestServer(new WebHostBuilder()
                                        .UseStartup<Northwind.Startup>());
            _client = _server.CreateClient();
        }

        [Test]
        public async Task GetCategories()
        {
            var response = await _client.GetAsync("/api/category");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            // List<CategoryRowApiO>
            List<CategoryRowApiO> result = (await response.Content.ReadFromJsonAsync<List<CategoryRowApiO>>()) ?? new List<CategoryRowApiO>();

            Assert.That(result.Count, Is.GreaterThan(0));
        }

        // Example TODO add more api tests
        //[TestCase]
        //public async Task PostCategory()
        //{
        //    var response = await _client.PostAsync("/api/category",
        //                                           JsonContent.Create(new Thing
        //                                           {
        //                                               Id = 1234,
        //                                               Some = "Thing"
        //                                           }));
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //    response = await _client.GetAsync("/category/1234");
        //    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        //    var user = await response.Content.ReadFromJsonAsync<CategoryApiO>();
        //    Assert.That(user.Some, Is.EqualTo("Thing"));
        //}
    }
}