using DatabaseConnectionLib;
using Core.Logic.Connections.RabbitMQ;
using GatewayServiceApi;
using MessageQueueConnectionLib;
using MonitoringConnectionLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient();
builder.Services.AddDatabaseConnectionLib();
builder.Services.AddMessageQueueConnectionLib();
builder.Services.AddRabbitMQServices();
builder.Services.AddServices();

builder.AddMonitoringMetrics();
builder.AddLogging();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMonitoringEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection(); с этой строчкой не работает prometheus

app.UseAuthorization();

app.MapControllers();

app.Run();
