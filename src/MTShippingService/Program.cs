using MassTransit;
using MassTransit.PrometheusIntegration;
using MTShippingService;
using Prometheus;

try
{
    var host = Host.CreateDefaultBuilder(args);

    //Fetch appsettings for WorkerService application
    var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddJsonFile("appsettings.json")
        .Build();

    host.ConfigureServices(services =>
    {
        //Configure Microsoft logging with Seq Sink
        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSeq(configuration.GetSection("Seq"));
        });

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
    Console.WriteLine(ex.Message);
}
