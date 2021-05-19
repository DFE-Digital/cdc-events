namespace Dfe.CdcEventApi.Application.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Linq;
    using Dfe.CdcEventApi.Application.Exceptions;
    using Dfe.CdcEventApi.Application.Models;
    using Dfe.CdcEventApi.Application.UnitTests.Models;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Newtonsoft.Json;

    [TestClass]
    public class EntityProcessorTests
    {
        private Mock<IEntityStorageAdapter> mockEntityStorageAdapter;

        private EntityProcessor unit;

        [TestInitialize]
        public void Arrange()
        {
            this.mockEntityStorageAdapter = new Mock<IEntityStorageAdapter>();

            Mock<ILoggerProvider> mockLoggerProvider = new Mock<ILoggerProvider>();

            this.unit = new EntityProcessor(
                this.mockEntityStorageAdapter.Object,
                mockLoggerProvider.Object);
        }

        [TestMethod]
        public async Task CreateEntitiesAsync_ModelsBasesNull_ThrowsArgumentNullException()
        {
            // Arrange
            DateTime runIdentifier = DateTime.UtcNow;
            ExampleEntity[] exampleEntities = null;
            CancellationToken cancellationToken = CancellationToken.None;

            Task processEntitiesAsync()
            {
                // Act
                return this.unit.CreateEntitiesAsync(
                    runIdentifier,
                    exampleEntities,
                    cancellationToken);
            }

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                processEntitiesAsync);
        }

        [TestMethod]
        public async Task CreateEntitiesAsync_ModelMissingDataHandlerAttribute_ThrowsMissingDataHandlerAttributeException()
        {
            // Arrange
            DateTime runIdentifier = DateTime.UtcNow;
            DataHandlerMissingEntity[] dataHandlerMissingEntities =
                new DataHandlerMissingEntity[]
                {
                    // Empty.
                };
            CancellationToken cancellationToken = CancellationToken.None;

            Task processEntitiesAsync()
            {
                // Act
                return this.unit.CreateEntitiesAsync(
                    runIdentifier,
                    dataHandlerMissingEntities,
                    cancellationToken);
            }

            // Assert
            await Assert.ThrowsExceptionAsync<MissingDataHandlerAttributeException>(
                processEntitiesAsync);
        }

        [TestMethod]
        public async Task CreateEntitiesAsync_MethodInvokedWithValidModelAndData_CorrectDataHandlerAndXmlRepresentationPassedToDataLayer()
        {
            // Arrange
            List<string> dataHandlerIdentifiers = new List<string>();
            List<XDocument> xDocuments = new List<XDocument>();

            void storeEntitiesAsync(string dhi, DateTime ri, XDocument xd, CancellationToken ct)
            {
                dataHandlerIdentifiers.Add(dhi);
                xDocuments.Add(xd);
            }

            this.mockEntityStorageAdapter
                .Setup(x => x.StoreEntitiesAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<XDocument>(), It.IsAny<CancellationToken>()))
                .Callback((Action<string, DateTime, XDocument, CancellationToken>)storeEntitiesAsync);

            string[] expectedDataHandlerIdentifiers = new string[] {
                "ExampleDataHandler",
                "ExampleSubEntityDataHandler"
            };
            string[] actualDataHandlerIdentifiers = null;

            int expectedNumberOfXDocuments = 4;
            int actualNumberOfXDocuments;

            DateTime runIdentifier = DateTime.UtcNow;
            ExampleEntity[] exampleEntities = ExtractTestData<ExampleEntity>(
                "ExampleEntities.json");
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await this.unit.CreateEntitiesAsync(
                runIdentifier,
                exampleEntities,
                cancellationToken);

            // Assert
            actualDataHandlerIdentifiers = dataHandlerIdentifiers
                .Distinct()
                .ToArray();

            actualNumberOfXDocuments = xDocuments.Count;

            CollectionAssert.AreEqual(
                expectedDataHandlerIdentifiers,
                actualDataHandlerIdentifiers);
            Assert.AreEqual(
                expectedNumberOfXDocuments,
                actualNumberOfXDocuments);
        }

       
        private static TModelsBase[] ExtractTestData<TModelsBase>(
            string filename)
            where TModelsBase : ModelsBase
        {
            Type type = typeof(EntityProcessorTests);
            Assembly assembly = type.Assembly;

            string resourcePath =
                $"{type.Namespace}.TestData.{filename}";

            string contentStr = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using StreamReader streamReader = new StreamReader(stream);
                contentStr = streamReader.ReadToEnd();
            }

            TModelsBase[] toReturn = JsonConvert.DeserializeObject<TModelsBase[]>(
                contentStr);
            return toReturn;
        }
    }
}