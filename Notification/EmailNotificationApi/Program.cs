using Core.Logic.Connections.RabbitMQ;
using EmailNotificationApi.Interfaces;
using EmailNotificationApi.Listeners.RabbitMQ;
using EmailNotificationApi.Services;
using MessageQueueConnectionLib;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRabbitMQServices();
builder.Services.AddMessageQueueConnectionLib();
// Регистрация сервиса отправки почты
builder.Services.AddTransient<IEmailService, SmtpEmailService>();
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
