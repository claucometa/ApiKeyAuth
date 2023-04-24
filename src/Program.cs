using ApiKeyAuth.Secure.AuthKey.MinimalAPI.Filter;
using ApiKeyAuthentication.Secure.Key.RestAPI.Strategy.Attributes;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add to all controllers
builder.Services.AddControllers(x => x.Filters.Add<ApiKeyAttribute>());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
    {
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme",
        In = ParameterLocation.Header,
        Description = "ApiKey must appear in header"
    });

    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "X-API-KEY"
                },
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseMiddleware<ApiKeyMiddleware>();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var group = app.MapGroup("WeatherForeCast");
group.MapGet("mini/weather", () => WeatherService.GetWeather());
group.MapGet("mini/weather/key", () => WeatherService.GetWeather())
     .AddEndpointFilter<ApiKeyEndpointFilter>();

app.Run();