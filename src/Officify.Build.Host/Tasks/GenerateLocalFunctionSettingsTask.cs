using System.Text.Json;
using Cake.Common.Diagnostics;
using Cake.Frosting;
using Officify.Build.Host.Contexts;

namespace Officify.Build.Host.Tasks;

[TaskName("GenerateLocalFunctionSettings")]
public class GenerateLocalFunctionSettingsTask : AsyncFrostingTask<OfficifyBuildContext>
{
    private const string AzuriteStorageConnectionString =
        "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
    private const string WorkerRuntime = "dotnet-isolated";
    private const string EntitiesTableName = "OfficifyEntities";

    public override async Task RunAsync(OfficifyBuildContext context)
    {
        if (File.Exists(context.ServiceHostLocalSettingsFilePath))
        {
            context.Information("Local settings file exists skipping generating one");
            return;
        }

        context.Information("Generating local settings file");
        var settings = GenerateLocalSettingsJson();
        await File.WriteAllTextAsync(context.ServiceHostLocalSettingsFilePath, settings);
    }

    private static string GenerateLocalSettingsJson()
    {
        return JsonSerializer.Serialize(
            new
            {
                IsEncrypted = false,
                Values = new Dictionary<string, string>()
                {
                    { "AzureWebJobsStorage", AzuriteStorageConnectionString },
                    { "FUNCTIONS_WORKER_RUNTIME", WorkerRuntime },
                    { "AzurePersistence:StorageConnectionString", AzuriteStorageConnectionString },
                    { "AzurePersistence:TableName", EntitiesTableName },
                },
                Host = new { CORS = "*" }
            }
        );
    }
}
