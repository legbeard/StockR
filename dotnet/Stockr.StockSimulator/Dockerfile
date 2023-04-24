FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Stockr.StockSimulator/Stockr.StockSimulator.csproj", "Stockr.StockSimulator/"]
RUN dotnet restore "Stockr.StockSimulator/Stockr.StockSimulator.csproj"
COPY . .
WORKDIR "/src/Stockr.StockSimulator"
RUN dotnet build "Stockr.StockSimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stockr.StockSimulator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stockr.StockSimulator.dll"]
