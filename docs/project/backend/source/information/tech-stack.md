# Tech Stack

---

## Runtime & Language

| Technology | Project |
|---|---|
| **.NET** | All projects |
| **C#** | All projects |

---

## Web & Host

| Technology | Project |
|---|---|
| **ASP.NET Core** (Web SDK) | `Adapter.RestApi` |
| **.NET Generic Host** (Worker SDK) | `Adapter.Scheduler` |

---

## Data Access

| Technology | Project |
|---|---|
| **Entity Framework Core** | `Infra.Persistence` |
| **SQL Server** | `Infra.Persistence` |
| **EF Core Design** (migrations tooling) | `Adapter.RestApi` |

---

## Caching

| Technology | Project |
|---|---|
| **Redis** | `Infra.Caching` |
| **StackExchange.Redis** | `Infra.Caching` |

---

## Background Processing

| Technology | Project |
|---|---|
| **Quartz.NET** | `Adapter.Scheduler` |

---

## Authentication

| Technology | Project |
|---|---|
| **Keycloak** (external IdP) | — |
| **JWT Bearer** | `Adapter.RestApi` |

---

## API

| Technology | Project |
|---|---|
| **Asp.Versioning.Mvc** | `Adapter.RestApi` |
| **OpenAPI** | `Adapter.RestApi` |

---

## Logging

| Technology | Project |
|---|---|
| **Serilog** | `Adapter.RestApi` |
| **Seq** | `Adapter.RestApi` |

---

## Containerisation

| Technology | Usage |
|---|---|
| **Docker** | All services |
| **Docker Compose** | `backend`, `scheduler`, `redis`, `sqlserver` *(dev only)*, `serilog` |

