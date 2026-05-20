FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Build.props .
COPY Directory.Build.targets .
COPY Directory.Packages.props .
COPY global.json .

COPY Resources/logo.png Resources/

COPY Wizdle.Models/ Wizdle.Models/
COPY Wizdle/ Wizdle/
COPY Wizdle.ServiceDefaults/ Wizdle.ServiceDefaults/
COPY Wizdle.Api/ Wizdle.Api/

RUN dotnet publish Wizdle.Api/Wizdle.Api.csproj \
    --configuration Release \
    --output /app/publish \
    -p:GeneratePackageOnBuild=false \
    -p:EnforceCodeStyleInBuild=false \
    -p:RunAnalyzers=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Wizdle.Api.dll"]
