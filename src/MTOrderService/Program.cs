using System.Reflection;
using MassTransit;
using MassTransit.PrometheusIntegration;
using Microsoft.OpenApi.Models;
using Prometheus;
using Serilog;

//Set early console for startup info
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);

    //Add serilog logger from configuration
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration));

    //Consifure XOrigin
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder => 
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
    }); 

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

    //Add request logging features
    app.UseSerilogRequestLogging();

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
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
