using backend.Models;
using backend.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword}";

builder.Services.AddDbContext<MyContext>(options => options.UseSqlServer(connectionString));
//builder.Services.AddScoped<IRepositoryBase, RepositoryBase>();
//Esto se usa para sacar el string connection de appsettings.json
//builder.Services.AddDbContext<MyContext>(options => 
//options.UseSqlServer(builder.Configuration.GetConnectionString("MyDBConnectionNet6"))); 

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

	try
	{
		var context = services.GetRequiredService<MyContext>();
	}
	catch (Exception ex)
	{
		var logger = services.GetRequiredService<ILogger<Program>>();
		logger.LogError(ex, "Ha ocurrido un error al enviar la info a la base de datos.");
	}
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//ejecuta las migraciones pendientes
PrepareDb.Population(app);

app.UseAuthorization();

app.MapControllers();

app.Run();
