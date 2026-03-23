# App.Services — Structure

```
App.Services/
├── {Module}/
│   └── {Entities}/
│       ├── {Entity}Service.cs
│       ├── {Entity}Service.Create.cs
│       ├── {Entity}Service.Update.cs
│       ├── {Entity}Service.Delete.cs
│       ├── {Entity}Service.Single.cs
│       └── {Entity}Service.Collection.cs
├── App.Services.csproj
└── ServiceRegistrar.cs
```

## Key conventions

| Convention | Rule |
|---|---|
| One folder per domain module | Mirrors the `Core.Domain/Entities/{Module}` layout |
| One partial file per operation | `{Entity}Service.Create.cs`, `.Update.cs`, `.Delete.cs`, `.Single.cs`, `.Collection.cs` |
| Root partial file | `{Entity}Service.cs` — constructor and injected fields only, no methods |
| DI registration | `ServiceRegistrar.cs` — `AddScoped<I{Entity}Service, {Entity}Service>()` for every service |
