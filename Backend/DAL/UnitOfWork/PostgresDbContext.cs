using Microsoft.EntityFrameworkCore;
using ProjectCookie.DAL.Entities;
using ILogger = Serilog.ILogger;

namespace ProjectCookie.DAL.UnitOfWork;

public class PostgresDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Score> Scores { get; set; }
    
    private readonly ILogger _log;
    private readonly string _server;
    private readonly string _username;
    private readonly string _password;
    private readonly string _port;
    private readonly string _database;

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options, ILogger log) : base(options)
    {
        _log = log;
        
        _server = "127.0.0.1";
        _username = "admin";
        _password = "pass";
        _port = "27017";
        _database = "AquariumManagement";
    }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (_log != null)
        {
            _log.Debug("Creating Connection String for Host {Host}", _server);
        }
        String con = "User ID=" + _username + ";Password=" + _password + ";Host=" + _server + ";Port=" + _port + ";Database=" + _database + ";CommandTimeout=120";
        optionsBuilder.UseNpgsql(con, b => b.CommandTimeout(120)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("User", schema: "cookie");
        modelBuilder.Entity<Score>().ToTable("Score", schema: "cookie");
            
        if (_log != null)
        {
            _log.Debug("Creating Model Finished");
        }
    }

    public async Task EnsureCreated()
    {
        if (_log != null)
        {
            _log.Debug("Ensuring that model is created");
        }

        try
        {
            Database.Migrate();
            Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            if (_log != null)
            {
                _log.Fatal(ex, "Could not create Database ");
            }
            else
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}