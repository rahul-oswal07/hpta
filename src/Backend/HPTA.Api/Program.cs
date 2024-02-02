using HPTA.Api.AuthorizationPolicies;
using HPTA.Api.Infrastructure;
using HPTA.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);
var configRoot = new
{
    ConnectionStrings = new ConnectionStrings()
};

builder.Configuration.Bind(configRoot);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddMicrosoftIdentityWebApi(builder.Configuration);

// Add services to the container.
builder.Services.RegisterDependency(configRoot.ConnectionStrings);
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
