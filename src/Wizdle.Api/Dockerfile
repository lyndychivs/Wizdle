FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props .
COPY Directory.Build.targets .
COPY Directory.Packages.props .
COPY global.json .

COPY resources/logo.png resources/

COPY src/Wizdle.Models/ src/Wizdle.Models/
COPY src/Wizdle/ src/Wizdle/
COPY src/Wizdle.ServiceDefaults/ src/Wizdle.ServiceDefaults/
COPY src/Wizdle.Api/ src/Wizdle.Api/

RUN dotnet publish src/Wizdle.Api/Wizdle.Api.csproj \
    --configuration Release \
    --output /app/publish \
    -p:GeneratePackageOnBuild=false \
    -p:EnforceCodeStyleInBuild=false \
    -p:RunAnalyzers=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Wizdle.Api.dll"]
