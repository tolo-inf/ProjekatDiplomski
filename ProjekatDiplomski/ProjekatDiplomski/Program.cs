using ManticoreSearch.Api;
using ManticoreSearch.Client;
using ManticoreSearch.Model;
using Microsoft.OpenApi.Models;
using ProjekatDiplomski.Services;
using ProjekatDiplomski.Services.IServices;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
});

Configuration config = new Configuration();
config.BasePath = "http://127.0.0.1:9308";
HttpClient httpClient = new HttpClient();
HttpClientHandler httpClientHandler = new HttpClientHandler();

var indexApi = new IndexApi(httpClient, config, httpClientHandler);
var searchApi = new SearchApi(httpClient, config, httpClientHandler);
var utilsApi = new UtilsApi(httpClient, config, httpClientHandler);

builder.Services.AddSingleton<IIndexApi>(indexApi);
builder.Services.AddSingleton<ISearchApi>(searchApi);
builder.Services.AddSingleton<IUtilsApi>(utilsApi);

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
