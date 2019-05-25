using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;



namespace MvvmTetris.Blazor.ServerSide
{
    public class Program
    {
        public static Task Main(string[] args)
            => Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .Build()
            .RunAsync();
    }
}
