FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api/PushNotificationApi.csproj", "Notification/PushNotificationApi/"]
RUN dotnet restore "Notification/PushNotificationApi/PushNotificationApi.csproj"

COPY ["Api/", "Notification/PushNotificationApi/"]
WORKDIR "/src/Notification/PushNotificationApi"

RUN dotnet build "PushNotificationApi.csproj" -c Release -o /app/build
RUN dotnet publish "PushNotificationApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "PushNotificationApi.dll"]