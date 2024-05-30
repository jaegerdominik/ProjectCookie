using DAL.Entities;
using DAL.Entities.Devices;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace ProjectCookie._src.dal.UnitOfWork
{
    public class PostgresDbContext : DbContext
    {
        ILogger log;
        public DbSet<Device> Device { get; set; }
        public DbSet<MQTTDevice> MQTTDevice { get; set; }
        public DbSet<DataPoint> DataPoint { get; set; }
        public DbSet<MQTTDataPoint> MQTTDataPoint { get; set; }

        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           /** if (log != null)
            {
                log.Debug("Creating Connection String for Host {Host}", Settings.Server);
            }
            String con = "User ID=" + Settings.Username + ";Password=" + Settings.Password + ";Host=" + Settings.Server + ";Port=" + Settings.Port + ";Database=" + Settings.DatabaseName + ";CommandTimeout=120";

            optionsBuilder.UseNpgsql(con, b => b.CommandTimeout(120)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); **/
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /**modelBuilder.Entity<TestTable>().ToTable("Test_table", schema: "test_schema");
            modelBuilder.Entity<Stats>().ToTable("Stats", schema: "academicar");
            modelBuilder.Entity<User>().ToTable("User", schema: "academicar");
            modelBuilder.Entity<FavoriteUser>().ToTable("FavoriteUser", schema: "academicar")
                .HasOne(f => f.FavUser)
                .WithMany()
                .HasForeignKey("UserId")
                .IsRequired();
            modelBuilder.Entity<Preferences>().ToTable("Preferences", schema: "academicar");
            modelBuilder.Entity<Rating>().ToTable("Rating", schema: "academicar");
            modelBuilder.Entity<Vehicle>().ToTable("Vehicle", schema: "academicar");
            modelBuilder.Entity<Trip>().ToTable("Trip", schema: "academicar");
            modelBuilder.Entity<TripRequest>().ToTable("TripRequest", schema: "academicar");
            modelBuilder.Entity<Address>().ToTable("Adress", schema: "academicar"); **/
            
            if (log != null)
            {
                log.Debug("Creating Model Finished");
            }
        }


        #region DB Tables

        /**
        public DbSet<TestTable> TestTableEntries { get; set; }
        public DbSet<Stats> Stats { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<FavoriteUser> FavoriteUsers { get; set; }
        public DbSet<Preferences> Preferences { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripRequest> TripRequests { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }**/

        #endregion


        public async Task EnsureCreated()
        {

            if (log != null)
            {
                log.Debug("Ensuring that model is created");
            }

            try
            {
                //Database.Migrate();

                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                if (log != null)
                {

                    log.Fatal(ex, "Could not create Database ");
                }
                else
                {
                    Console.Error.WriteLine(ex.ToString());
                }
            }
        }
    }
}
