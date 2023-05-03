using Grpc.Net.Client;
using HelloWorldMicroService;
using System.Numerics;

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:5101", new GrpcChannelOptions { HttpHandler = httpHandler });
var client = new Hello.HelloClient(channel);
var reply = await client.SayHelloAsync(new SayHelloRequest() { Name = "me " });
Console.WriteLine(reply.Message);

Console.ReadKey();
