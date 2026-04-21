using API_pokoje_s33979;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// builder.Services.AddEndpointsApiExplorer(); // Wymagane, by Swagger "widział" endpointy

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("openapi/v1.json", "v1");
        s.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();