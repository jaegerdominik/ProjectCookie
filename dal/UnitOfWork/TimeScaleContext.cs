using DAL.Entities;
using DAL.Entities.Devices;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Utilities.Logging;
using Utils;

namespace DAL.UnitOfWork;

public class TimeScaleContext : DbContext
{
    ILogger log;
    TimeScaleDBSettings Settings;
        
    public TimeScaleContext(ISettings settings, IAquariumLogger logger)
    {
        Settings = settings.TimeScaleDBSettings;
        log = logger.ContextLog<TimeScaleContext>("TimeScaleDB").ForContext("Host", Settings.Server, destructureObjects: true);

        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (log != null)
        {
            log.Debug("Creating Connection String for Host {Host}", Settings.Server);
        }
        String con = "User ID=" + Settings.Username + ";Password=" + Settings.Password + ";Host=" + Settings.Server + ";Port=" + Settings.Port + ";Database=" + Settings.DatabaseName + ";CommandTimeout=120";

        optionsBuilder.UseNpgsql(con, b => b.CommandTimeout(120)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    public DbSet<Device> Device { get; set; }
    public DbSet<MQTTDevice> MQTTDevice { get; set; }
    public DbSet<ModbusDevice> ModbusDevice { get; set; }
    public DbSet<DataPoint> DataPoint { get; set; }
    public DbSet<MQTTDataPoint> MQTTDataPoint { get; set; }
    public DbSet<ModbusDataPoint> ModbusDataPoint { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (log != null)
        {
            log.Debug("Creating Model");
        }
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Device>()
            .ToTable("Devices")
            .HasDiscriminator(b => b.DeviceType)
            .HasValue<MQTTDevice>("MQTT")
            .HasValue<ModbusDevice>("Modbus");


        modelBuilder.Entity<DataPoint>()
            .ToTable("DataPoints")
            .HasDiscriminator(b => b.DatapointType)
            .HasValue<MQTTDataPoint>("MQTT")
            .HasValue<ModbusDataPoint>("Modbus");

        modelBuilder.Entity<NumericSample>().ToTable("NumericSamples").HasKey(table => new { table.FK_Datapoint, table.Time, table.CreationDate }); ;
        modelBuilder.Entity<BinarySample>().ToTable("BinarySamples").HasKey(table => new { table.FK_Datapoint, table.Time, table.CreationDate }); ;



        if (log != null)
        {
            log.Debug("Creating Model Finished");
        }
        //   modelBuilder.HasDbFunction(
    }

    public async Task EnsureCreated()
    {

        if (log != null)
        {
            log.Debug("Ensuring that model is created");
        }
        try
        {
            Database.Migrate();

            Database.EnsureCreated();
            this.ApplyHypertables();

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