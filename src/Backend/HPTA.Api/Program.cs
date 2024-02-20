using Azure.Identity;
using DevCentralClient.Infrastructure;
using HPTA.Api.AuthorizationPolicies;
using HPTA.Api.Infrastructure;
using HPTA.Common.Configurations;
using HPTA.Mapping.Infrastructure;
using HPTA.Scheduler.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}
var appSettings = new ApplicationSettings();

builder.Configuration.Bind(appSettings);
appSettings.MasterDbConnectionString = builder.Configuration.GetConnectionString("MasterDbConnectionString");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApi(builder.Configuration);

// Add services to the container.
builder.Services.RegisterDependency(appSettings);
builder.Services.RegisterDevCentralClient(appSettings);
builder.Services.RegisterMappingProfiles();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

//builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.Register(builder.Configuration, "AuthorizationPolicies");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Scheduler
builder.Services.RegisterScheduler(appSettings);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();
app.UseCors(options => options
    .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").GetChildren().Select(c => c.Value).ToArray())
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.InitTeamSync();

//app.MapControllers();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
