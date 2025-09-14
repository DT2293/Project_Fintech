#  build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY src/FintechApp.Application/FintechApp.Application.csproj FintechApp.Application/
COPY src/FintechApp.Infrastructure/FintechApp.Infrastructure.csproj FintechApp.Infrastructure/
COPY src/FintechApp.Presentation/FintechApp.Presentation.csproj FintechApp.Presentation/
RUN dotnet restore src/FintechApp.Presentation/FintechApp.Presentation.csproj

# Copy and build
COPY src/FintechApp.Application/ FintechApp.Application/
COPY src/FintechApp.Infrastructure/ FintechApp.Infrastructure/
COPY src/FintechApp.Presentation/ FintechApp.Presentation/
WORKDIR /src/src/FintechApp.Presentation
RUN dotnet publish -c Release -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "FintechApp.Presentation.dll"]

# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
# WORKDIR /app
# ENV ASPNETCORE_URLS=http://+:80
# COPY src/FintechApp.Presentation/bin/Release/net9.0/publish/ .
# COPY src/FintechApp.Presentation/bin/Release/net9.0/publish/FintechApp.Presentation.xml .

# ENTRYPOINT ["dotnet", "FintechApp.Presentation.dll"]
