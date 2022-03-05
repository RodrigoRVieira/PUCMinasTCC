#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM rrvieira/core-sdk-selenium:1.0 AS build

USER seluser

WORKDIR /src

COPY --chown=seluser ["Tests/IntegrationTests/**", "Tests/IntegrationTests/"]
COPY --chown=seluser ["GISA.Domain/**", "GISA.Domain/"]
COPY --chown=seluser ["SAF.Repository/**", "SAF.Repository/"]

RUN dotnet restore "Tests/IntegrationTests/IntegrationTests.csproj"