using System;
using System.Threading;
using RabbitMQ.Client;
using System.Text;
using System.Globalization;

Random rand = new Random();
double get_next_wait(double mean){
return mean*rand.NextDouble();
}
double get_next_exp_wait(double rate){
return -Math.Log(1-rand.NextDouble())/rate;
}
var rate = .5;
if (Environment.GetCommandLineArgs().Length > 1)
{
    string string_rate = Environment.GetCommandLineArgs()[1];
        Console.WriteLine($"{string_rate}");

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

var mean=2*((double)1 / rate);

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
    while (true)
    {
        //var delay = (int)(1000*get_next_wait(mean));//(1000 / rate);
        var delay = (int)(1000*get_next_exp_wait(rate));//(1000 / rate);
        
        Console.WriteLine($"delay: {delay}");

        Thread.Sleep(delay);
        string message = $"{DateTime.Now.ToString("HH:mm:ss")}";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "Q",
                             routingKey: "Q1",
                             basicProperties: null,
                             body: body);
        Console.WriteLine("Message Sent. Payload:[{0}]", message);
    }

}
Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
