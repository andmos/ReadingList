using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace ReadingList.Carter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).UseLightInject(registry => registry.RegisterFrom<CompositionRoot>())
            .Build()
            .Run();
        }   
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel();
                webBuilder.UseStartup<Startup>();
            });
    }
}
