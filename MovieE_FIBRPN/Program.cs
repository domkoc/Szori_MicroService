using MovieE_FIBRPN;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<MovieEService>();

app.Run();
