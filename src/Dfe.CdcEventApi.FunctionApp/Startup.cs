namespace Dfe.CdcEventApi.FunctionApp
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Dfe.CdcEventApi.Application;
    using Dfe.CdcEventApi.Application.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions;
    using Dfe.CdcEventApi.Domain.Definitions.SettingsProviders;
    using Dfe.CdcEventApi.FunctionApp.Infrastructure;
    using Dfe.CdcEventApi.FunctionApp.Infrastructure.SettingsProviders;
    using Dfe.CdcEventApi.Infrastructure.SqlServer;
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Azure.WebJobs.Logging;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ILoggerProvider = Dfe.CdcEventApi.Domain.Definitions.ILoggerProvider;

    /// <summary>
    /// Functions startup class.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Startup : FunctionsStartup
    {
        /// <inheritdoc />
        public override void Configure(IFunctionsHostBuilder functionsHostBuilder)
        {
            if (functionsHostBuilder == null)
            {
                throw new ArgumentNullException(nameof(functionsHostBuilder));
            }

            IServiceCollection serviceCollection =
                functionsHostBuilder.Services;

            AddLogging(serviceCollection);
            AddSettingsProviders(serviceCollection);
            AddProcessors(serviceCollection);
            AddAdapters(serviceCollection);
        }

        private static void AddLogging(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<ILogger>(CreateILogger)
                .AddScoped<ILoggerProvider, LoggerProvider>();
        }

        private static void AddSettingsProviders(
            IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<IEntityStorageAdapterSettingsProvider, EntityStorageAdapterSettingsProvider>();
            serviceCollection
               .AddSingleton<IBlobSettingsProvider, BlobSettingsProvider>();
        }

        private static void AddProcessors(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IEntityProcessor, EntityProcessor>();
            serviceCollection
                .AddScoped<IBlobProcessor, BlobProcessor>();
            serviceCollection
                .AddScoped<IBlobConvertor, BlobConvertor>();
            serviceCollection
                .AddScoped<ILoadProcessor, LoadProcessor>();
        }

        private static void AddAdapters(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddScoped<IEntityStorageAdapter, EntityStorageAdapter>();
            serviceCollection
                .AddScoped<ILoadStorageAdapter, LoadStorageAdapter>();
        }

        private static ILogger CreateILogger(IServiceProvider serviceProvider)
        {
            ILogger toReturn = null;

            ILoggerFactory loggerFactory =
                serviceProvider.GetService<ILoggerFactory>();

            string categoryName = LogCategories.CreateFunctionUserCategory(
                nameof(Dfe.CdcEventApi));

            toReturn = loggerFactory.CreateLogger(categoryName);

            return toReturn;
        }
    }
}