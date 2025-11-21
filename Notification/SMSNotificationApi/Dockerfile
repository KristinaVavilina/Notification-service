FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api/SMSNotificationApi.csproj", "Notification/SMSNotificationApi/"]
RUN dotnet restore "Notification/SMSNotificationApi/SMSNotificationApi.csproj"

COPY ["Api/", "Notification/SMSNotificationApi/"]
WORKDIR "/src/Notification/SMSNotificationApi"

RUN dotnet build "SMSNotificationApi.csproj" -c Release -o /app/build
RUN dotnet publish "SMSNotificationApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "SMSNotificationApi.dll"]