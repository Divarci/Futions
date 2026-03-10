# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Source/Adapter/Adapter.RestApi/Adapter.RestApi.csproj", "Api/Source/Adapter/Adapter.RestApi/"]
COPY ["Api/Source/Application/App.Services/App.Services.csproj", "Api/Source/Application/App.Services/"]
COPY ["Api/Source/Core/Core.Domain/Core.Domain.csproj", "Api/Source/Core/Core.Domain/"]
COPY ["Api/Source/Core/Core.Library/Core.Library.csproj", "Api/Source/Core/Core.Library/"]
COPY ["Api/Source/Application/App.UseCases/App.UseCases.csproj", "Api/Source/Application/App.UseCases/"]
COPY ["Api/Source/Infrastructure/Infra.Caching/Infra.Caching.csproj", "Api/Source/Infrastructure/Infra.Caching/"]
COPY ["Api/Source/Infrastructure/Infra.Persistence/Infra.Persistence.csproj", "Api/Source/Infrastructure/Infra.Persistence/"]
RUN dotnet restore "./Api/Source/Adapter/Adapter.RestApi/Adapter.RestApi.csproj"
COPY . .
WORKDIR "/src/Api/Source/Adapter/Adapter.RestApi"
RUN dotnet build "./Adapter.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Adapter.RestApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Adapter.RestApi.dll"]