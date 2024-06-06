using Grpc.Core;
using GrpcService1;
using Microsoft.EntityFrameworkCore;
using Service1.Model;

namespace GrpcService1.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly ApplicationContext db;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
            try
            {
                db = new ApplicationContext();
            }
            catch (Exception e) { Console.WriteLine($"Exception: {e.Message}"); }
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            //using (ApplicationContext db = new ApplicationContext())
            //{
            try
            {
                db.Append(new Model1 { Name = request.Name, Description = request.Name });
                //db.ShowAll();
            }
            catch (Exception e) { Console.WriteLine($"Exception: {e.Message}"); }
            //}

            return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
        }

        public override Task<ListenReply> ListenHello(ListenRequest filter, ServerCallContext context)
        {
            List<Model1> l=new List<Model1>();
            //using (ApplicationContext db = new ApplicationContext())
            //{
            try
            {
                l=db.Models1.ToList(); 
            }
            catch (Exception e) { Console.WriteLine($"Exception: {e.Message}"); }
            //}

            return Task.FromResult(new ListenReply { Message = $"Done {l.Count}"});
        }

        public class ApplicationContext : DbContext
        {
            public DbSet<Service1.Model.Model1> Models1 { get; set; } = null!;

            public ApplicationContext()
            {
                Database.EnsureCreated();
            }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseNpgsql("Host=postgres;Port=5432;Database=usersdb;Username=postgres;Password=11223344;");
                //optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            }

            public void ShowAll()
            {
                // получаем объекты из бд и выводим на консоль
                ////var users = this.Models1.ToList();
                ////Console.WriteLine("Users list:");
                ////foreach (Model1 u in users)
                ////{
                ////    Console.WriteLine($"{u.Id}.{u.Name} - {u.Description}");
                ////}
            }
            public void Append(Model1 model)
            {
                // добавляем их в бд
                this.Models1.Add(model);
                this.SaveChanges();
            }
        }
    }
}
