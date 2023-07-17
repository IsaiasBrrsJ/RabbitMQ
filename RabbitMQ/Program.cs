using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQ
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var servidor = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 562,
                UserName = "usuario",
                Password = "Senha@123",
            };

            var con = servidor.CreateConnection();
            {
                using (var canal = con.CreateModel())
                {
                    canal.QueueDeclare(queue: "fila_de_tarefas",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    canal.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    Console.WriteLine("[*] Aguardando mensagens.");

                    var consumidor = new EventingBasicConsumer(canal);

                    consumidor.Received += (model, ea) =>
                    { 
                        var corpo = ea.Body.ToArray();

                        var mensagem = Encoding.UTF8.GetString(corpo);

                        Console.WriteLine(" [x] Recebido {0}", mensagem);

                        canal.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    canal.BasicConsume(queue: "fila_de_tarefas",
                        autoAck: false,
                        consumer: consumidor);

                    Console.WriteLine("Pressione [Enter] para finalizar");
                    Console.ReadKey();
                }
            }
        }
    }
}