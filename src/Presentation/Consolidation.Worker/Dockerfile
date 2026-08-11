FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore src/Presentation/Consolidation.Worker/Consolidation.Worker.csproj
RUN dotnet publish src/Presentation/Consolidation.Worker/Consolidation.Worker.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Consolidation.Worker.dll"]
