# DPBack - Print Shop API
Backend for a pint shop / e-commerce platform build with ASP.NET Core 8

## Features
- Order management with status workflow
- JWT authentication with role-based access (Admin, Worker, Customer)
- PayU payment integration with webhook handling
- Dynamic price calculation (Strategy patern)
- Paginated order queries

## Tech Stack
- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- JWT Bearer
- PayU REST API

## Achitecture
Clean Architecture with 5 layers:
- **Domain** - domain models, enums
- **Application** - inerfaces, business logic, DTOs, services
- **Infrastructure** - EF Core, repositories, PayU, entities
- **API** - controllers, middleware
- **Tests** - well, unit-test

## Getting started 🚧
_instructions will be added later🤷‍♂️_

## Design Decisions
- Strategy pattern for price calculators. Adding new product type requires only a new class implementing IPriceCalculator
- PayU webhooke ("notify") signature verification via HMAC
- Global exception handler middleware

#
__API documentation available via Swagger__