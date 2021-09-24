using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
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

            var reply = await client.LoginAsync(new LoginRequest
            {
                Email = email,
                Password = password,
            });

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {reply.Token}");
            
            var greeterClient = new FileUploader.FileUploaderClient(channel);
            using (var call = greeterClient.Upload(headers))
            {
                await using var fileStream =
                    new FileStream("C:\\Users\\User\\Desktop\\reshenie.txt", FileMode.OpenOrCreate);
                using var br = new BinaryReader(fileStream);

                var byteArray = br.ReadBytes((int)fileStream.Length);
                var btSize = byteArray.Length;
                var buffSize = 1024 * 1024; //1M
                var lastBiteSize = btSize % buffSize;
                var currentTimes = 0;
                var loopTimes = btSize / buffSize;

                await call.RequestStream.WriteAsync(new UploadFileRequest
                {
                    Metadata = new FileMetadata
                    {
                        Name = fileStream.Name,
                        Type = fileStream.Name.Split('.').Last()
                    },
                });

                while (currentTimes <= loopTimes)
                {
                    var sbytes = ByteString.CopyFrom(byteArray, currentTimes * buffSize, lastBiteSize);

                    var contentStruct = new UploadFileRequest
                    {
                        Chunk = new Chunk
                        {
                            Content = sbytes
                        }
                    };
                    await call.RequestStream.WriteAsync(contentStruct);
                    currentTimes++;
                }

                await call.RequestStream.CompleteAsync();
                await call;
            }
        }
    }
}