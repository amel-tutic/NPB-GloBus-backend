using GloBus.Data;
using GloBus.Infrastructure;
using GloBus.Infrastructure.Interfaces;
using GloBus.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//context
builder.Services.AddDbContext<GloBusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

//repositories DI
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//automapper config
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
