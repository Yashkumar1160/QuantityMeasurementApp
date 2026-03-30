# Quantity Measurement Application

A layered .NET 8 C# application built incrementally across 18 Use Cases — evolving from a simple `Feet` equality check into a full-stack system with multi-category measurement arithmetic, SQL persistence, a REST Web API, and JWT-secured authentication.

---

## Table of Contents

- [Project Overview](#project-overview)
- [Supported Measurement Types](#supported-measurement-types)
- [Solution Structure](#solution-structure)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Use Case Progress](#use-case-progress)
  - [UC1 – Feet Equality](#uc1--feet-equality)
  - [UC2 – Inches Equality](#uc2--inches-equality)
  - [UC3 – Generic QuantityLength with DRY Principle](#uc3--generic-quantitylength-with-dry-principle)
  - [UC4 – Extended Length Units](#uc4--extended-length-units)
  - [UC5 – Unit-to-Unit Conversion](#uc5--unit-to-unit-conversion)
  - [UC6 – Addition of Two Length Quantities](#uc6--addition-of-two-length-quantities)
  - [UC7 – Addition with Explicit Target Unit](#uc7--addition-with-explicit-target-unit)
  - [UC8 – Standalone Unit with Conversion Responsibility](#uc8--standalone-unit-with-conversion-responsibility)
  - [UC9 – Weight Measurement Support](#uc9--weight-measurement-support)
  - [UC10 – Generic Quantity with IMeasurable Interface](#uc10--generic-quantity-with-imeasurable-interface)
  - [UC11 – Volume Measurement Support](#uc11--volume-measurement-support)
  - [UC12 – Subtraction and Division Operations](#uc12--subtraction-and-division-operations)
  - [UC13 – Centralized Arithmetic Logic](#uc13--centralized-arithmetic-logic)
  - [UC14 – Temperature Measurement with Selective Arithmetic](#uc14--temperature-measurement-with-selective-arithmetic)
  - [UC15 – N-Tier Architecture and Service Layer](#uc15--n-tier-architecture-and-service-layer)
  - [UC16 – SQL Database Persistence](#uc16--sql-database-persistence)
  - [UC17 – ASP.NET Core Web API](#uc17--aspnet-core-web-api)
  - [UC18 – Authentication and Security](#uc18--authentication-and-security)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Configuration](#configuration)
- [Database Setup](#database-setup)
- [Running the Application](#running-the-application)
- [API Endpoints](#api-endpoints)
- [Running Tests](#running-tests)
- [Design Patterns Used](#design-patterns-used)
- [SOLID Principles Applied](#solid-principles-applied)

---

## Project Overview

The Quantity Measurement Application is a progressive C# project built use case by use case. Each UC introduces a small, focused feature on top of the previous one, keeping the codebase clean, testable, and extensible at every stage.

The project begins with basic value-object equality for `Feet`, grows through unit conversion and arithmetic, adds multiple measurement categories, refactors into a full N-Tier layered architecture with service and repository abstractions, persists data to SQL Server, exposes everything as a REST API, and finally secures the API with JWT authentication and BCrypt password hashing.

---

## Supported Measurement Types

| Category    | Units                        | Compare | Convert | Add | Subtract | Divide |
|-------------|------------------------------|---------|---------|-----|----------|--------|
| Length      | Feet, Inch, Yard, Centimeter | Yes     | Yes     | Yes | Yes      | Yes    |
| Weight      | Kilogram, Gram, Pound        | Yes     | Yes     | Yes | Yes      | Yes    |
| Volume      | Litre, Millilitre, Gallon    | Yes     | Yes     | Yes | Yes      | Yes    |
| Temperature | Celsius, Fahrenheit, Kelvin  | Yes     | Yes     | No  | No       | No     |

> Temperature arithmetic is intentionally blocked at the domain level. Attempting Add, Subtract, or Divide on temperature throws `NotSupportedException`.
---

## Architecture

The project evolves its architecture across use cases, starting as a simple two-project solution and growing into a full N-Tier layered system:

```
┌──────────────────────────────────────────────────────────────┐
│            Presentation / Entry Point Layer                   │
│   QuantityMeasurementApp (Console)          UC1–UC16         │
│   QuantityMeasurementWebApi (REST API)      UC17–UC18        │
└──────────────────────────┬───────────────────────────────────┘
                           │
┌──────────────────────────▼───────────────────────────────────┐
│                      Service Layer                             │
│   IQuantityMeasurementService / QuantityMeasurementServiceImpl│
│   IQuantityWebService / QuantityWebServiceImpl   (UC17+)     │
│   IAuthService / AuthService                     (UC18)      │
│   JwtService / EncryptionService / HashingService (UC18)     │
└──────────────────────────┬───────────────────────────────────┘
                           │
┌──────────────────────────▼───────────────────────────────────┐
│                    Business Layer                              │
│   Quantity<U> (Generic class)   IMeasurable (interface)      │
│   LengthMeasurementImpl         WeightMeasurementImpl        │
│   VolumeMeasurementImpl         TemperatureMeasurementImpl   │
│   QuantityMeasurementException  DatabaseException            │
└──────────────────────────┬───────────────────────────────────┘
                           │
┌──────────────────────────▼───────────────────────────────────┐
│                   Models / DTOs Layer                          │
│   QuantityDTO              QuantityMeasurementEntity          │
│   UserEntity               ArithmeticRequest / ConvertRequest │
│   AuthResponse             QuantityMeasurementResponseDTO     │
│   ErrorResponseDTO         Enums (LengthUnit, WeightUnit …)   │
└──────────────────────────┬───────────────────────────────────┘
                           │
┌──────────────────────────▼───────────────────────────────────┐
│                Repository / Data Access Layer                  │
│   Cache (JSON file)              ADO.NET (SQL Server)        │
│   EF Core (AppDbContext)         UserRepository              │
│   IQuantityMeasurementRepository IQuantityRecordRepository   │
└──────────────────────────────────────────────────────────────┘
```

---

## Tech Stack

| Technology             | Usage                                             |
|------------------------|---------------------------------------------------|
| C# / .NET 8            | Core language and runtime                         |
| ASP.NET Core Web API   | REST endpoints (UC17, UC18)                      |
| Entity Framework Core  | ORM, migrations, SQL Server (UC17, UC18)         |
| ADO.NET (SqlClient)    | Raw SQL, stored procedures (UC15, UC16)          |
| SQL Server (SQLEXPRESS)| Persistent relational storage                    |
| BCrypt.Net             | Password hashing, work factor 11 (UC18)          |
| System.IdentityModel   | JWT token generation and validation (UC18)        |
| AES-256 Encryption     | Sensitive data encryption utility (UC18)         |
| MSTest                 | Unit and integration testing framework           |
| Swagger / Swashbuckle  | API documentation and testing (UC17, UC18)       |
| System.Text.Json       | JSON serialization and config parsing            |

---

## Use Case Progress

### UC1 – Feet Equality

**Goal:** Establish the foundational value-object pattern for quantity measurements.

**Files:** `Feet.cs`, `FeetTests.cs`

**What was implemented:**
- Immutable `Feet` class with `private readonly double value`.
- Overridden `Equals()` with same-reference check, null check, type check, and value comparison via `CompareTo`.
- Overridden `GetHashCode()` consistent with `Equals`.
- MSTest suite: same value equals, different value not equals, null safe, same-reference short-circuit.

**Key concept:** Value-based equality — two objects are equal if their physical values match, regardless of memory reference.

---

### UC2 – Inches Equality

**Goal:** Add a second unit type demonstrating replication of the value-object pattern for `Inches`.

**Files:** `Inches.cs`, `InchesTests.cs`

**What was implemented:**
- Immutable `Inches` class with the same equality contract as `Feet`.
- `Feet` and `Inches` are completely separate types — no cross-unit comparison in this UC.
- Separate MSTest suite for `Inches` covering the same equality scenarios as UC1.

**Key concept:** Encapsulation and type safety — `Feet` and `Inches` are independent value objects.

---

### UC3 – Generic QuantityLength with DRY Principle

**Goal:** Eliminate duplication between `Feet` and `Inches` by introducing a single class with a unit enum.

**Files:** `LengthUnit.cs`, `QuantityLength.cs`, `QuantityLengthTests.cs`

**What was implemented:**
- `LengthUnit` enum: `Feet`, `Inch`.
- `QuantityLength` — single class replacing both `Feet` and `Inches`:
  - `private readonly double value` and `private readonly LengthUnit unit`.
  - `ConvertToBaseUnit()` normalizes all values to Feet for comparison.
  - `Equals()` compares base-unit values, enabling cross-unit equality: `1 Foot == 12 Inches`.
  - `GetHashCode()` based on base-unit value.

**Key concept:** DRY Principle — eliminate duplication by parameterizing behavior with a unit enum.

---

### UC4 – Extended Length Units

**Goal:** Prove the UC3 design scales by adding Yards and Centimeters with zero changes to the class.

**Files:** `LengthUnit.cs` (extended), `QuantityLengthTests.cs` (extended)

**What was implemented:**
- `LengthUnit` enum extended: `Feet`, `Inch`, `Yard`, `Centimeter`.
- Conversion factors added to `ConvertToBaseUnit()`:
  - Yard → `value × 3.0`
  - Centimeter → `value × 0.0328084`
- `QuantityLength` class unchanged.
- 32+ new test cases covering Yard and Centimeter cross-unit comparisons.

**Key concept:** Open/Closed Principle — open for extension (new units), closed for modification (existing class).

---

### UC5 – Unit-to-Unit Conversion

**Goal:** Add an explicit conversion API so callers can request a value expressed in any target unit.

**Files:** `QuantityLength.cs` (ConvertTo, Convert static), `QuantityLengthTests.cs`

**What was implemented:**
- Static `ConvertToBase(double value, LengthUnit unit)` — converts any value to Feet (base).
- Static `ConvertFromBase(double feet, LengthUnit target)` — converts Feet to any unit.
- Instance `ConvertTo(LengthUnit targetUnit)` — returns a new `QuantityLength` in the target unit.
- Immutability preserved — originals unchanged.
- Conversion formula: `result = ConvertFromBase(ConvertToBase(value, source), target)`.
- Tests: round-trip conversion accuracy, precision, all unit pair combinations.

---

### UC6 – Addition of Two Length Quantities

**Goal:** Support arithmetic addition between same-category quantities, even when units differ.

**Files:** `QuantityLength.cs` (Add method), `QuantityLengthTests.cs`

**What was implemented:**
- `Add(QuantityLength other)` — converts both to Feet, adds, converts result back to first operand's unit.
- Null operand validation throws `ArgumentException`.
- Result is a new immutable `QuantityLength`.
- Tests: same-unit, cross-unit, commutativity, identity with zero.

```
1 Feet + 12 Inches → 1 Feet + 1 Feet → 2 Feet
```

---

### UC7 – Addition with Explicit Target Unit

**Goal:** Allow the caller to specify any target unit for the addition result.

**Files:** `QuantityLength.cs` (Add overload), `QuantityLengthTests.cs`

**What was implemented:**
- `Add(QuantityLength second, LengthUnit targetUnit)` — result expressed in the requested unit.
- Converts both operands to Feet, adds, then converts to `targetUnit`.
- Validates `targetUnit` via `Enum.IsDefined`.
- Tests: explicit target produces correct values, commutativity under a fixed target, invalid target rejection.

```
Add(1 Feet, 12 Inches, Inch) → 24 Inches
Add(1 Feet, 12 Inches, Feet) → 2 Feet
```

---

### UC8 – Standalone Unit with Conversion Responsibility

**Goal:** Move conversion logic from `QuantityLength` into a dedicated static class, following Single Responsibility Principle.

**Files:** `ExtendedLengthUnit.cs`, `QuantityLength.cs` (refactored), `QuantityLengthTests.cs`

**What was implemented:**
- `ExtendedLengthUnit` static class:
  - `GetConversionFactor(LengthUnit unit)` — returns factor relative to Feet.
  - `ConvertToBase(double value, LengthUnit unit)` — delegates to factor.
  - `ConvertFromBase(double valueInFeet, LengthUnit target)` — inverse.
- `QuantityLength` now delegates all conversion calls to `ExtendedLengthUnit`.
- All public APIs remain unchanged — zero behavioral change.
- New dedicated test suite for `ExtendedLengthUnit` methods.

**Key concept:** Delegation pattern — unit conversion responsibility belongs to the unit abstraction, not the quantity class.

---

### UC9 – Weight Measurement Support

**Goal:** Add a second measurement category (Weight) to validate the design from UC8 scales.

**Files:** `WeightUnit.cs`, `ExtendedWeightUnit.cs`, `QuantityWeight.cs`, `QuantityWeightTests.cs`

**What was implemented:**
- `WeightUnit` enum: `Kilogram`, `Gram`, `Pound`.
- `ExtendedWeightUnit` static class with conversion factors:
  - Kilogram → `1.0` (base unit)
  - Gram → `0.001`
  - Pound → `0.453592`
- `QuantityWeight` class mirroring `QuantityLength` design with `Equals`, `ConvertTo`, `Add`, `Add(other, targetUnit)`.
- Cross-category prevention: `QuantityLength` and `QuantityWeight` are incomparable.
- Tests: cross-unit equality, conversion round-trips, addition with implicit and explicit target units.

---

### UC10 – Generic Quantity with IMeasurable Interface

**Goal:** Eliminate duplication between `QuantityLength` and `QuantityWeight` by introducing a single generic class driven by an interface.

**Files:** `IMeasurable.cs`, `Quantity.cs`, `LengthMeasurementImpl.cs`, `WeightMeasurementImpl.cs`, `QuantityMeasurementTests.cs`

**What was implemented:**

`IMeasurable` interface:
```csharp
double GetConversionFactor();
double ConvertToBaseUnit(double value);
double ConvertFromBaseUnit(double baseValue);
string GetUnitName();
```

`Quantity<U>` generic class where `U : IMeasurable`:
- Constructor rejects `NaN`, `Infinity`, and null units.
- `ConvertTo(U targetUnit)` — converts through base unit.
- `Add(Quantity<U> second)` — adds in base unit, result in first operand's unit.
- `Add(Quantity<U> second, U targetUnit)` — result in specified target unit.
- `Equals()` — base-unit comparison with `epsilon = 0.0001` floating-point tolerance.
- `GetHashCode()` — normalized to base unit.
- Runtime cross-category guard: `Unit.GetType() != second.Unit.GetType()` throws `ArgumentException`.

`LengthMeasurementImpl` and `WeightMeasurementImpl` implementing `IMeasurable`.

**Key concept:** Generic programming — one `Quantity<U>` class handles all measurement categories.

---

### UC11 – Volume Measurement Support

**Goal:** Add a third category (Volume) to validate the generic architecture requires zero changes to `Quantity<U>`.

**Files:** `VolumeUnit.cs`, `VolumeMeasurementImpl.cs`, `QuantityVolumeTests.cs`

**What was implemented:**
- `VolumeUnit` enum: `Litre`, `Millilitre`, `Gallon`.
- `VolumeMeasurementImpl` implementing `IMeasurable`:
  - Litre → `1.0` (base unit)
  - Millilitre → `0.001`
  - Gallon → `3.78541`
- `Quantity<U>` with `VolumeMeasurementImpl` — zero changes to generic class.
- Tests: equality, conversion, addition across all volume units.

```
Quantity(1.0, Litre) == Quantity(1000.0, Millilitre)       → true
Quantity(1.0, Gallon).ConvertTo(Litre)                     → 3.78541 Litre
Quantity(1.0, Litre).Add(Quantity(500.0, Millilitre))      → 1.5 Litre
```

---

### UC12 – Subtraction and Division Operations

**Goal:** Extend the generic arithmetic model with subtraction and scalar division.

**Files:** `Quantity.cs` (Subtract + Divide), `SubtractionDivisionTests.cs`

**What was implemented:**
- `Subtract(Quantity<U> second)` — base-unit subtraction, result in first operand's unit, rounded to 2 decimal places.
- `Subtract(Quantity<U> second, U targetUnit)` — result in specified target unit.
- `Divide(Quantity<U> second)` — returns dimensionless `double` scalar (no unit).
- Validation:
  - Null operand → `ArgumentException`
  - Cross-category → `ArgumentException`
  - Division by zero → `DivideByZeroException`
- Non-commutativity verified: `A − B ≠ B − A`, `A ÷ B ≠ B ÷ A`.

```
Quantity(10.0, Feet).Subtract(Quantity(6.0, Inch))              → 9.5 Feet
Quantity(5.0, Litre).Subtract(Quantity(2.0, Litre), Millilitre) → 3000.0 Millilitre
Quantity(24.0, Inch).Divide(Quantity(2.0, Feet))                → 1.0
```

---

### UC13 – Centralized Arithmetic Logic

**Goal:** Refactor UC12's arithmetic methods to eliminate duplication, applying the DRY principle with enum-based operation dispatch.

**Files:** `ArithmeticOperation.cs`, `Quantity.cs` (refactored), `IMeasurable.cs` (extended), `QuantityArithmeticRefactoringTests.cs`

**What was implemented:**
- `ArithmeticOperation` enum: `ADD`, `SUBTRACT`, `DIVIDE`.
- Private `ValidateArithmeticOperands(Quantity<U> second, U targetUnit, bool targetRequired)` — one validation method for all operations (null, cross-category, non-finite, missing target).
- Private `PerformBaseArithmetic(Quantity<U> second, ArithmeticOperation operation)` — converts both to base, dispatches via enum, returns base-unit result.
- All three public methods (`Add`, `Subtract`, `Divide`) delegate to these two helpers.
- `IMeasurable` extended with `SupportsArithmetic()` and `ValidateOperationSupport(string operation)` as preparation for UC14.
- Public API and behavior fully preserved — all UC12 tests continue to pass unmodified.
- `QuantityArithmeticRefactoringTests.cs` verifies consistent validation messages across all three operations.

**Key concept:** DRY through enum dispatch — one centralized arithmetic path replaces three near-identical implementations.

---

### UC14 – Temperature Measurement with Selective Arithmetic

**Goal:** Add temperature as a fourth category, supporting equality and conversion but blocking arithmetic operations by domain design.

**Files:** `TemperatureUnit.cs`, `TemperatureMeasurementImpl.cs`, `IMeasurable.cs` (updated), `Quantity.cs` (updated), `TemperatureQuantityTests.cs`

**What was implemented:**
- `TemperatureUnit` enum: `Celsius`, `Fahrenheit`, `Kelvin`.
- `TemperatureMeasurementImpl` implementing `IMeasurable` with non-linear conversion formulas (base unit: Celsius):
  - Fahrenheit: `°C = (°F − 32) × 5/9`
  - Kelvin: `°C = K − 273.15`
- `IMeasurable` interface extended:
  - `bool SupportsArithmetic()` — returns `false` for temperature, `true` for all other categories.
  - `void ValidateOperationSupport(string operation)` — throws `NotSupportedException` for temperature.
- `Quantity<U>.PerformBaseArithmetic()` calls `ValidateOperationSupport()` before execution.
- Temperature equality (`0°C == 32°F == 273.15K`) and conversion work correctly.
- Temperature `Add`, `Subtract`, `Divide` throw `NotSupportedException`.
- All UC1–UC13 tests continue to pass.

**Test coverage (`TemperatureQuantityTests.cs`):**
- Equality across all three temperature units.
- Conversion accuracy and round-trip.
- Arithmetic rejection verification for all three operations.
- Cross-category prevention.

---

### UC15 – N-Tier Architecture and Service Layer

**Goal:** Restructure the codebase into a clean 5-project N-Tier architecture with service abstraction, DTOs, repository pattern, and proper layer separation.

**Projects added:** `QuantityMeasurementAppBusiness`, `QuantityMeasurementAppModels`, `QuantityMeasurementAppRepositories`, `QuantityMeasurementAppServices`

**What was implemented:**

Service interface (`IQuantityMeasurementService`):
```csharp
QuantityDTO Add(QuantityDTO first, QuantityDTO second, string targetUnit);
QuantityDTO Subtract(QuantityDTO first, QuantityDTO second, string targetUnit);
double Divide(QuantityDTO first, QuantityDTO second);
bool Compare(QuantityDTO first, QuantityDTO second);
QuantityDTO Convert(QuantityDTO quantity, string targetUnit);
```

Repository interface (`IQuantityMeasurementRepository`):
```csharp
void Save(QuantityMeasurementEntity entity);
List<QuantityMeasurementEntity> GetAll();
```

Repository implementations:
- `QuantityMeasurementCacheRepository` — Singleton, in-memory list backed by JSON file on disk.
- `QuantityMeasurementDatabaseRepository` — ADO.NET `SqlConnection` with `ConnectionPool`.

Application bootstrap (`QuantityMeasurementApp.cs`):
- Singleton pattern with double-checked locking.
- Factory construction of repository, service, controller, and menu.

DTO layer:
- `QuantityDTO` — carries `Value`, `UnitName`, `MeasurementType` between layers.
- `QuantityMeasurementEntity` — persistence record with operation, operands, result, and error state.

Test coverage (`QuantityMeasurementNtierTests.cs`):
- `MockRepository` and `MockService` for controller-layer isolation.
- N-Tier flow: request → service → repository → entity saved.
- All UC14 tests pass in the new layered structure.

---

### UC16 – SQL Database Persistence

**Goal:** Replace in-memory/JSON cache with SQL Server persistence via ADO.NET, stored procedures, connection pooling, and config-driven repository selection with automatic fallback.

**Files:** `QuantityMeasurementAppSchema.sql`, extended `IQuantityMeasurementRepository`, `QuantityMeasurementDatabaseRepository.cs`, `ConnectionPool.cs`, `ApplicationConfig.cs`, `QuantityMeasurementDBTests.cs`

**What was implemented:**

Database schema (`QuantityMeasurementAppSchema.sql`):
```sql
CREATE TABLE quantity_measurements (
    id               INT           IDENTITY(1,1) PRIMARY KEY,
    operation        NVARCHAR(50)  NOT NULL,
    first_value      FLOAT         NOT NULL DEFAULT 0,
    first_unit       NVARCHAR(50)  NULL,
    second_value     FLOAT         NOT NULL DEFAULT 0,
    second_unit      NVARCHAR(50)  NULL,
    result_value     FLOAT         NOT NULL DEFAULT 0,
    measurement_type NVARCHAR(50)  NULL,
    is_error         BIT           NOT NULL DEFAULT 0,
    error_message    NVARCHAR(500) NULL,
    created_at       DATETIME      NOT NULL DEFAULT GETDATE()
);
```

Stored procedures: `sp_SaveMeasurement`, `sp_GetAllMeasurements`, `sp_GetMeasurementsByOperation`, `sp_GetMeasurementsByType`, `sp_GetTotalCount`, `sp_DeleteAllMeasurements`.

Extended `IQuantityMeasurementRepository`:
```csharp
void Save(QuantityMeasurementEntity entity);
List<QuantityMeasurementEntity> GetAll();
List<QuantityMeasurementEntity> GetByOperation(string operation);
List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);
int GetTotalCount();
void DeleteAll();
string GetPoolStatistics();
void ReleaseResources();
```

Config-driven repository selection (`appsettings.json`):
```json
{
  "Database": {
    "Provider": "sqlserver",
    "ConnectionString": "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementAppDB;...",
    "PoolSize": 5,
    "ConnectionTimeout": 30
  },
  "Repository": { "Type": "database" }
}
```

Auto-fallback: If SQL Server is unreachable, the app automatically switches to `QuantityMeasurementCacheRepository` and logs the reason at startup.

Lifecycle methods in `Program.cs`:
```csharp
app.Start();                    // Run interactive menu
app.ReportAllMeasurements();    // Print full history after menu exits
app.CloseResources();           // Release DB connections on shutdown
```

Test coverage (`QuantityMeasurementDBTests.cs`):
- `ApplicationConfig` loads connection string and pool size from `appsettings.json`.
- Fallback to defaults when config keys are missing.
- `ConnectionPool` acquire/return lifecycle.
- `QuantityMeasurementDatabaseRepository` save and retrieval.

---

### UC17 – ASP.NET Core Web API

**Goal:** Expose all measurement operations as REST HTTP endpoints with EF Core ORM, auto-migration on startup, Swagger UI, and health check.

**New project:** `QuantityMeasurementWebApi`

**What was implemented:**

EF Core `AppDbContext` maps `QuantityMeasurementEntity` to `quantity_measurements` with column-level mappings and indexes on `operation`, `measurement_type`, and `is_error`. Migration `20260325105353_InitialCreate` creates the table.

New repository interface (`IQuantityRecordRepository`):
```csharp
void Save(QuantityMeasurementEntity entity);
List<QuantityMeasurementEntity> GetAll();
List<QuantityMeasurementEntity> GetByOperation(string operation);
List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType);
List<QuantityMeasurementEntity> GetErrorHistory();
int GetOperationCount(string operation);
List<QuantityMeasurementEntity> GetByCreatedAfter(DateTime date);
```

New web service interface (`IQuantityWebService`):
```csharp
QuantityMeasurementResponseDTO Compare(QuantityInputRequest request);
QuantityMeasurementResponseDTO Convert(ConvertRequest request);
QuantityMeasurementResponseDTO Add(ArithmeticRequest request);
QuantityMeasurementResponseDTO Subtract(ArithmeticRequest request);
QuantityMeasurementResponseDTO Divide(QuantityInputRequest request);
List<QuantityMeasurementResponseDTO> GetHistoryByOperation(string operation);
List<QuantityMeasurementResponseDTO> GetHistoryByType(string measurementType);
List<QuantityMeasurementResponseDTO> GetErrorHistory();
int GetOperationCount(string operation);
```

REST Endpoints (`/api/v1/quantities`):

| Method | Route | Description |
|--------|-------|-------------|
| POST | `/api/v1/quantities/compare` | Compare two quantities |
| POST | `/api/v1/quantities/convert` | Convert to a different unit |
| POST | `/api/v1/quantities/add` | Add two quantities |
| POST | `/api/v1/quantities/subtract` | Subtract one from another |
| POST | `/api/v1/quantities/divide` | Divide one by another |
| GET | `/api/v1/quantities/history/operation/{op}` | History by operation type |
| GET | `/api/v1/quantities/history/type/{type}` | History by measurement type |
| GET | `/api/v1/quantities/history/errored` | All error records |
| GET | `/api/v1/quantities/count/{operation}` | Count of successful operations |
| GET | `/health` | Health check |

`GlobalExceptionHandler` middleware (IExceptionFilter):
- `QuantityMeasurementException` / `ArgumentException` / `NotSupportedException` → `400 Bad Request`
- `UnauthorizedAccessException` → `401 Unauthorized`
- `InvalidOperationException` → `409 Conflict`
- Unhandled → `500 Internal Server Error`

`Program.cs` wires up: `AppDbContext`, DI for all services and repositories, `GlobalExceptionHandler` filter, Swagger with XML comments, health checks, and `db.Database.Migrate()` on startup.

---

### UC18 – Authentication and Security

**Goal:** Secure the Web API with JWT authentication, BCrypt password hashing, AES-256 encryption, and per-user data isolation.

**What was implemented:**

New entities:
- `UserEntity` — `users` table: `id`, `email`, `name`, `password_hash`, `created_at`, `last_login_at`.
- `QuantityMeasurementEntity` extended with `user_id` foreign key.

New EF Core migrations:
- `20260329161739_InitialCreate` — Creates `quantity_measurements` and `users` tables.
- `20260330054655_AddUserIdToQuantityMeasurements` — Adds `user_id` column with index.
- `20260330085236_RemoveUserIdFromUsers` — Schema cleanup.

New services:

| Service | Responsibility |
|---------|----------------|
| `AuthService` | BCrypt `Register`, BCrypt `Verify` on `Login`, issues JWT |
| `JwtService` | HMAC-SHA256 signed JWT with `userId`, `email`, `name` claims |
| `EncryptionService` | AES-256 Encrypt/Decrypt with random IV per operation |
| `HashingService` | BCrypt (work factor 11) + SHA-256 utility method |

New Auth endpoints (`/api/v1/auth`):

| Method | Route | Auth Required | Description |
|--------|-------|---------------|-------------|
| POST | `/api/v1/auth/register` | No | Register a new user |
| POST | `/api/v1/auth/login` | No | Login and receive JWT |
| GET | `/api/v1/auth/ping` | No | Auth controller health check |

All `/api/v1/quantities/*` endpoints now require: `Authorization: Bearer <token>`

Per-user data isolation: `GetCurrentUserId()` extracts the `userId` claim from the validated JWT. All repository queries are scoped to the authenticated user's ID. The console app passes `userId = 0` to retrieve all records without authentication.

JWT configuration (`appsettings.json`):
```json
{
  "Jwt": {
    "SecretKey": "replace-with-a-strong-secret-key",
    "Issuer": "QuantityMeasurementAPI",
    "Audience": "QuantityMeasurementClient",
    "ExpiryMinutes": "480"
  },
  "Encryption": {
    "Key": "replace-with-a-32-byte-aes-key"
  }
}
```

Swagger configured with Bearer auth — paste the JWT in the Swagger UI `Authorize` dialog to test protected endpoints.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or SQL Server Express (for UC16 ADO.NET and UC17/UC18 EF Core)
- Visual Studio 2022 or VS Code with C# extension (optional)

---

## Getting Started

```bash
# Clone the repository
git clone <repository-url>
cd QuantityMeasurementApp

# Restore dependencies
dotnet restore UC-3.sln

# Build the solution
dotnet build UC-3.sln
```

---

## Configuration

### Web API (`QuantityMeasurementWebApi/appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementAppDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "SecretKey": "replace-with-a-strong-secret-key",
    "Issuer": "QuantityMeasurementAPI",
    "Audience": "QuantityMeasurementClient",
    "ExpiryMinutes": "480"
  },
  "Encryption": {
    "Key": "replace-with-a-32-byte-aes-key"
  }
}
```

> **Important:** Replace JWT secret and AES key with secure values before any non-local deployment. Never commit real secrets to source control.

### Console App (`QuantityMeasurementApp/appsettings.json`)

```json
{
  "Database": {
    "Provider": "sqlserver",
    "ConnectionString": "Server=localhost\\SQLEXPRESS;Database=QuantityMeasurementAppDB;Trusted_Connection=True;TrustServerCertificate=True;",
    "PoolSize": 5,
    "ConnectionTimeout": 30
  },
  "Repository": {
    "Type": "database"
  }
}
```

Set `Repository.Type` to `"cache"` to skip SQL Server and use in-memory persistence with JSON file backup.

---

## Database Setup

### Option A — SQL Script (UC16 Console / ADO.NET Mode)

Run `QuantityMeasurementAppSchema.sql` in SSMS:

```sql
CREATE DATABASE QuantityMeasurementAppDB;
USE QuantityMeasurementAppDB;
-- Script creates the table and all stored procedures
```

### Option B — EF Core Migrations (UC17 / UC18 Web API Mode)

The Web API applies all pending migrations automatically on startup via `db.Database.Migrate()`.

Migrations applied in order:
1. `20260329161739_InitialCreate` — Creates `quantity_measurements` and `users` tables.
2. `20260330054655_AddUserIdToQuantityMeasurements` — Adds `user_id` column with index.
3. `20260330085236_RemoveUserIdFromUsers` — Schema cleanup.

To apply manually:
```bash
dotnet ef database update --project QuantityMeasurementAppRepositories --startup-project QuantityMeasurementWebApi
```

---

## Running the Application

### Run the Web API (UC17 / UC18)

```bash
dotnet run --project QuantityMeasurementWebApi
```

After startup:
- Swagger UI: `https://localhost:{port}/swagger`
- Health check: `GET https://localhost:{port}/health`

### Run the Console App (UC1–UC16)

```bash
dotnet run --project QuantityMeasurementApp
```

The console app reads `Repository.Type` from `appsettings.json`, tries SQL Server, auto-falls back to cache if unavailable, runs the interactive menu, prints measurement history after exit, and releases connections on shutdown.

---

## API Endpoints

### Authentication (no token required)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/auth/register` | Register a new user |
| POST | `/api/v1/auth/login` | Login and receive a JWT |
| GET | `/api/v1/auth/ping` | Auth controller health check |

**Register body:**
```json
{ "name": "Yash Kumar", "email": "yash@gmail.com", "password": "mypassword123" }
```

**Login body:**
```json
{ "email": "yash@gmail.com", "password": "mypassword123" }
```

Use the returned `token` as: `Authorization: Bearer <token>`

### Quantity Operations (JWT required)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/quantities/compare` | Compare two quantities |
| POST | `/api/v1/quantities/convert` | Convert to a different unit |
| POST | `/api/v1/quantities/add` | Add two quantities |
| POST | `/api/v1/quantities/subtract` | Subtract one from another |
| POST | `/api/v1/quantities/divide` | Divide one by another |
| GET | `/api/v1/quantities/history/operation/{op}` | History by operation type |
| GET | `/api/v1/quantities/history/type/{type}` | History by measurement type |
| GET | `/api/v1/quantities/history/errored` | All error records |
| GET | `/api/v1/quantities/count/{operation}` | Count of successful operations |
| GET | `/api/v1/quantities/me` | Current user info from JWT claims |
| GET | `/health` | Application health check |

**Example — Compare:**
```json
{
  "thisQuantityDTO": { "value": 1.0, "unitName": "Feet", "measurementType": "Length" },
  "thatQuantityDTO": { "value": 12.0, "unitName": "Inch", "measurementType": "Length" }
}
```

**Example — Add:**
```json
{
  "thisQuantityDTO": { "value": 1.0, "unitName": "Feet", "measurementType": "Length" },
  "thatQuantityDTO": { "value": 12.0, "unitName": "Inch", "measurementType": "Length" },
  "targetUnit": "Feet"
}
```

**Example — Convert:**
```json
{
  "thisQuantityDTO": { "value": 1.0, "unitName": "Feet", "measurementType": "Length" },
  "targetUnit": "Inch"
}
```

---

## Running Tests

```bash
dotnet test UC-3.sln
```

### Test Coverage by UC

| Test File | UC Coverage | What Is Tested |
|-----------|-------------|----------------|
| `FeetTests.cs` | UC1 | Feet value-object equality |
| `InchesTests.cs` | UC2 | Inches value-object equality |
| `QuantityLengthTests.cs` | UC3–UC8 | Cross-unit equality, conversion, addition, subtraction |
| `QuantityWeightTests.cs` | UC9–UC13 | Weight operations across all units |
| `QuantityMeasurementTests.cs` | UC10–UC14 | Generic Quantity<U> across all categories |
| `QuantityVolumeTests.cs` | UC11–UC14 | Volume operations across Litre, Millilitre, Gallon |
| `SubtractionDivisionTests.cs` | UC12–UC14 | Subtraction and division with explicit targets |
| `QuantityArithmeticRefactoringTests.cs` | UC13–UC14 | DRY arithmetic: consistent validation and dispatch |
| `TemperatureQuantityTests.cs` | UC14 | Temperature equality, conversion, arithmetic rejection |
| `QuantityMeasurementNtierTests.cs` | UC15 | N-Tier flow with MockRepository and MockService |
| `QuantityMeasurementDBTests.cs` | UC16 | ApplicationConfig, ConnectionPool, database repository |

---

## Design Patterns Used

| Pattern | Where Applied |
|---------|---------------|
| Singleton | `QuantityMeasurementApp`, `QuantityMeasurementCacheRepository`, `ConnectionPool` |
| Factory | Repository, service, controller construction in app bootstrap |
| Repository | `IQuantityMeasurementRepository`, `IQuantityRecordRepository`, `IUserRepository` |
| Strategy | `ArithmeticOperation` enum dispatch in `PerformBaseArithmetic()` |
| Template Method | `Quantity<U>` generic class with `IMeasurable` contract |
| Delegation | `QuantityLength` delegates conversion to `ExtendedLengthUnit` (UC8) |
| DTO | `QuantityDTO`, `QuantityMeasurementResponseDTO`, `AuthResponse`, `ErrorResponseDTO` |
| Middleware | `GlobalExceptionHandler` as IExceptionFilter in the API pipeline |

---

## SOLID Principles Applied

| Principle | How It Is Applied |
|-----------|-------------------|
| Single Responsibility (SRP) | `QuantityLength` handles quantity math; `ExtendedLengthUnit` handles conversion logic; service orchestrates operations; repository handles persistence |
| Open/Closed (OCP) | New units added by extending enums and adding an `IMeasurable` implementation — `Quantity<U>` is never modified |
| Liskov Substitution (LSP) | All `IMeasurable` implementations are fully substitutable in `Quantity<U>`; temperature's `NotSupportedException` is explicit and meaningful |
| Interface Segregation (ISP) | `IQuantityMeasurementService` for business operations; `IQuantityRecordRepository` for data access; `IAuthService` for authentication — no class implements unused members |
| Dependency Inversion (DIP) | Controllers depend on service interfaces; services depend on repository interfaces; concrete classes are never directly coupled across layers |

---

## Notes

- Temperature arithmetic (Add, Subtract, Divide) is blocked at the domain level by design, not a limitation.
- Keep JWT secret key and AES encryption key environment-specific. Never commit real production secrets to source control.
- The console app does not require authentication. Records saved by the console are stored with `userId = 0`.
- If SQL Server is unreachable at console startup, the app automatically switches to cache mode and saves data to a local JSON file.
- All Web API error responses follow a consistent `ErrorResponseDTO` shape: `{ Timestamp, Status, Error, Message, Path }`.
