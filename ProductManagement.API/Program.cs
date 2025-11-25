using Microsoft.EntityFrameworkCore;
using ProductManagement.Application.Events.Interfaces;
using ProductManagement.Application.Events.Services;
using ProductManagement.Application.RabbitMq.MessageHandlers;
using ProductManagement.Domain.Entities;
using ProductManagement.Infrastructure;
using ProductManagement.Infrastructure.Database;
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithOrigins("http://localhost:4200");
        });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
    var retry = 5;

    while (retry > 0)
    {
        try
        {
            db.Database.Migrate();
            break;
        }
        catch
        {
            retry--;
            Thread.Sleep(2000);
        }
    }
}

app.Run();
