using Microsoft.OpenApi.Models;  // Для OpenApiInfo и других типов Swagger

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
});

// Configure CORS in the `ConfigureServices` area (where `builder.Services` is available)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        policy => policy.WithOrigins("http://localhost:7285") // Замените на адрес вашего фронтенда
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1"));
}

app.UseHttpsRedirection();

// Add CORS middleware *after* UseHttpsRedirection and *before* UseAuthorization
app.UseCors("AllowMyOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();