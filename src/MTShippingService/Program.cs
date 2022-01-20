using MassTransit;
using MassTransit.PrometheusIntegration;
using MTShippingService;
using Prometheus;
using Serilog;

//Set early console for startup info
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var host = Host.CreateDefaultBuilder(args);

    host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

    host.ConfigureServices(services =>
    {
        // Add MassTransit to service container
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
                cfg.UsePrometheusMetrics(serviceName: "MTShippingService");
            });
        });

        //Add MT Hosted Service
        services.AddMassTransitHostedService(true);
    });

    var app = host.Build();

    var metricServer = new KestrelMetricServer(port: 7200);
    metricServer.Start();

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
