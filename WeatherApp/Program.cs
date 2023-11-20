using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WeatherApp.Application.Configuration;
using WeatherApp.Application.Services.Implementation;
using WeatherApp.Application.Services.Interfaces;
using WeatherApp.Application.Utils.Implementation;
using WeatherApp.Application.Utils.Interfaces;
using WeatherApp.Core.CommonServices.Implementation;
using WeatherApp.Core.CommonServices.Interfaces;
using WeatherApp.Core.Interfaces.Repositories;
using WeatherApp.DataAccess.Repositories;
using WeatherApp.Database;
using WeatherApp.Database.Seeds;
using WeatherApp.Mapping;
using WeatherApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("DatabaseConnection").Value);
});

builder.Services.AddSingleton(_ => builder.Configuration.GetSection("PeriodicWeather").Get<PeriodicWeather>());


var config = new MapperConfiguration(c => {
    c.AddProfile<WeatherProfile>();
});
builder.Services.AddSingleton<IMapper>(s => config.CreateMapper());

builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IJsonSerializationService, JsonSerializationService>();
builder.Services.AddScoped<IDateTimeService, DateTimeService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();

builder.Services.AddScoped<ICityWeatherRepository, CityWeatherRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddSingleton<IKelvinToCelciusConverter, KelvinToCelciusConverter>();
builder.Services.AddScoped<IPeriodicWeatherService, PeriodicWeatherService>();
builder.Services.AddScoped<ICityWeatherService, CityWeatherService>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddSingleton<PeriodicWeatherHostedService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<PeriodicWeatherHostedService>());

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
    InitialSeeds.Seed(context.Database);
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
