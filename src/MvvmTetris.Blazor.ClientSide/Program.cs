using Microsoft.AspNetCore.Blazor.Hosting;



namespace MvvmTetris.Blazor.ClientSide
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
