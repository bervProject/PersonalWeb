using PersonalWeb.MigrationService;
using PersonalWeb.Api.EntityFramework;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddHostedService<Worker>();
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
builder.AddNpgsqlDbContext<PersonalWebApiContext>("Default");

var host = builder.Build();
host.Run();
