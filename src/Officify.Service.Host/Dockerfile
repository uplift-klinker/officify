FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", ""]
COPY ["src/Officify.Service.Host/Officify.Service.Host.csproj", "src/Officify.Service.Host/"]
COPY ["src/Officify.Core/Officify.Core.csproj", "src/Officify.Core/"]
COPY ["src/Officify.Models/Officify.Models.csproj", "src/Officify.Models/"]
RUN dotnet restore "src/Officify.Service.Host/Officify.Service.Host.csproj"
COPY . .

RUN dotnet build "src/Officify.Service.Host.csproj" --no-restore -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/Officify.Service.Host.csproj" --no-restore -c $BUILD_CONFIGURATION -o /app/publish -r linux-arm64 /p:UseAppHost=false

FROM mcr.microsoft.com/azure-functions/dotnet:4.0 AS final
WORKDIR /app
COPY --from=publish ["/app/publish", "/home/site/wwwroot"]
