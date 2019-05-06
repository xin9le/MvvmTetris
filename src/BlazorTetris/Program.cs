using Microsoft.AspNetCore.Blazor.Hosting;



namespace BlazorTetris
{
    public class Program
    {
        public static void Main(string[] args)
            => BlazorWebAssemblyHost.CreateDefaultBuilder()
            .UseBlazorStartup<Startup>()
            .Build()
            .Run();
    }
}
