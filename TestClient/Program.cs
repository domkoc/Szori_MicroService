using Grpc.Core;
using Grpc.Net.Client;
using Streaming;

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var channel = GrpcChannel.ForAddress("https://localhost:5000", new GrpcChannelOptions { HttpHandler = httpHandler });

var client = new MovieStream.MovieStreamClient(channel);

var reply = await client.GetMoviesAsync(new GetMoviesRequest());
Console.WriteLine(reply.Movies);

var reply2 = client.Watch(new WatchRequest() { StartPosition = 5, Title = "e" });

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
try
{
    await foreach (var watchReply in reply2.ResponseStream.ReadAllAsync(cancellationToken: cts.Token))
    {
        Console.WriteLine($"{watchReply.Frame}");
    }
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
{
    Console.WriteLine("Stream cancelled.");
}
Console.ReadKey();