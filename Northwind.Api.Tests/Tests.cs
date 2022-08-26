namespace Northwind.Api.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Sanity()
        {
            Assert.Pass();
        }

        [Test]
        public void Math()
        {
            Assert.That(1 + 1, Is.EqualTo(2));
        }
    }
}