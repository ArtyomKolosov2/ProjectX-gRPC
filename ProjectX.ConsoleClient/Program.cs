using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using ProjectX.Protobuf.Protos;
using ProjectX.Protobuf.Protos.Models;
using ProjectX.Protobuf.Protos.Services;

namespace ProjectX.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new UserAuthentication.UserAuthenticationClient(channel);
            var email = "test1@mail.com";
            var password = "123abcABC!";

            var reply = await client.LoginAsync(new LoginRequest()
            {
                Email = email,
                Password = password,
            });

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {reply.Token}");
            var greeterClient = new Greeter.GreeterClient(channel);

            var greeterResponse = await greeterClient.SayHelloAsync(new HelloRequest
            {
                Name = "testing auth call"
            }, headers);

            Console.WriteLine(greeterResponse.Message);
            Console.ReadKey();
        }
    }
}