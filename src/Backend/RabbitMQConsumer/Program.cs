using EmailClient.Infrastructure;
using HPTA.Common.Configurations;
using RabbitMQConsumer;

var builder = Host.CreateApplicationBuilder(args);
var appSettings = new ApplicationSettings();

builder.Configuration.Bind(appSettings);
//appSettings.EmailClientConfig = builder.Configuration.GetConnectionString("EmailClientConfig");// Adjust this line to your actual app settings loading logic
builder.Services.RegisterEmailClient(builder.Environment.ContentRootFileProvider, appSettings);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
