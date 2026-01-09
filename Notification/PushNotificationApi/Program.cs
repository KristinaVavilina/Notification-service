using Core.Logic.Connections.RabbitMQ;
using MessageQueueConnectionLib;
using PushNotificationApi.Interfaces;
using PushNotificationApi.Listeners.RabbitMQ;
using PushNotificationApi.Services;
using MonitoringConnectionLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.AddMonitoringMetrics();
builder.AddLogging();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQServices();
builder.Services.AddMessageQueueConnectionLib();

builder.Services.AddScoped<IPushService, PushService>();
builder.Services.AddHostedService<NotificationRabbitMQListener>();

var app = builder.Build();

app.UseMonitoringEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
