using Core.Logic.Connections.RabbitMQ;
using MessageQueueConnectionLib;
using MessangerNotificationApi.Interfaces;
using MessangerNotificationApi.Listeners.RabbitMQ;
using MessangerNotificationApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQServices();
builder.Services.AddMessageQueueConnectionLib();

builder.Services.AddHttpClient<IMessangerService, MessangerService>();
builder.Services.AddHostedService<NotificationRabbitMQListener>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
