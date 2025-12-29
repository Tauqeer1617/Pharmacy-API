using Microsoft.EntityFrameworkCore;
using Pharmacy.Infrastructure.Data;
using Pharmacy.Domain.Interfaces;
using Pharmacy.Infrastructure.Repositories;
using Pharmacy.Infrastructure.UnitOfWork;
using Pharmacy.Application.Services.Interfaces;
using Pharmacy.Application.Services.Implementations;
using FluentValidation;
using FluentValidation.AspNetCore;
using Pharmacy.Infrastructure.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// ======================= Add Services =======================

// Add controllers
builder.Services.AddControllers();

// Add Redis ConnectionMultiplexer (Singleton)
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("RedisConnection")
    );

    configuration.AbortOnConnectFail = false; // App won't crash if Redis is down
    configuration.ConnectRetry = 5;           // Retry 5 times
    configuration.ConnectTimeout = 5000;      // 5-second timeout

    return ConnectionMultiplexer.Connect(configuration);
});

// Add RedisCacheService (Singleton)
builder.Services.AddSingleton<RedisCacheService>();

// Configure CORS (Allow all)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Pharmacy.Application.DTOs.Members.MemberDto).Assembly);

// Entity Framework Core
builder.Services.AddDbContext<PharmacyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PharmacyConnection")));

// Register repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IProviderRepository, ProviderRepository>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register application services
builder.Services.AddScoped<IMemberService, MemberService>(sp =>
{
    var unitOfWork = sp.GetRequiredService<IUnitOfWork>();
    var redisCache = sp.GetRequiredService<RedisCacheService>();
    return new MemberService(unitOfWork, redisCache);
});

builder.Services.AddScoped<IProviderService, ProviderService>();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// ======================= Build App =======================
var app = builder.Build();


// ====================== AUTO MIGRATION ======================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();
    dbContext.Database.Migrate();
}
// ===========================================================


// ======================= Configure Pipeline =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
