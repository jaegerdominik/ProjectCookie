using Microsoft.EntityFrameworkCore;
using ProjectCookie.DAL.Entities;
using Serilog;

namespace ProjectCookie.DAL.UnitOfWork;

public class PostgresDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Score> Scores { get; set; }
    
    private readonly string _server;
    private readonly string _username;
    private readonly string _password;
    private readonly string _port;
    private readonly string _database;

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
    {
        //jdbc:postgresql://localhost:5433/cookie?password=pass&user=admin
        _server = "127.0.0.1";
        _username = "admin";
        _password = "pass";
        _port = "5433";
        _database = "cookie";
    }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (Log.Logger != null)
        {
            Log.Logger.Information("Creating Connection String for Host {Host}", _server);
        }
        
        string con = 
            "User ID=" + _username
            + ";Password=" + _password
            + ";Host=" + _server
            + ";Port=" + _port
            + ";Database=" + _database
            + ";CommandTimeout=120";
        
        optionsBuilder
            .UseNpgsql(con, b => b.CommandTimeout(120))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User", schema: "cookie");
        modelBuilder.Entity<Score>().ToTable("Score", schema: "cookie");
            
        if (Log.Logger != null)
        {
            Log.Logger.Information("Creating Model Finished");
        }
    }

    public async Task EnsureCreated()
    {
        if (Log.Logger != null)
        {
            Log.Logger.Information("Ensuring that model is created");
        }

        try
        {
            Database.Migrate();
            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            if (Log.Logger != null)
            {
                Log.Logger.Fatal(ex, "Could not create Database ");
            }
            else
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}