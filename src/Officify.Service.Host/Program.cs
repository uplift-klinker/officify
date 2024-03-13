using System.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Officify.Azure.Persistence;
using Officify.Core;
using Officify.Service.Host;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(
        (context, services) =>
        {
            var storageAccountConnectionString = context.Configuration.GetValue<string>(
                ConfigurationKeys.AzurePersistence.StorageConnectionString
            );
            var tableName = context.Configuration.GetValue<string>(
                ConfigurationKeys.AzurePersistence.TableName
            );

            if (string.IsNullOrEmpty(storageAccountConnectionString))
            {
                throw new ConfigurationErrorsException(
                    $"configuration is missing '{ConfigurationKeys.AzurePersistence.StorageConnectionString}'"
                );
            }
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ConfigurationErrorsException(
                    $"configuration is missing '{ConfigurationKeys.AzurePersistence.TableName}'"
                );
            }

            services.AddApplicationInsightsTelemetryWorkerService();
            services.ConfigureFunctionsApplicationInsights();
            services
                .AddOfficifyCore()
                .AddOfficifyAzurePersistence(
                    storageAccountConnectionString,
                    opts =>
                    {
                        opts.TableName = tableName;
                    }
                );
        }
    )
    .Build();

host.Run();
