using WebApiSubscriber.RabbitMQ;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Register RabbitMQConnection
var rabbitMQConnection = await RabbitMQConnection.CreateAsync();
builder.Services.AddSingleton<IRabbitMQConnection>(rabbitMQConnection);

//Register MessageConsumer
builder.Services.AddSingleton<IMessageConsumer, MessageConsumer>();

// Register the RabbitMQ Consumer service as a background service
builder.Services.AddHostedService<RabbitMQConsumerService>();

var app = builder.Build();

Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
 .WriteTo.File("logs/Message-.txt", rollingInterval: RollingInterval.Day)
 .CreateLogger();

// Configure the HTTP request pipeline.

app.Run();


