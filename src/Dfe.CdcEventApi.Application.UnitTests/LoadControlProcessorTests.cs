namespace Dfe.CdcEventApi.Application.UnitTests
{
    using Dfe.CdcEventApi.Domain.Definitions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class LoadControlProcessorTests
    {
        private Mock<IEntityStorageAdapter> mockEntityStorageAdapter;

        private LoadControlProcessor unit;

        [TestInitialize]
        public void Arrange()
        {
            this.mockLoadControlStorageAdapter = new Mock<IEntityStorageAdapter>();

            Mock<ILoggerProvider> mockLoggerProvider = new Mock<ILoggerProvider>();

            this.unit = new EntityProcessor(
                this.mockEntityStorageAdapter.Object,
                mockLoggerProvider.Object);
        }

        [TestMethod]
        public Task WhenPostingWithoutARunIdentifier_ItShouldThrowAnExcpetion
        {

        }
    }
}