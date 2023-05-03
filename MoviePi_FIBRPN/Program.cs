using MoviePi_FIBRPN;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<MoviePiService>();

app.Run();
