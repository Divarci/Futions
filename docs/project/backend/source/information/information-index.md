# Project Architecture — Backend (overview)

This folder contains short, focused notes describing the backend architecture and the conventions used in this repository.

Pages

- [Architecture](architecture.md) — High-level summary of the hybrid Clean Architecture + Vertical Slice approach, layer responsibilities, inter-layer dependency rules, scalability model, and Docker Compose setup.
- [Coding Style](coding-style.md) — Code quality and consistency standards. Covers naming conventions, vertical alignment code writing, unused code rules, class design, and record design.
- [Domain Driven Design](domain-driven-design.md) — DDD decisions made in the domain layer. Covers entity design, value object design, aggregate boundaries, domain event decisions, repository interface decisions, model (DTO) decisions, multi-tenancy, and soft delete.
- [Event Driven Design](event-driven-design.md) — How domain events are raised, persisted via the Transactional Outbox pattern, and processed asynchronously by a background scheduler.
- [Tech Stack](tech-stack.md) — What technologies are used and in which project. No descriptions or version numbers.

Guidelines

- These docs are intentionally concise — use them as the single source for architectural decisions when writing AI prompts or onboarding notes.

