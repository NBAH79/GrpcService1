using Google.Protobuf;
using Grpc.Core;
using GrpcService2;
using MongoDB.Bson;
using MongoDB.Driver;
using System.IO;

namespace GrpcService2.Services
{
    public class GreeterService : GreeterStream.GreeterStreamBase
    {
        private readonly ILogger<GreeterService> _logger;
        private MongoClient client;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
            try
            {
                client = new MongoClient("mongodb://mongo1:27017,mongo2:27018");
            }
            catch (Exception e) { Console.WriteLine($"Exception: {e.Message}"); }
        }

        public override Task<HelloStreamReply> SayHello(HelloStreamRequest request, ServerCallContext context)
        {
            try {
            var db = client.GetDatabase("dbusers");
            var collection = db.GetCollection<BsonDocument>("Models1");

            // добавляем в коллекцию users документ
            collection.InsertOne(new Service2.Model.Model1{ Name="Hello World!", Description="Hello World!" }.ToBsonDocument());
            }
            catch (Exception e) { Console.WriteLine($"Exception: {e.Message}"); }
            return Task.FromResult(new HelloStreamReply { Message = "Hello " + request.Name });
        }

        public override async Task<ListenStreamReply> ListenHello(ListenStreamRequest filter, ServerCallContext context)
        {
            List<BsonDocument> l=new List<BsonDocument>();
            try
            {
                var db = client.GetDatabase("dbusers");
                var collection = db.GetCollection<BsonDocument>("Models1");

                l= await collection.Find(filter.Filter).ToListAsync();
            }
            catch (Exception e) { Console.WriteLine($"Exception: {e.Message}"); }
            return new ListenStreamReply { Message = $"Quantity {l.Count}" };
        }

        public override async Task SayHelloStream(HelloStreamRequestFile request, IServerStreamWriter<HelloStreamResponseFile> responseStream, ServerCallContext context)
        {
            //foreach (var message in messages)
            //{
            using FileStream fileStream=new FileStream(request.File,FileMode.Open);
                using StreamReader sr=new StreamReader(fileStream);
                await responseStream.WriteAsync(new HelloStreamResponseFile { Content = await sr.ReadToEndAsync() });
                // для имитации работы делаем задержку в 1 секунду
                //await Task.Delay(TimeSpan.FromSeconds(1));
            //}
        }
    }
}
