using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Application.RabbitMq.MessageHandlers;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure;
using ProductManagement.Infrastructure.RabbitMQ.Interfaces;
using ProductManagement.Infrastructure.RabbitMQ.Services;
using ProductManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.Configure<RabbitMqSettings>(
    builder.Configuration.GetSection("RabbitMq"));

builder.Services.AddSingleton<IRabbitMqConnection, RabbitMqConnection>();
builder.Services.AddSingleton<IRabbitMqProducer, RabbitMqProducer>();
builder.Services.AddSingleton(typeof(IRabbitMqConsumer<>), typeof(RabbitMqConsumer<>));

builder.Services.AddScoped<IProductEventService, ProductEventService>();
builder.Services.AddScoped<IMessageHandler<Product>, CreatedProductHandler>();

builder.Services.AddHostedService<ProductCreatedConsumerHostedService>();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();
