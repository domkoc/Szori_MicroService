using MovieStream_FIBRPN;
using Streaming;

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var backendUrls = builder.Configuration.GetSection("SingleMovies").GetSection("Url").Get<string[]>();
int i = 0;
foreach (var backendUrl in backendUrls)
{
    builder.Services.AddGrpcClient<SingleMovie.SingleMovieClient>($"SingleMovieClient{i}", o =>
    {
        o.Address = new Uri(backendUrl);
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        return handler;
    });
    i++;
}

var app = builder.Build();

app.MapGrpcService<MovieStreamService>();
app.Run();