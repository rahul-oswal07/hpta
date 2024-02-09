using AutoMapper;
using Azure.Identity;
using DevCentralClient.Infrastructure;
using DevCentralClient.Models;
using HPTA.Api.AuthorizationPolicies;
using HPTA.Api.Infrastructure;
using HPTA.Common.Configurations;
using HPTA.Mapping.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsProduction())
{
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());
}
var configRoot = new
{
    ConnectionStrings = new ConnectionStrings(),
    DevCentralConfig = new DevCentralConfig()
};

builder.Configuration.Bind(configRoot);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApi(builder.Configuration);

// Add services to the container.
builder.Services.RegisterDependency(configRoot.ConnectionStrings);
builder.Services.RegisterDevCentralClient(configRoot.DevCentralConfig);
builder.Services.RegisterMappingProfiles();
builder.Services.AddControllers();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthorization(options =>
{
    options.Register(builder.Configuration, "AuthorizationPolicies");
});

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutoMapperConfig());
}).CreateMapper());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(options => options
    .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").GetChildren().Select(c => c.Value).ToArray())
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

//app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
