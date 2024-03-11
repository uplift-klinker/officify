FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ENV NODE_VERSION=20.10.0
ENV NVM_VERSION=0.39.7
ENV NVM_DIR=/root/.nvm

WORKDIR /src

RUN apt install -y curl
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v${NVM_VERSION}/install.sh | bash
RUN . "$NVM_DIR/nvm.sh" && nvm install ${NODE_VERSION}
RUN . "$NVM_DIR/nvm.sh" && nvm use v${NODE_VERSION}
RUN . "$NVM_DIR/nvm.sh" && nvm alias default v${NODE_VERSION}
ENV PATH="/root/.nvm/versions/node/v${NODE_VERSION}/bin/:${PATH}"

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
