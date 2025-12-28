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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add RedisCacheService
builder.Services.AddSingleton<RedisCacheService>(sp =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
    return new RedisCacheService(redisConnectionString);
});

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(Pharmacy.Application.DTOs.Members.MemberDto).Assembly);

// Add Entity Framework
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// ====================== AUTO MIGRATION ======================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PharmacyDbContext>();
    dbContext.Database.Migrate();
}
// ===========================================================


// Configure the HTTP request pipeline.
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
