using ProductApi.Infrastructure.DependencyInjection;
using E_CommerceSharedLibrary.DependencyInjecation;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddJwtAutheticationschem(builder.Configuration); // ?? add JWT

var app = builder.Build();

// Middleware order is important
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseInfrastructurePolicy(); // ?? global middleware + API gateway
app.UseHttpsRedirection();

app.UseAuthentication(); // ?? add authentication
app.UseAuthorization();

app.MapControllers();

app.Run();
