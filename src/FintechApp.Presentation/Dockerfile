# build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY src/FintechApp.Domain/FintechApp.Domain.csproj FintechApp.Domain/
COPY src/FintechApp.Application/FintechApp.Application.csproj FintechApp.Application/
COPY src/FintechApp.Infrastructure/FintechApp.Infrastructure.csproj FintechApp.Infrastructure/
COPY src/FintechApp.Presentation/FintechApp.Presentation.csproj FintechApp.Presentation/

RUN dotnet restore FintechApp.Presentation/FintechApp.Presentation.csproj

# Copy all source code
COPY src/FintechApp.Domain/ FintechApp.Domain/
COPY src/FintechApp.Application/ FintechApp.Application/
COPY src/FintechApp.Infrastructure/ FintechApp.Infrastructure/
COPY src/FintechApp.Presentation/ FintechApp.Presentation/

# Publish app (includes XML docs if enabled in .csproj)
WORKDIR /src/FintechApp.Presentation
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Env config
ENV ASPNETCORE_URLS=http://+:80

# Copy publish output (including XML docs)
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FintechApp.Presentation.dll"]


