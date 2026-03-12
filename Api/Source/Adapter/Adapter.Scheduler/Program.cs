using Adapter.Scheduler;
using App.Services;
using App.UseCases;
using Infra.Caching;
using Infra.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .RegisterSchedulerLayer(builder.Configuration)
    .RegisterUseCaseLayer()
    .RegisterServiceLayer()
    .RegisterCachingLayer(builder.Configuration)
    .RegisterPersistenceLayer();

var host = builder.Build();
host.Run();
