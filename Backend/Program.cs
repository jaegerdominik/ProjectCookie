using Microsoft.EntityFrameworkCore;
using ProjectCookie.DAL.BaseInterfaces;
using ProjectCookie.DAL.UnitOfWork;
using ProjectCookie.Services;
using ProjectCookie.Services.BaseInterfaces;
using ProjectCookie.Services.MQTT;
using ProjectCookie.Utils;
using ProjectCookie.Utils.Logging;

var builder = WebApplication.CreateBuilder(args);

string _server = "127.0.0.1";
string _username = "admin";
string _password = "pass";
string _port = "5433";
string _database = "cookie";

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
                          $"Username={_username};Password={_password};Host={_server};Port={_port};Database={_database};CommandTimeout=120";

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<PostgresDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IGlobalService, GlobalService>();
builder.Services.AddScoped<ICookieLogger, CookieLogger>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the MqttDriver hosted service
builder.Services.AddSingleton<MqttDriver>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<MqttDriver>());

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