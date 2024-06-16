using Microsoft.EntityFrameworkCore;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.UnitOfWork;
using ProjectCookie.Services;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.MQTT;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddHostedService<MqttDriver>();
        });

var builder = WebApplication.CreateBuilder(args);

string _server = "127.0.0.1";
string _username = "admin";
string _password = "pass";
string _port = "5433";
string _database = "cookie";

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                          "Username=" + _username +
                          ";Password=" + _password +
                          ";Host=" + _server +
                          ";Port=" + _port +
                          ";Database=" + _database +
                          ";CommandTimeout=120";


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGlobalService, GlobalService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
