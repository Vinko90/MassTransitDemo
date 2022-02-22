FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MTBlazorApp/MTBlazorApp.csproj", "MTBlazorApp/"]
COPY ["MTCore/MTCore.csproj", "MTCore/"]
RUN dotnet restore "MTBlazorApp/MTBlazorApp.csproj"
COPY . .
WORKDIR "/src/MTBlazorApp"
RUN dotnet build "MTBlazorApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MTBlazorApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MTBlazorApp.dll"]
