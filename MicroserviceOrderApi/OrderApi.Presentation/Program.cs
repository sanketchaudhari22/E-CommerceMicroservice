using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interface;
using OrderApi.Application.Service;
using OrderApi.Infrastructure.Data;
using OrderApi.Infrastructure.Repositories;
using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

// ?? DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("E-CommerceMicroservice")));

// ?? Repository
builder.Services.AddScoped<IOrderInterface, OrderRepository>();

// ?? Polly ResiliencePipeline
builder.Services.AddSingleton<ResiliencePipeline>(sp =>
{
    return new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(2)
        })
        .Build();
});

// ?? HttpClients for microservices
builder.Services.AddHttpClient("ProductApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

builder.Services.AddHttpClient("AuthApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000");
});

// ?? Service
builder.Services.AddScoped<IOrderService, OrderService>();

// ?? Authentication & Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = false
    };
});

builder.Services.AddAuthorization();

// ?? Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ?? Middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication(); // MUST be before UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();
