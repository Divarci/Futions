# Adapter.Scheduler

## Description

`Adapter.Scheduler` is a .NET Worker Service that hosts the background job scheduler. It is the adapter responsible for all time-triggered, recurring background processing in the system.

The project contains no business logic. Its only jobs are to:

1. **Bootstrap the host** — wire all layers together through their `Register*` extension methods.
2. **Register jobs and their triggers** — define what runs, when it runs, and how often.
3. **Delegate execution** — each job calls into the Application layer (use cases or services) and surfaces errors as structured exceptions.

At present, the scheduler hosts one job: the outbox processor, which polls the database for unprocessed domain events and dispatches them to their handlers. The architecture is designed so that adding new jobs requires no changes to existing code.

**Source reference:** `Api/Source/Adapter/Adapter.Scheduler/`

---

## Structure Tree

```
Adapter.Scheduler/
├── Program.cs                         ← Host bootstrap; no logic
├── ServiceRegistrar.cs                ← Registers all jobs and their triggers
│
├── Configuration/
│   ├── {Job}Options.cs                ← Strongly-typed config record per job
│   └── SerializerOptions.cs           ← Shared JSON serializer singleton
│
├── Jobs/
│   └── {Job}.cs                       ← One file per job; implements IJob
│
└── Properties/
    └── launchSettings.json            ← Dev launch profiles (Project + Docker)
```

**Current jobs:**

| Job | Config Section | Trigger type |
|---|---|---|
| `ProcessOutboxJob` | `Outbox` | Fixed interval (seconds) |

---

## Critical Rules

1. **No domain logic in jobs.** A job is only glue code. It calls a use case or service method, checks the result, and either succeeds or throws. Business logic lives in the Application layer.

2. **Always apply `[DisallowConcurrentExecution]`.** Every job class must carry this attribute. The scheduler must never run two instances of the same job simultaneously.

3. **Job failure throws `{Solution}Exception`.** When a result indicates failure, the job throws a `{Solution}Exception` with four required arguments: `assemblyName`, `className`, `methodName`, and `message`. This ensures all failures produce structured, traceable error entries.

4. **One `{Job}Options` class per job.** Each job has its own configuration record under `Configuration/`. Options records implement the corresponding interface from `Core.Library` and map to a dedicated config section key (`SectionKey` constant on the record itself).

5. **`ServiceRegistrar` owns all job registration.** Jobs, triggers, and their schedules are registered exclusively inside `ServiceRegistrar.AddBackgroundServices()`. `Program.cs` never touches scheduler internals.

6. **`SerializerOptions` is a shared singleton.** Never instantiate `JsonSerializerOptions` inside a job. Use `SerializerOptions.Instance` from the `Configuration/` class.

7. **Primary constructor injection.** Jobs use primary constructor syntax. Each injected dependency is stored in a `readonly` backing field with the same name (lowercase, prefixed with `_`).

8. **`WaitForJobsToComplete = true`.** The hosted scheduler service is always configured to wait for in-flight jobs before the process shuts down, preventing partial processing on graceful stop.

---

## Program.cs — How the Host Is Bootstrapped

The entry point uses `Host.CreateApplicationBuilder` (Worker SDK — no HTTP pipeline, no middleware). It calls each layer's `Register*` extension method in dependency order and then runs the host.

```csharp
var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .RegisterSchedulerLayer(builder.Configuration)   // jobs + triggers
    .RegisterUseCaseLayer()                          // application use cases
    .RegisterServiceLayer()                          // application services
    .RegisterCachingLayer(builder.Configuration)     // infrastructure: cache
    .RegisterPersistenceLayer(builder.Configuration);// infrastructure: database

var host = builder.Build();
host.Run();
```

**Rules for `Program.cs`:**

- Only `Register*` calls and `host.Run()` — nothing else.
- Layer registration order is fixed: Scheduler → UseCases → Services → Caching → Persistence.
- No direct `services.Add*` calls in `Program.cs`. All registrations go through the layer's own `ServiceRegistrar`.

---

## How to Scale — Adding a New Job

Adding a new job follows a four-step pattern. No existing files are modified except `ServiceRegistrar.cs`.

### Step 1 — Create the Options record

**Path:** `Configuration/{Job}Options.cs`

```csharp
using Core.Library.Abstractions.Interfaces;

namespace Adapter.Scheduler.Configuration;

internal sealed record {Job}Options : I{Job}Options
{
    public const string SectionKey = "{Section}";

    public int IntervalInSeconds { get; init; }
    // add other config properties as needed
}
```

- The record must be `internal sealed`.
- It must implement the interface declared in `Core.Library.Abstractions.Interfaces`.
- `SectionKey` is `const string` — always use it when binding config, never hardcode the string elsewhere.

### Step 2 — Add the config section to `appsettings.json`

```json
"{Section}": {
  "IntervalInSeconds": 30
}
```

### Step 3 — Create the job

**Path:** `Jobs/{Job}.cs`

```csharp
using Adapter.Scheduler.Configuration;
using Core.Library.Exceptions;
using Core.Library.ResultPattern;
using Microsoft.Extensions.Options;
using Quartz;

namespace Adapter.Scheduler.Jobs;

[DisallowConcurrentExecution]
internal sealed class {Job}(
    I{UseCase} {useCase},
    IOptions<{Job}Options> options) : IJob
{
    private readonly I{UseCase} _{useCase} = {useCase};
    private readonly {Job}Options _{job}Options = options.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        Result result = await _{useCase}.{Method}Async(_{job}Options.{Property}, SerializerOptions.Instance);

        if (result.IsFailure)
            throw new {Solution}Exception(
                assemblyName: "Adapter.Scheduler",
                className: nameof({Job}),
                methodName: nameof(Execute),
                message: $"...");
    }
}
```

### Step 4 — Register the job and trigger in `ServiceRegistrar`

Inside `AddBackgroundServices()`, add a new job/trigger block alongside any existing ones:

```csharp
services.AddQuartz(q =>
{
    // existing jobs...

    JobKey {job}Key = new(nameof({Job}));

    q.AddJob<{Job}>(opts => opts.WithIdentity({job}Key));

    q.AddTrigger(opts => opts
        .ForJob({job}Key)
        .WithIdentity($"{nameof({Job})}-trigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInSeconds({job}Options.IntervalInSeconds)
            .RepeatForever()));
});
```

Also bind and configure the new options at the top of `RegisterSchedulerLayer`:

```csharp
services.Configure<{Job}Options>(configuration.GetSection({Job}Options.SectionKey));
```

That is the complete process. No changes to `Program.cs`, no changes to any existing job, no changes to any other layer.
