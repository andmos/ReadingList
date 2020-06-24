using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
namespace ReadingList.Carter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .UseLightInject()
                .Build();

            host.Run();
        }
    }
}
