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
    .RegisterCachingLayer(builder.Configuration)
    .RegisterPersistenceLayer(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
