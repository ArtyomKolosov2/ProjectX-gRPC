using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ProjectX.Protobuf.Protos;

namespace ProjectX.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            Console.Write("Введите имя: ");
            var name = Console.ReadLine();
            var reply = await client.SayHelloAsync(new HelloRequest { Name = name });
            Console.WriteLine("Ответ сервера: " + reply.Message);
            Console.ReadKey();
        }
    }
}