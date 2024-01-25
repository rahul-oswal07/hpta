using HPTA.Api.AuthorizationPolicies;
using HPTA.Repositories.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(options =>
    {
        builder.Configuration.Bind("AzureAd", options);
        options.TokenValidationParameters.NameClaimType = "name";
    },
          options =>
          {
              builder.Configuration.Bind("AzureAd", options);
          });

builder.Services.DependencyRegistry(builder.Configuration.GetConnectionString("MasterDbConnectionString"));
builder.Services.AddControllers();

builder.Services.AddAuthorization(options =>
{
    options.Register(builder.Configuration, "AuthorizationPolicies");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseCors(options => options
    .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").GetChildren().Select(c => c.Value).ToArray())
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
