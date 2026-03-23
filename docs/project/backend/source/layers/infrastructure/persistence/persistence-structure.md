# Infra.Persistence — Structure

```
Infra.Persistence/
├── Configurations/
│   ├── {Module}/              ← one folder per domain module
│   │   ├── {Entity}Config.cs
│   │   └── ...
│   └── System/
│       ├── AuditLogConfig.cs
│       └── OutboxMessageConfig.cs
├── Context/
│   ├── AppDbContext.cs
│   └── AppDbContextFactory.cs
├── Extensions/
│   └── IQueryableExtensions.cs
├── Migrations/
│   └── ...
├── Repositories/
│   ├── Generics/
│   │   ├── BaseRepository.cs
│   │   ├── BaseRepository.Create.cs
│   │   ├── BaseRepository.Update.cs
│   │   ├── BaseRepository.Delete.cs
│   │   ├── TenantedRepository.cs
│   │   ├── TenantedRepository.Single.cs
│   │   ├── TenantedRepository.Exist.cs
│   │   ├── TenantedRepository.Count.cs
│   │   ├── GlobalRepository.cs
│   │   ├── GlobalRepository.Single.cs
│   │   ├── GlobalRepository.Exist.cs
│   │   └── GlobalRepository.Count.cs
│   ├── {Module}/
│   │   └── {Entities}/
│   │       ├── {Entity}Repository.cs
│   │       ├── {Entity}Repository.Collection.cs
│   │       └── ...                              ← additional partial files as needed
│   └── System/
│       ├── AuditLogs/
│       │   ├── AuditLogRepository.cs
│       │   └── AuditLogRepository.Collection.cs
│       └── OutboxMessages/
│           ├── OutboxMessageRepository.cs
│           └── OutboxMessageRepository.Collection.cs
├── UnitOfWorks/
│   └── UnitOfWork.cs
├── Infra.Persistence.csproj
├── SerializerOptions.cs
└── ServiceRegistrar.cs
```

## Key folders

| Folder | Purpose |
|---|---|
| `Configurations/` | `IEntityTypeConfiguration<T>` files; auto-applied by `AppDbContext` |
| `Context/` | `AppDbContext` and design-time factory |
| `Extensions/` | `IQueryable` extension methods (`WhereIf`, `OrderByIf`) |
| `Migrations/` | EF Core generated migration files; never edited manually |
| `Repositories/Generics/` | Abstract base and intermediate repositories |
| `Repositories/{Module}/` | Concrete repository partial files per domain module |
| `UnitOfWorks/` | `UnitOfWork` — the only component that calls `SaveChangesAsync` |
