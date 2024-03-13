FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Directory.Build.props", "src/"]
COPY ["src/Officify.Service.Host/Officify.Service.Host.csproj", "src/Officify.Service.Host/"]
COPY ["src/Officify.Core/Officify.Core.csproj", "src/Officify.Core/"]
COPY ["src/Officify.Models/Officify.Models.csproj", "src/Officify.Models/"]
COPY ["src/Officify.Azure.Persistence/Officify.Azure.Persistence.csproj", "src/Officify.Azure.Persistence/"]
RUN dotnet restore "src/Officify.Service.Host/Officify.Service.Host.csproj"
COPY . .

RUN dotnet build "src/Officify.Service.Host/Officify.Service.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Officify.Service.Host/Officify.Service.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS final

ENV AzureWebJobsScriptRoot="/home/site/wwwroot"
ENV AzureFunctionsJobHost__Logging__Console__IsEnabled="true"
ENV AzureWebJobsStorage="DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://azurite:10000/devstoreaccount1;QueueEndpoint=http://azurite:10001/devstoreaccount1;TableEndpoint=http://azurite:10002/devstoreaccount1;"
ENV FUNCTIONS_WORKER_RUNTIME="dotnet-isolated"
ENV AzurePersistence__StorageConnectionString="${AzureWebJobsStorage}"
ENV AzurePersistence__TableName="OfficifyEntities"

WORKDIR /app
COPY --from=publish ["/app/publish", "/home/site/wwwroot"]
