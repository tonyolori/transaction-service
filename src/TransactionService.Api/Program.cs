using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using TransactionService.Application.Handlers;
using TransactionService.Infrastructure.DocGen;
using TransactionService.Infrastructure.Signing;
using TransactionService.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GenerateReceiptHandler).Assembly));

// Infrastructure bindings
builder.Services.AddSingleton<IDocumentGenerator, PdfDocumentGenerator>();
builder.Services.AddSingleton<ILinkSigner>(sp => new HmacLinkSigner(builder.Configuration["Signing:Key"] ?? "dev-key"));
builder.Services.AddSingleton<IInMemoryQueue, InMemoryQueue>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
