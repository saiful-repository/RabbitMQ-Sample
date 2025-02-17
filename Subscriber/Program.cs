using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.Common;
using System.Text;

// Create a new RabbitMQ connection factory and set the hostname to localhost
var factory = new ConnectionFactory
{
    HostName = "localhost"
};

// Establish an connection to the RabbitMQ server
var connection = await factory.CreateConnectionAsync();

// Create a new channel for communication with RabbitMQ
using var channel = await connection.CreateChannelAsync();

// Declare an exchange of type "fanout"
await channel.ExchangeDeclareAsync(exchange: "multi_log_msg", type: ExchangeType.Fanout, durable: true, autoDelete: false);

// Declare a queue named "queue_console"
await channel.QueueDeclareAsync(queue: "queue_console", durable: true, exclusive: false, autoDelete: false);

//Bind the Queue with Exchange
await channel.QueueBindAsync(queue: "queue_console", exchange: "multi_log_msg", routingKey: "");

// Create a new consumer to receive messages from the queue
var consumer = new AsyncEventingBasicConsumer(channel);

// Define the event handler for processing received messages
consumer.ReceivedAsync += (sender, args) =>
{
    // Convert the message body from a byte array to a string
    var body = args.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);

    // Print the received message to the console
    Console.WriteLine($"Message Receive: { message}");

    // Return a completed task
    return Task.CompletedTask;
};

// Start consuming messages from the "info" queue with automatic acknowledgment
await channel.BasicConsumeAsync(queue: "queue_console", autoAck:true, consumer: consumer);

Console.ReadKey();


