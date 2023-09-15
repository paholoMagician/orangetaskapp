
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using orangebackend6.Models;
using orange.Controllers;
using orangebackend6;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(c => {
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.IgnoreObsoleteActions();
    c.IgnoreObsoleteProperties();
    c.CustomSchemaIds(type => type.FullName);
});

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json").Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<orangeContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
IServiceCollection serviceCollection = builder.Services.AddDbContext<orangeContext>(opt => opt.UseInMemoryDatabase(databaseName: "bin2"));

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
    options.MaximumReceiveMessageSize = 1024;
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200", "http://localhost:5078/", "http://localhost:60004", "*")
    .AllowCredentials());

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseAuthorization();

app.MapControllers();

//var jwtSettings = configuration.GetSection("JwtSettings");
//var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));

//app.UseAuthentication();
app.UseAuthorization();


//app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<ComentariosHub>("/hubs/ComentariosHub");
app.MapHub<tablaHub>("/hubs/tablaHub");

app.Run();
