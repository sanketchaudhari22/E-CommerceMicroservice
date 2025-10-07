using Microsoft.EntityFrameworkCore;
using OrderApi.Infrastructure.Data;
using OrderApi.Application.Interface;
using OrderApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("E-CommerceMicroservice")));

// Repository
builder.Services.AddScoped<IOrderInterface, OrderRepository>();

// Other services like controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
