FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api/MessangerNotificationApi.csproj", "Notification/MessangerNotificationApi/"]
RUN dotnet restore "Notification/MessangerNotificationApi/MessangerNotificationApi.csproj"

COPY ["Api/", "Notification/MessangerNotificationApi/"]
WORKDIR "/src/Notification/MessangerNotificationApi"

RUN dotnet build "MessangerNotificationApi.csproj" -c Release -o /app/build
RUN dotnet publish "MessangerNotificationApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "MessangerNotificationApi.dll"]