using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Notes_API_DAL.Context
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(),"..","Notes_API"); //usar Notes_API_WEB si falla
            var configuration = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json").Build();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            Console.WriteLine(configuration);
            Console.WriteLine(optionsBuilder.IsConfigured);
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
