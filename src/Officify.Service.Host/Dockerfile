FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", ""]
COPY ["src/Officify.Service.Host/Officify.Service.Host.csproj", "src/Officify.Service.Host/"]
COPY ["src/Officify.Core/Officify.Core.csproj", "src/Officify.Core/"]
COPY ["src/Officify.Models/Officify.Models.csproj", "src/Officify.Models/"]
RUN dotnet restore "src/Officify.Service.Host/Officify.Service.Host.csproj"
COPY . .
WORKDIR "/src/src/Officify.Service.Host"
RUN dotnet build "Officify.Service.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Officify.Service.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/azure-functions/dotnet:4.0 AS final
WORKDIR /app
COPY --from=publish ["/app/publish", "/home/site/wwwroot"]
