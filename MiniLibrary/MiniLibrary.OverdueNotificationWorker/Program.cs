using MiniLibrary.Application;
using MiniLibrary.Infrastructure;
using MiniLibrary.OverdueNotificationWorker;

var builder = Host.CreateApplicationBuilder(args);

// Add application and infrastructure services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Add the worker
builder.Services.AddHostedService<OverdueNotificationWorker>();

var host = builder.Build();
host.Run();
