FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MTShippingService/MTShippingService.csproj", "MTShippingService/"]
COPY ["MTCore/MTCore.csproj", "MTCore/"]
RUN dotnet restore "MTShippingService/MTShippingService.csproj"
COPY . .
WORKDIR "/src/MTShippingService"
RUN dotnet build "MTShippingService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MTShippingService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MTShippingService.dll"]
