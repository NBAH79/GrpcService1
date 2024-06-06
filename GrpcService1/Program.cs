using Grpc.Core;
using GrpcService1.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

#if (DEBUG)
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ConfigureHttpsDefaults(o =>
//    {
//        //o.ClientCertificateValidation += ValidateClientCertificate;
//        o.AllowAnyClientCertificate();
//        o.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
//        o.CheckCertificateRevocation = false;
//    });
//    options.ConfigureEndpointDefaults(options =>
//    {
//        options.Protocols = HttpProtocols.Http2;
//        options.UseHttps();
//    });
//});

//WebHost.CreateDefaultBuilder(args)
//    .ConfigureKestrel(options =>
//    {
//        options.Listen(IPAddress.Any, ports.httpPort, listenOptions =>
//        {
//            listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
//        });
//        options.Listen(IPAddress.Any, ports.grpcPort, listenOptions =>
//        {
//            listenOptions.Protocols = HttpProtocols.Http2;
//        });

//    })

#endif

//ConfigureWebHostDefaults(webBuilder =>
//{
//    webBuilder.UseStartup()

//    .ConfigureKestrel(options =>
//    {
//        options.Listen(IPAddress.Any, 5001, listenOptions =>
//        {
//            listenOptions.Protocols = HttpProtocols.Http1;
//        });
//    });
//});


// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();

IWebHostEnvironment env = app.Environment;
if (env.IsDevelopment())
{
    app.MapGrpcReflectionService();
}
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
