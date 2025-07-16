using GreeterGrpcServices.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

// Enable HTTP/2 without TLS
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to use HTTP/2 without TLS
builder.WebHost.ConfigureKestrel(options =>
{
    // Force HTTP/2 protocol
    options.ListenAnyIP(8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

// Add services
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Log all addresses
var addresses = app.Urls;
foreach (var address in addresses)
{
    Console.WriteLine($"Listening on {address}");
}

// Configure endpoints
app.MapGrpcService<GreeterService>();
app.MapGrpcReflectionService();

// Log that the application is ready
Console.WriteLine("Application started and ready to receive requests");

app.Run();