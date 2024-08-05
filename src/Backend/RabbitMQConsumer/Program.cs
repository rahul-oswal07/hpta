using EmailClient.Infrastructure;
using HPTA.Common.Configurations;

var builder = Host.CreateApplicationBuilder(args);
var appSettings = new ApplicationSettings();

builder.Configuration.Bind(appSettings); // Adjust this line to your actual app settings loading logic
builder.Services.RegisterEmailClient(builder.Environment.ContentRootFileProvider, appSettings);


var host = builder.Build();
host.Run();
