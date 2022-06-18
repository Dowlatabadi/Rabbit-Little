using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Globalization;
using System.Diagnostics;
// See https://aka.ms/new-console-template for more information
var rate = .5;

if (Environment.GetCommandLineArgs().Length > 1)
{
    string string_rate = Environment.GetCommandLineArgs()[1];

    bool success = Double.TryParse(string_rate, out double number);
    if (success)
    {
        rate = number;
    }
    else
    {
        Console.WriteLine($"Set to default rate {rate}");

    }
}

var delay = (int)(1000 / rate);
var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(queue: "Q1",
                         durable: true,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);
    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var receive_time = DateTime.Now.ToString("HH:mm:ss");
                Thread.Sleep(delay);
                //processing
        Console.WriteLine("Sent at {0}, Received at {1}, and process finished at {2}", message, receive_time, DateTime.Now.ToString("HH:mm:ss"));

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    File.AppendAllText("dump",message+" "+receive_time+" "+ DateTime.Now.ToString("HH:mm:ss")+"\n");

    };
    channel.BasicConsume(queue: "Q1",
                         autoAck: false,
                         consumer: consumer);


    Console.WriteLine("Consumer Started.");
    Console.ReadLine();
}
