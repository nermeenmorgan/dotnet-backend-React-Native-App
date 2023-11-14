using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;

using Serilog;
using Serilog.Events;

namespace Acme.BookStore.DbMigrator;

class Program
{
    //   public class AppDbContext : DbContext
    // {
    //     // DbSet properties for your entities go here

    //     public DbSet<Book> Books { get; set; }
    //     // Add other DbSet properties as needed

    //     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    //     {
    //     }
    // }
    
 static void Main(string[] args)
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
        .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
#if DEBUG
            .MinimumLevel.Override("Acme.BookStore", LogEventLevel.Debug)
#else
            .MinimumLevel.Override("Acme.BookStore", LogEventLevel.Information)
#endif
            .Enrich.FromLogContext()
        .WriteTo.Async(c => c.File("Logs/logs.txt"))
        .WriteTo.Async(c => c.Console())
        .CreateLogger();

    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .AddAppSettingsSecretsJson()
            .ConfigureLogging((context, logging) => logging.ClearProviders())
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<DbMigratorHostedService>();
            });
}