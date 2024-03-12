FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY ["Directory.Build.props", "src/"]
COPY ["src/Officify.Api.Client/Officify.Api.Client.csproj", "src/Officify.Api.Client/"]
COPY ["src/Officify.Web.Host/Officify.Web.Host.csproj", "src/Officify.Web.Host/"]
COPY ["src/Officify.Models/Officify.Models.csproj", "src/Officify.Models/"]
RUN dotnet restore "src/Officify.Web.Host/Officify.Web.Host.csproj"

COPY . .

RUN dotnet build "src/Officify.Web.Host/Officify.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "src/Officify.Web.Host/Officify.Web.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
COPY ["src/Officify.Web.Host/nginx.conf", "/etc/nginx/nginx.conf"]
COPY --from=publish /app/publish/wwwroot /app/publish
