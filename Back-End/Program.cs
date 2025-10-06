using Microsoft.EntityFrameworkCore;
using WeatherLens.Models;
using WeatherLens.Data.Repositories;
using WeatherLens.Data;
using WeatherLens.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Location>, LocationRepository>();
builder.Services.AddScoped<IRepository<WeatherQuery>, WeatherQueryRepository>();
builder.Services.AddScoped<IRepository<WeatherVariable>, WeatherVariableRepository>();
builder.Services.AddScoped<IRepository<WeatherQueryVariable>, WeatherQueryVariableRepository>();
builder.Services.AddScoped<IRepository<WeatherResult>, WeatherResultRepository>();

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
