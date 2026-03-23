# App.UseCases — Structure

```
App.UseCases/
├── {Module}/
│   └── {Entities}/
│       ├── {Entity}UseCase.cs
│       ├── {Entity}UseCase.Create.cs
│       ├── {Entity}UseCase.Update.cs
│       ├── {Entity}UseCase.Delete.cs
│       ├── {Entity}UseCase.Single.cs
│       └── {Entity}UseCase.Collection.cs
├── Helpers/
│   └── CacheKeyHelper.cs
├── Scheduling/
│   └── DomainEvents/
│       ├── {Module}/
│       │   └── {Entity}{Event}DomainEventHandler.cs
│       ├── {Concern}Processor.cs
│       └── {Concern}Processor.Private.cs
├── App.UseCases.csproj
└── ServiceRegistrar.cs
```

## Key conventions

| Convention | Rule |
|---|---|
| One folder per domain module | Mirrors the `Core.Domain/Entities/{Module}` layout |
| One partial file per operation | `{Entity}UseCase.Create.cs`, `.Update.cs`, `.Delete.cs`, `.Single.cs`, `.Collection.cs` |
| Root partial file | `{Entity}UseCase.cs` — constructor and injected fields only, no methods |
| Cache key helpers | `CacheKeyHelper.Single` and `CacheKeyHelper.Collection` are passed as delegates to service calls |
| DI registration | `ServiceRegistrar.cs` — `AddScoped<I{Entity}UseCase, {Entity}UseCase>()` for every use case |
