using MessageQueueConnectionLib.Interfaces;
using MessageQueueConnectionLib.Rabbit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(sp =>
    new RabbitMqConnectionFactory(
        host: builder.Configuration["Rabbit:Host"] ?? "localhost",
        user: builder.Configuration["Rabbit:User"] ?? "guest",
        pass: builder.Configuration["Rabbit:Password"] ?? "guest"
    ));

builder.Services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
builder.Services.AddSingleton<IMessageConsumer, RabbitMqConsumer>();
builder.Services.AddHostedService<EmailConsumerService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
