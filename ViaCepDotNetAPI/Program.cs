using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Infrastructure.Configurations;
using ViaCepDotNetAPI.Infrastructure.Services;
using ViaCepDotNetAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<ViaCepOptions>(options => builder.Configuration
        .GetSection("ViaCep")
        .Bind(options));

builder.Services.AddHttpClient();

builder.Services.AddScoped<IViaCepService, ViaCepService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/viaCep", async (string cep, IViaCepService viaCepService) =>
{
    Root result = await viaCepService.GetAddressByCepAsync(cep);

    return result is null
        ? Results.BadRequest("Invalid CEP or not found.")
        : Results.Ok(result);
})
.WithName("GetAddressByCepAsync")
.WithOpenApi();

app.Run();
