using HelloWorldBackend;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGet("/", () => "Hello World Back-End!");
app.MapGrpcService<HelloBackendService>();

app.Run();
