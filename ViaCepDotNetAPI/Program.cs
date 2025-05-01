using System.Text.Json;
using System.Text.Json.Serialization;
using ViaCepDotNetAPI.Domains.Dtos;
using ViaCepDotNetAPI.Domains.Entities;
using ViaCepDotNetAPI.Domains.Mappings;
using ViaCepDotNetAPI.Domains.Shared;
using ViaCepDotNetAPI.Endpoints;
using ViaCepDotNetAPI.Infrastructure.Configurations;
using ViaCepDotNetAPI.Infrastructure.Services;
using ViaCepDotNetAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()); //null, allowIntegerValues: false));
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

app.MapCepRoutes();

app.Run();
