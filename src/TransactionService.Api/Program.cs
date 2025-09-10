using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using TransactionService.Grpc;
using TransactionService.Application.Handlers;
using TransactionService.Infrastructure.DocGen;
using TransactionService.Infrastructure.Signing;
using TransactionService.Infrastructure.Messaging;
using Grpc.Net.ClientFactory;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GenerateReceiptHandler).Assembly));

// Infrastructure bindings
builder.Services.AddSingleton<IDocumentGenerator, PdfDocumentGenerator>();
builder.Services.AddSingleton<ILinkSigner>(sp => new HmacLinkSigner(builder.Configuration["Signing:Key"] ?? "dev-key"));
builder.Services.AddSingleton<IInMemoryQueue, InMemoryQueue>();

// Add health checks
builder.Services.AddHealthChecks();

var grpcAddress = builder.Configuration["GrpcSettings:GrpcServer"];

builder.Services.AddGrpcClient<Greeter.GreeterClient>(o =>
{
    o.Address = new Uri(grpcAddress!);
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "DENY";
    context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'";

    await next();
});

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (error != null)
        {
            Log.Error(error.Error, "Unhandled exception occurred");
            
            var response = new
            {
                error = "An internal server error occurred",
                requestId = Activity.Current?.Id ?? context.TraceIdentifier
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    });
});

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                exception = x.Value.Exception?.Message,
                duration = x.Value.Duration.ToString()
            }),
            totalTime = report.TotalDuration.ToString()
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready")
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false
});

app.MapControllers();

app.Run();

public partial class Program { } // for tests