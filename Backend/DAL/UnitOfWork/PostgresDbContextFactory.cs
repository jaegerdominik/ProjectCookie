using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectCookie.DAL.UnitOfWork;

public class PostgresDbContextFactory : IDesignTimeDbContextFactory<PostgresDbContext>
{
    public PostgresDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();
        optionsBuilder.UseNpgsql("User ID=admin;Password=pass;Host=127.0.0.1;Port=5433;Database=cookie;CommandTimeout=120");

        return new PostgresDbContext(optionsBuilder.Options);
    }
}
