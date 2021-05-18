namespace Dfe.CdcEventApi.Application.UnitTests
{
    using Dfe.CdcEventApi.Domain.Definitions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Threading.Tasks;

    [TestClass]
    public class LoadProcessorTests
    {
        private Mock<ILoadStorageAdapter> mockLoadStorageAdapter;

        private LoadProcessor unit;

        [TestInitialize]
        public void Arrange()
        {
            mockLoadStorageAdapter = new Mock<ILoadStorageAdapter>();

            Mock<ILoggerProvider> mockLoggerProvider = new Mock<ILoggerProvider>();

            this.unit = new LoadProcessor(
                this.mockLoadStorageAdapter.Object,
                mockLoggerProvider.Object);
        }

        [TestMethod]
        public async Task WhenPostingWithoutARunIdentifier_ItShouldThrowAnException()
        {
            Assert.Fail();
        }
    }
}