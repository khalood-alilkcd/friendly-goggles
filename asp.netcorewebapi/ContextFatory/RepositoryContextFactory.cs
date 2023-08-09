using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace asp.netcorewebapi.ContextFatory
{
    public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
    {
        public RepositoryContext CreateDbContext(string[] args)
        {
            var configure = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
            var builder = new DbContextOptionsBuilder<RepositoryContext>().UseSqlServer(configure.GetConnectionString("sqlConnections"),e => e.MigrationsAssembly("asp.netcorewebapi"));
            return new RepositoryContext(builder.Options);
        }
    }
}
