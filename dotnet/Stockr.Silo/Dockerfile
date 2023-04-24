FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Stockr.Silo/Stockr.Silo.csproj", "Stockr.Silo/"]
COPY ["Stockr.Core/Stockr.Core.csproj", "Stockr.Core/"]
RUN dotnet restore "Stockr.Silo/Stockr.Silo.csproj"
COPY . .
WORKDIR "/src/Stockr.Silo"
RUN dotnet build "Stockr.Silo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stockr.Silo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Stockr.Silo.dll"]
