# sneakers-shop-api
API to sneakers shop e-commercial project with automatizations in CI github. Docker for database, and compose solution

# Roadmap

## Completed

- **Domain Layer** — Onion/Clean Architecture with separate .csproj per layer, pure Domain with zero external dependencies
- **DDD** — Aggregate roots, Value Objects (`SubmissionSize`, `Address`), TPH inheritance for discounts, `Audience` enum, Domain Events on entities with `DbContext.SaveChangesAsync` dispatch
- **CQRS + MediatR** — Commands and Queries for Submissions, Orders, Reservations, Products, Auth
- **Authentication** — ASP.NET Core Identity + JWT with refresh token rotation, CQRS-based `LoginCommand`, `RegisterCommand`, `RefreshTokenCommand`
- **Authorization** — Role seeding (Admin, Moderator, Customer, Dropper), policy-based auth with `ActiveDropperRequirement` and custom `ActiveModeratorHandler` (email confirmed check)
- **Result Pattern** — `Result<T>` / `Error` for flow control instead of exceptions, `ToActionResult` extension mapping error codes to HTTP status codes
- **Validation** — FluentValidation integrated into MediatR pipeline via `ValidationBehavior`
- **Database** — EF Core + PostgreSQL with Docker Compose, navigation properties, `OwnsMany` for value objects
- **Storage** — `IStorageService` abstraction with `LocalStorageService` implementation, file validation (size, type, count)
- **Size conversion** — `SizeConversionService` for EU/US/UK → CM with seeded `Size` dictionary
- **Submissions module** — Full dropper flow (CRUD) + moderator flow (Approve, Reject, UpdateDetails, GetPending, GetById)
- **Orders & Cart** — Reservation-based cart with 2h TTL, `CreateOrder` converting reservations, order lifecycle (Pending, Paid, Shipped, Delivered, Cancelled)
- **Background Jobs** — `ExpireReservationsJob` via `BackgroundService` running every 15 minutes
- **Redis Cache** — `IDistributedCache` with cache-aside pattern for `GetMyCartQuery`, 5 min TTL, invalidation on cart mutations
- **CI/CD** — GitHub Actions pipeline with Postman integration tests, branch protection rules, SonarQube analysis
- **Controllers refactoring** — Split by role (`DroppersSubmissionsController`, `ModeratorsSubmissionsController`), kebab-case routes via `SlugifyParameterTransformer`

## In Progress

- **Products Controller** — Public endpoints for product browsing, filtering, search
- **Unit Tests** — xUnit + Moq covering Domain entities and Application handlers

## Planned

- **Security hardening** — Path Traversal protection in `StorageService`, magic bytes file validation, stream disposal fixes
- **Rate Limiting** — Brute-force protection on auth endpoints via built-in `AddRateLimiter`
- **Payment integration** — Stripe/payment gateway for order fulfillment
- **User profile management** — Update profile, shipping addresses, order history
- **Comments and reviews** — Product ratings and comment moderation
- **Wishlist** — Per-user wishlist without stock reservation
- **Promo codes and discounts** — Discount engine with TPH inheritance already modeled
- **Email notifications** — `EmailService` for order confirmations, password reset, email verification
- **Dropper payouts** — `SalesSnapshot` background job, payout calculations
- **Admin analytics** — Sales dashboard, product statistics, moderator performance
- **Structured logging** — Serilog with JSON output for production observability
- **Health checks** — `/health` endpoint for database and Redis connectivity
- **API versioning** — `Asp.Versioning.Http` for future breaking changes
