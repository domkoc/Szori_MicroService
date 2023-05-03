using HelloWorldFrontend;
using HelloWorldMicroService;

var httpHandler = new HttpClientHandler();
httpHandler.ServerCertificateCustomValidationCallback =
HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc().AddJsonTranscoding();

var backendUrl = builder.Configuration.GetSection("HelloBackend")
    .GetValue<string>("Url");
builder.Services.AddGrpcClient<Hello.HelloClient>(o =>
{
    o.Address = new Uri(backendUrl);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback =
    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
    return handler;
});

var app = builder.Build();

app.UseStaticFiles();
app.MapGet("/", () => "Hello World Front-End!");
app.MapGrpcService<HelloFrontendService>();
app.Run();