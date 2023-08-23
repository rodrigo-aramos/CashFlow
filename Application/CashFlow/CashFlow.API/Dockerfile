
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src

COPY ["CashFlow.Domain/CashFlow.Domain.csproj", "CashFlow.Domain/"]
COPY ["CashFlow.Infrastructure/CashFlow.Infrastructure.csproj", "CashFlow.Infrastructure/"]
COPY ["CashFlow.API/CashFlow.API.csproj", "CashFlow.API/"]

RUN dotnet restore "CashFlow.API/CashFlow.API.csproj"

COPY . .
WORKDIR "/src/CashFlow.API"
RUN dotnet build "CashFlow.API.csproj" -c Release -o /app/build

FROM build AS publish

RUN dotnet publish "CashFlow.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CashFlow.API.dll"]