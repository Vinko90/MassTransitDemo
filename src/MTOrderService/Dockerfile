FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
#EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MTOrderService/MTOrderService.csproj", "MTOrderService/"]
COPY ["MTCore/MTCore.csproj", "MTCore/"]
RUN dotnet restore "MTOrderService/MTOrderService.csproj"
COPY . .
WORKDIR "/src/MTOrderService"
RUN dotnet build "MTOrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MTOrderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MTOrderService.dll"]
