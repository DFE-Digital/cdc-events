namespace Dfe.CdcEventApi.Application.UnitTests
{
    using System;
    using System.Collections.Generic;
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

        private EntityProcessor entityProcessor;

        [TestInitialize]
        public void Arrange()
        {
            this.mockEntityStorageAdapter = new Mock<IEntityStorageAdapter>();
            
            Mock<ILoggerProvider> mockLoggerProvider = new Mock<ILoggerProvider>();

            this.entityProcessor = new EntityProcessor(
                this.mockEntityStorageAdapter.Object,
                mockLoggerProvider.Object);
        }

        [TestMethod]
        public async Task ProcessEntitiesAsync_ModelsBasesNull_ThrowsArgumentNullException()
        {
            // Arrange
            DateTime runIdentifier = DateTime.UtcNow;
            ExampleEntity[] exampleEntities = null;
            CancellationToken cancellationToken = CancellationToken.None;

            Func<Task> processEntitiesAsync = () =>
            {
                // Act
                return this.entityProcessor.ProcessEntitiesAsync(
                    runIdentifier,
                    exampleEntities,
                    cancellationToken);
            };

            // Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(
                processEntitiesAsync);
        }

        [TestMethod]
        public async Task ProcessEntitiesAsync_ModelMissingDataHandlerAttribute_ThrowsMissingDataHandlerAttributeException()
        {
            // Arrange
            DateTime runIdentifier = DateTime.UtcNow;
            DataHandlerMissingEntity[] dataHandlerMissingEntities = 
                new DataHandlerMissingEntity[]
                {
                    // Empty.
                };
            CancellationToken cancellationToken = CancellationToken.None;

            Func<Task> processEntitiesAsync = () =>
            {
                // Act
                return this.entityProcessor.ProcessEntitiesAsync(
                    runIdentifier,
                    dataHandlerMissingEntities,
                    cancellationToken);
            };

            // Assert
            await Assert.ThrowsExceptionAsync<MissingDataHandlerAttributeException>(
                processEntitiesAsync);
        }

        [TestMethod]
        public async Task ProcessEntitiesAsync_MethodInvokedWithValidModelAndData_CorrectDataHandlerAndXmlRepresentationPassedToDataLayer()
        {
            // Arrange
            List<string> dataHandlerIdentifiers = new List<string>();
            List<XDocument> xDocuments = new List<XDocument>();

            Action<string, DateTime, XDocument, CancellationToken> storeEntitiesAsync =
                (dhi, ri, xd, ct) =>
                {
                    dataHandlerIdentifiers.Add(dhi);
                    xDocuments.Add(xd);
                };

            this.mockEntityStorageAdapter
                .Setup(x => x.StoreEntitiesAsync(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<XDocument>(), It.IsAny<CancellationToken>()))
                .Callback(storeEntitiesAsync);

            string[] expectedDataHandlerIdentifiers = new string[] {
                "ExampleDataHandler",
                "ExampleSubEntityCollectionDataHandler"
            };
            string[] actualDataHandlerIdentifiers = null;

            int expectedNumberOfXDocuments = 4;
            int actualNumberOfXDocuments;

            DateTime runIdentifier = DateTime.UtcNow;
            ExampleEntity[] exampleEntities = ExtractTestData<ExampleEntity>(
                "ExampleEntities.json");
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            await this.entityProcessor.ProcessEntitiesAsync(
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
            TModelsBase[] toReturn = null;

            Type type = typeof(EntityProcessorTests);
            Assembly assembly = type.Assembly;

            string resourcePath =
                $"{type.Namespace}.TestData.{filename}";

            string contentStr = null;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    contentStr = streamReader.ReadToEnd();
                }
            }

            toReturn = JsonConvert.DeserializeObject<TModelsBase[]>(
                contentStr);

            return toReturn;
        }
    }
}