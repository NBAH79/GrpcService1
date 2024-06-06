using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcService1;
using GrpcGreeterStreamClient;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

// The port number must match the port of the gRPC server.
//var port=Console.ReadLine();
var connection = $"http://localhost:55000";
var connectionstream = $"http://localhost:55002";
var iterations = 1000;

Console.WriteLine("Press any key to start.");
Console.ReadKey();

//X509Certificate2 cert = new X509Certificate2("aspnetapp.pfx", "12345678");
//HttpClientHandler handler = new HttpClientHandler();
//handler.ClientCertificates.Add(cert);
//HttpClient httpClient = new HttpClient(handler);
//GrpcChannelOptions options = new GrpcChannelOptions() { HttpClient = httpClient };

//первый сервис
var channel = GrpcChannel.ForAddress(connection);//, options);
try
{
    {
        var client = new Greeter.GreeterClient(channel);
        var sw = new Stopwatch();
        sw.Start();
        HelloReply reply = new HelloReply();
        for (int i = 0; i < iterations; i++) { reply = await client.SayHelloAsync(new HelloRequest { Name = "GreeterClient" }); }
        sw.Stop();
        Console.WriteLine($"Greeting1W {reply.Message} done in {sw.ElapsedMilliseconds} ms");
    }

    {
        var client = new Greeter.GreeterClient(channel);
        var sw = new Stopwatch();
        sw.Start();
        ListenReply reply = new ListenReply();
        for (int i = 0; i < iterations; i++) { reply = await client.ListenHelloAsync(new ListenRequest { Filter = "*" }); }
        sw.Stop();
        Console.WriteLine($"Greeting1R {reply.Message} done in {sw.ElapsedMilliseconds} ms");
    }
    //Console.WriteLine("Greeting: " + reply.Message);
}
catch (Exception ex)
{
    Console.WriteLine($"Exception {ex.Message}");
}

//второй сервис
var channelstream = GrpcChannel.ForAddress(connectionstream);
try
{
    {
        //первый тест
        var client1 = new GreeterStream.GreeterStreamClient(channelstream);
        var sw1 = new Stopwatch();
        sw1.Start();
        HelloStreamReply reply = new HelloStreamReply();
        for (int i = 0; i < iterations; i++) { reply = await client1.SayHelloAsync(new HelloStreamRequest { Name = "GreeterClient" }); }
        sw1.Stop();
        Console.WriteLine($"Greeting2W {reply.Message} done in {sw1.ElapsedMilliseconds} ms");
    }
    {
        var client1 = new GreeterStream.GreeterStreamClient(channelstream);
        var sw1 = new Stopwatch();
        sw1.Start();
        ListenStreamReply reply = new ListenStreamReply();
        for (int i = 0; i < iterations; i++) { reply = await client1.ListenHelloAsync(new ListenStreamRequest { Filter = "{}" }); }
        sw1.Stop();
        Console.WriteLine($"Greeting2R {reply.Message} done in {sw1.ElapsedMilliseconds} ms");
    }
    //третий тест

    var client = new GreeterStream.GreeterStreamClient(channelstream);
    var sw = new Stopwatch();
    sw.Start();
    for (int i = 0; i < iterations; i++)
    {
        var reply = client.SayHelloStream(new HelloStreamRequestFile { File = "лютик.jpg" });


        var responseStream = reply.ResponseStream;

        await foreach (var response in responseStream.ReadAllAsync())
        {
            //Console.WriteLine(response.Content);
        }

    }
    sw.Stop();
    Console.WriteLine($"GreetingStream {iterations} done in {sw.ElapsedMilliseconds} ms");


}
catch (Exception ex)
{
    Console.WriteLine($"Exception {ex.Message}");
}
Console.WriteLine("Press any key to exit...");
Console.ReadKey();

