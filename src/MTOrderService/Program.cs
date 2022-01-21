using System.Reflection;
using MassTransit;
using MassTransit.PrometheusIntegration;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Prometheus;

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Configure Microsoft logging with Seq Sink
    builder.Services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddSeq(builder.Configuration.GetSection("Seq"));
    });

    //Configure XOrigin
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder => 
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    }); 

    //Configure OpenTelemetry Tracing
    builder.Services.AddOpenTelemetryTracing((builder) => builder
        .AddAspNetCoreInstrumentation()
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MTOrderService"))
        .AddSource("MTOrderService")
        .AddJaegerExporter(o =>
        {
            o.AgentHost = "localhost";
            o.AgentPort = 6831;
        }));

    // Add MassTransit
    builder.Services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.ConfigureEndpoints(context);
            cfg.UsePrometheusMetrics(serviceName: "MTOrderService");
        });
    });

    builder.Services.AddControllers();
    //builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Order Service API",
            Description = "Demo API used to receive Orders from Web Application"      
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });

    //Add MassTransit Hosted Service
    builder.Services.AddMassTransitHostedService(true);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseRouting();

    app.UseAuthorization();

    app.UseCors();

    //Add MT Metrics
    app.UseEndpoints(endpoints =>
    {
        // add this line
        endpoints.MapMetrics();
        endpoints.MapControllers();
    });

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
