FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api/GatewayServiceApi.csproj", "GatewayServiceApi/"]
RUN dotnet restore "GatewayServiceApi/GatewayServiceApi.csproj"

COPY ["Api/", "GatewayServiceApi/"]
WORKDIR "/src/GatewayServiceApi"

RUN dotnet build "GatewayServiceApi.csproj" -c Release -o /app/build
RUN dotnet publish "GatewayServiceApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "GatewayServiceApi.dll"]