# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

nopCommerce is a free, open-source ASP.NET Core eCommerce platform built on .NET 9. This is a mature, enterprise-level solution with over 3 million downloads and an active community of 250,000+ developers.

## Common Development Commands

### Build & Run
- **Build solution**: `dotnet build` or `dotnet build NopCommerce.sln`
- **Run web application**: `dotnet run --project Presentation/Nop.Web`
- **Build specific project**: `dotnet build Libraries/Nop.Core/Nop.Core.csproj`
- **Publish application**: `dotnet publish Presentation/Nop.Web -c Release`

### Testing
- **Run all tests**: `dotnet test`
- **Run tests for specific project**: `dotnet test Tests/Nop.Tests/Nop.Tests.csproj`
- **Run tests with coverage**: `dotnet test --collect:"XPlat Code Coverage"`

### Database Migrations
- **Database operations are handled through FluentMigrator** - check Nop.Data project for migration files

## Architecture & Structure

### Core Architecture (Layered Architecture Pattern)

**Libraries** - Core business logic layer:
- **Nop.Core**: Domain entities, interfaces, caching, events, and core business objects (Order, Customer, etc.)
- **Nop.Data**: Data access layer using linq2db ORM, repositories, FluentMigrator for migrations
- **Nop.Services**: Business logic layer, services, validations, calculations (Business Access Layer)

**Presentation** - Web layer:
- **Nop.Web**: Main MVC web application (public store + admin area)
- **Nop.Web.Framework**: Shared web components, filters, attributes, extensions

**Plugins** - Extensibility layer:
- Self-contained plugin system with `plugin.json` descriptors
- Categories: Payment providers, Shipping providers, Tax providers, Widgets, etc.
- Each plugin is a separate .csproj with its own dependencies

**Tests** - Testing layer:
- **Nop.Tests**: NUnit-based test project with FluentAssertions and Moq

### Key Technologies & Patterns
- **Framework**: ASP.NET Core (.NET 9), Entity Framework alternatives (linq2db)
- **DI Container**: Autofac with built-in .NET DI
- **ORM**: linq2db (not Entity Framework)
- **Database Support**: SQL Server, MySQL, PostgreSQL
- **Caching**: Redis, SQL Server, in-memory
- **Testing**: NUnit, Moq, FluentAssertions
- **Architecture Patterns**: Repository pattern, Service layer pattern, Plugin architecture

### Plugin System
- Plugins are discovered at runtime via `plugin.json` files
- Each plugin has its own assembly and can include controllers, services, views
- Plugin assemblies are automatically cleaned up during build via MSBuild targets
- Plugins can extend discount rules, payments, shipping, widgets, authentication, etc.

### Database Architecture
- Uses FluentMigrator for schema migrations (not EF migrations)
- Supports multiple database providers through linq2db
- Connection strings and database configuration in appsettings.json

### Web Application Structure
- **Areas/Admin**: Administrative interface
- **Areas/Api**: Web API endpoints (if Web API plugin enabled)
- **Views**: Razor views for public store
- **Themes**: Support for multiple themes
- **App_Data**: Configuration, data protection keys, installation files

## Development Guidelines

### Plugin Development
- Each plugin must have a `plugin.json` with metadata
- Implement `IPlugin` interface for plugin lifecycle
- Use dependency injection for services
- Follow naming convention: `Nop.Plugin.[Category].[PluginName]`

### Service Layer Patterns
- All service methods should be async
- Use repository pattern for data access
- Implement caching at service layer when appropriate
- Follow single responsibility principle for services

### Database Considerations
- Uses linq2db, not Entity Framework
- All database operations should be async
- Use FluentMigrator for schema changes
- Support multiple database providers (SQL Server, MySQL, PostgreSQL)

### Performance & Scalability
- Supports web farms and load balancing
- Built-in caching support (Redis, SQL Server, Memory)
- All methods are async for better scalability
- Garbage collection is configured for server scenarios

## Important Notes

- **Multi-tenant**: Not multi-tenant by design but supports multiple stores
- **Plugin Loading**: Plugins are loaded dynamically and assemblies are cleaned automatically
- **Themes**: Theme system allows complete UI customization
- **Localization**: Full internationalization support
- **Security**: Multi-factor authentication support, follows security best practices
- **Docker Support**: Includes Dockerfile and docker-compose configurations
- **Documentation**: Extensive documentation available at docs.nopcommerce.com