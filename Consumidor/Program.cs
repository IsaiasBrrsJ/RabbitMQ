
using RabbitMQ.Client;
using System.Text;

namespace Publicador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Servidor RabbitMq
            var servidor = new ConnectionFactory() 
            {   HostName = "localhost", 
                Port = 562, 
                UserName = "usuario", 
                Password = "Senha@123" 
            };

            //Conexao com o servidor
            var conexao = servidor.CreateConnection();
            {
                using(var canal = conexao.CreateModel())
                {
                    //Criar Fila ou escutar fila
                    canal.QueueDeclare(queue: "fila_de_tarefas",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    Console.WriteLine("Digite uma mensagem: ");

                    while (true)
                    {
                        string mensagem = Console.ReadLine();

                        if(mensagem is "1") break;

                        var corpoMensagem = Encoding.UTF8.GetBytes(mensagem);
                        var propriedades = canal.CreateBasicProperties();
                        propriedades.Persistent = true;

                        canal.BasicPublish(exchange: "",
                            routingKey: "fila_de_tarefas",
                            basicProperties: propriedades,
                            body: corpoMensagem);

                        Console.WriteLine(" [x] Enviou {0}", mensagem);


                    }

                  
                }
            }
        }
    }
}