FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Api/EmailNotificationApi.csproj", "Notification/EmailNotificationApi/"]
RUN dotnet restore "Notification/EmailNotificationApi/EmailNotificationApi.csproj"

COPY ["Api/", "Notification/EmailNotificationApi/"]
WORKDIR "/src/Notification/EmailNotificationApi"

RUN dotnet build "EmailNotificationApi.csproj" -c Release -o /app/build
RUN dotnet publish "EmailNotificationApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "EmailNotificationApi.dll"]