using Adapter.RestApi;
using App.Services;
using App.UseCases;
using Infra.Caching;
using Infra.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .RegisterRestApiLayer()
    .RegisterUseCaseLayer()
    .RegisterServiceLayer()
    .RegisterCachingLayer()
    .RegisterPersistenceLayer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.MapGet("/health", () => Results.Ok(new { status = "healthy", env = app.Environment.EnvironmentName }));


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
