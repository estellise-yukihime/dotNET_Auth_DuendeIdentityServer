using main.Database.Storage.MainUser.Context;
using main.Database.Storage.MainUser.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var conStringDefault = configuration.GetConnectionString("DefaultConnection");
var conMigration = typeof(Program).Assembly.GetName().Name;

// Services

// Add Application Db Context
// Add MainUser Db Context
builder.Services.AddDbContext<MainUserDbContext>(x =>
{
    x.UseSqlServer(conStringDefault);
});

builder.Services.AddIdentity<MainUser, IdentityRole>()
    .AddEntityFrameworkStores<MainUserDbContext>()
    .AddDefaultTokenProviders();

// Add Identity Server
builder.Services.AddIdentityServer()
    .AddConfigurationStore(x =>
    {
        x.ConfigureDbContext = builderDbContext =>
        {
            builderDbContext.UseSqlServer(conStringDefault, c => c.MigrationsAssembly(conMigration));
        };
    })
    .AddOperationalStore(x =>
    {
        x.ConfigureDbContext = builderDbContext =>
        {
            builderDbContext.UseSqlServer(conStringDefault, c => c.MigrationsAssembly(conMigration));
        };
    })
    .AddAspNetIdentity<MainUser>();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

// Use IdentityServer
// - UseAuthentication is already added by IdentityServer
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllers();

app.Run();