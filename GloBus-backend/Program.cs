using GloBus.Data;
using GloBus.Infrastructure;
using GloBus.Infrastructure.CustomMiddlewares;
using GloBus.Infrastructure.Interfaces;
using GloBus.Infrastructure.Repositories;
using GloBus_backend.BackgroundJobs.CheckForInvalidTickets;
using GloBus_backend.BackgroundJobs.DeleteObsoleteTickets;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//database context
builder.Services.AddDbContext<GloBusContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//cors policy config to allow cross-origin request
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            //.WithOrigins("http://localhost:5093")
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders("Authorization");
        });
});

//jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        RequireExpirationTime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value, 
        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value, 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
    };
});

builder.Services.AddAuthorization();

//swagger auth
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "GloBus API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

//logging
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
});

//register repository services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ILinesRepository, LinesRepository>();
builder.Services.AddScoped<ITicketsRepository, TicketsRepository>();
builder.Services.AddScoped<ITicketTypesRepository, TicketTypesRepository>();
builder.Services.AddScoped<IActiveTicketsRepository, ActiveTicketsRepository>();
builder.Services.AddScoped<IInvalidTicketsRepository, InvalidTicketsRepository>();
builder.Services.AddScoped<IRegionsRepository, RegionsRepository>();
builder.Services.AddScoped<IPenaltiesRepository, PenaltiesRepository>();
builder.Services.AddScoped<IAdminsRepository, AdminsRepository>();

//register services for recurring jobs
builder.Services.AddTransient<ICheckForInvalidTickets, CheckForInvalidTickets>();
builder.Services.AddTransient<IDeleteObsoleteTickets, DeleteObsoleteTickets>();

//automapper config
builder.Services.AddAutoMapper(typeof(Program).Assembly);

//hangfire
builder.Services.AddHangfire(config =>
{
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("default"));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.UseHangfireServer();
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate<ICheckForInvalidTickets>("checking-for-invalid-tickets", service => service.Check(), Cron.Minutely);
RecurringJob.AddOrUpdate<IDeleteObsoleteTickets>("delete-obsolete-tickets", service => service.Delete(), Cron.Monthly);

app.Run();

