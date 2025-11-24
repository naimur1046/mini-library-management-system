# Mini Library Management System

A production-ready library management system built with .NET 9.0, PostgreSQL, and Clean Architecture principles. Features comprehensive book management, member management, and borrowing tracking with JWT authentication, CQRS pattern, and advanced error handling.

## Table of Contents

- [Features](#features)
- [Architecture & Design Patterns](#architecture--design-patterns)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Setup & Run Instructions](#setup--run-instructions)
- [Sample Login Credentials](#sample-login-credentials)
- [API Workflow Documentation](#api-workflow-documentation)
- [Development Notes](#development-notes)

## Features

### Core Functionality
- **Book Management**:
  - Full CRUD operations with automatic availability status updates
  - ISBN uniqueness validation
  - Cursor-based pagination for efficient data browsing
  - Advanced filtering by title, category, and ISBN
- **Member Management**:
  - Complete member lifecycle management
  - Email uniqueness validation
  - Soft delete support with audit trail
- **Borrowing System**:
  - Multi-book borrowing in a single transaction
  - Maximum **5 active borrows** per member (enforced at business logic level)
  - Real-time **book availability** checks (`CopiesAvailable > 0`)
  - Automatic copy count management
  - Book return functionality with availability restoration
  - Borrowing summary and statistics by member
  - **2-second simulated processing delay** for testing async operations

### Security & Authentication
- **JWT Authentication**: Token-based authentication with configurable expiration
- **Role-Based Authorization**: Admin and User roles with policy-based access control
- **Password Security**: BCrypt password hashing
- **Secure Endpoints**: All endpoints (except login) require valid JWT tokens

### Advanced Features
- **Audit Trail**: Automatic tracking of CreatedBy, ModifiedBy, and DeletedBy with timestamps
- **Soft Delete**: Logical deletion preserving data integrity and history
- **Correlation IDs**: Request tracking with unique identifiers for debugging
- **Result Pattern**: Railway-oriented programming for predictable error handling
- **Domain Events**: Event-driven architecture foundation
- **Auto-Seeding**: Default admin account creation on first run

---

## Architecture & Design Patterns

This project demonstrates professional software architecture principles:

### Clean Architecture (5 Layers)
- **Presentation Layer** (`MiniLibrary.API`): Minimal APIs, endpoints, middleware
- **Application Layer** (`MiniLibrary.Application`): Business logic, CQRS handlers, validators
- **Domain Layer** (`MiniLibrary.Domain`): Entities, business rules, domain events
- **Infrastructure Layer** (`MiniLibrary.Infrastructure`): Database, authentication, external services
- **Shared Kernel** (`MiniLibrary.SharedKernel`): Cross-cutting abstractions (Result, Error, Entity base classes)

### Design Patterns Implemented
- **CQRS (Command Query Responsibility Segregation)**: Separate read and write operations for better scalability
- **Decorator Pattern**: Cross-cutting concerns via behavior decorators
  - `ValidationDecorator`: FluentValidation rules before execution
  - `LoggingDecorator`: Structured logging with Serilog
  - `SimulatedDelayDecorator`: 2-second delay for testing
- **Result Pattern**: Type-safe error handling without exceptions
- **Repository Pattern**: Data access abstraction via `IApplicationDbContext`
- **Domain Events**: Event sourcing foundation for future features
- **Dependency Injection**: Auto-registration via Scrutor with interface scanning

---

## Tech Stack

### Core Technologies
- **.NET 9.0** - Modern application framework with Minimal APIs
- **PostgreSQL** - Relational database with advanced features
- **Entity Framework Core 9.0** - ORM with migrations and fluent configuration
- **C# 13** - Latest language features

### Security & Authentication
- **JWT Bearer Authentication** - Stateless token-based auth
- **BCrypt.Net-Next** - Secure password hashing

### Logging & Monitoring
- **Serilog 4.3.0** - Structured logging framework
- **Console + File Sinks** - Real-time and persistent logging
- **Correlation IDs** - Request tracking across log entries

### API & Documentation
- **Swagger/OpenAPI** - Interactive API documentation
- **Minimal APIs** - Lightweight endpoint definitions
- **Problem Details** - RFC 7807 standard error responses

### Development Tools
- **FluentValidation 12.1.0** - Declarative validation rules
- **Scrutor 6.1.0** - Assembly scanning and auto-registration
- **EF Core Tools** - Database migrations and scaffolding
- **Health Checks** - Application health monitoring

---

### Key Directories Explained

**Endpoints/** - Minimal API endpoints organized by feature (Authentication, Books, Members, Borrowings)

**Application/Behaviors/** - Decorator pattern implementations for cross-cutting concerns (validation, logging, delay)

**Domain/Entities/** - Business entities with rich domain logic and error definitions

**Infrastructure/Database/** - EF Core DbContext, migrations, configurations, and seeding logic

**SharedKernel/** - Reusable abstractions shared across projects (Result pattern, base classes)

---

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL 12+](https://www.postgresql.org/download/)

## Setup & Run Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/naimur1046/mini-library-management-system.git
cd MiniLibrary
```

### 2. Configure Database Connection

Update the connection string in `MiniLibrary.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=MiniLibraryDb;Username=postgres;Password=YourPassword"
  }
}
```

Replace `YourPassword` with your PostgreSQL password.

### 3. Configure JWT Settings

The JWT settings are pre-configured in `appsettings.json`. For production, update the secret key:

```json
{
  "JwtSettings": {
    "Secret": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
    "Issuer": "MiniLibrary",
    "Audience": "MiniLibraryUsers",
    "ExpirationInMinutes": 60
  }
}
```

### 4. Create Database

Create the PostgreSQL database:

```sql
CREATE DATABASE MiniLibraryDb;
```

### 5. Run Database Migrations

Navigate to the API project directory and run migrations:

```bash
cd MiniLibrary.API
dotnet ef database update
```

If Entity Framework tools are not installed, install them first:

```bash
dotnet tool install --global dotnet-ef
```

### 6. Build the Solution

```bash
dotnet build
```

### 7. Run the Application

```bash
cd MiniLibrary.API
dotnet run
```

The API will be available at:

- HTTPS: `https://localhost:5001`
- HTTP: `http://localhost:5000`
- Swagger UI: `https://localhost:5001/swagger` (Development mode only)

### 8. Access Swagger Documentation

Navigate to `https://localhost:5001/swagger` to explore and test the API endpoints interactively.

## Sample Login Credentials

### Creating User Accounts

The system seeds a default Admin account automatically if no users exist in the database.

| Field    | Value                   |
| -------- | ----------------------- |
| Email    | `admin@minilibrary.com` |
| Password | `Admin@123`             |
| Role     | `Admin`                 |

### Registering a Regular User

**POST** `/authentication/register`

```json
{
  "fullName": "Naimur Rahman",
  "email": "naimur@gmail.com",
  "password": "Naimur@123#",
  "role": 1
}
```

### User Roles

- `User = 1`: Regular user with read access and can create borrowings
- `Admin = 2`: Administrator with full access to all operations

### Logging In

**POST** `/authentication/login`

```json
{
  "email": "admin@minilibrary.com",
  "password": "Admin@123"
}
```

**Response:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

Use the returned token in the `Authorization` header for subsequent requests:

```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## API Workflow Documentation

### Base URL

```
https://localhost:5001
```

### Authentication Endpoints

#### 1. Register User

Creates a new user account. Only administrators can register new users.

- **Endpoint:** `POST /authentication/register`
- **Authorization:** AdminOnly (requires valid Admin JWT token)
- **Request Body:**

```json
{
  "fullName": "Naimur Rahman",
  "email": "naimur@gmail.com",
  "password": "Naimur@123#",
  "role": 1 or 2
}
```

- **Response:** `201 Created`

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

#### 2. Login

Authenticates a user and returns a JWT token.

- **Endpoint:** `POST /authentication/login`
- **Authorization:** None
- **Request Body:**

```json
{
  "email": "naimur@gmail.com",
  "password": "Naimur@123#"
}
```

- **Response:** `200 OK`

```json
{
  "token": "string"
}
```

---

### Books Endpoints

#### 1. Fetch Books (Paginated + Filterable)

Retrieves a paginated list of available books with optional filters and backward and forward navigation.

- **Endpoint:** `GET /books`
- **Authorization:** UserOrAdmin policy required
- **Query Parameters:**

- **Pagination**
  | Name | Type | Required | Description |
  | ------------ | -------- | -------- | ------------------------------------------------------------------------ |
  | `lastBookId` | `GUID` | No | Cursor value of the last book from the previous response. |
  | `size` | `int` | No | Page size. Default: 100. Max: 1000. |
  | `direction` | `string` | No | Pagination direction: `"Forward"` or `"Backward"`. Default: `"Forward"`. |

- **Filtering**
  | Name | Type | Required | Description |
  | ---------- | -------- | -------- | ------------------------------------ |
  | `title` | `string` | No | Filter books by title (contains). |
  | `category` | `string` | No | Filter books by category (contains). |
  | `isbn` | `string` | No | Filter books by ISBN (contains). |

- **Response:** `200 OK`

```json
{
  "books": [
    {
      "id": "guid",
      "title": "string",
      "author": "string",
      "isbn": "string",
      "category": "string",
      "copiesAvailable": 5,
      "publishedYear": 2020,
      "isAvailable": true,
      "createdOnUtc": "2025-01-01T10:00:00Z"
    }
  ],
  "hasMore": true
}
```

#### 2. Get Book by ID

Retrieves a specific book by its ID.

- **Endpoint:** `GET /books/{id}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**

| Name | Type   | Required | Description                        |
| ---- | ------ | -------- | ---------------------------------- |
| `id` | `GUID` | Yes      | The unique identifier of the book. |

- **Response:** `200 OK`

```json
{
  "id": "guid",
  "title": "string",
  "author": "string",
  "isbn": "string",
  "category": "string",
  "copiesAvailable": 0,
  "publishedYear": 0,
  "isAvailable": true,
  "createdOnUtc": "2025-01-01T10:00:00Z"
}
```

#### 3. Create Book

Creates a new book in the library.

- **Endpoint:** `POST /books`
- **Authorization:** UserOrAdmin policy required
- **Request Body:**

```json
{
  "title": "string",
  "author": "string",
  "isbn": "string",
  "category": "string",
  "copiesAvailable": 0,
  "publishedYear": 0
}
```

- **Response:** `201 Created`

```json
{
  "id": "guid"
}
```

#### 4. Update Book

Updates an existing book. All fields should be provided in the request body.

- **Endpoint:** `PUT /books/{id}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**
  | Parameter | Type | Description |
  | --------- | ---- | ----------------------------- |
  | `id` | GUID | Unique identifier of the book |

- **Request Body:**

```json
{
  "title": "string",
  "author": "string",
  "isbn": "string",
  "category": "string",
  "copiesAvailable": 0,
  "publishedYear": 0
}
```

- **Response:** `200 OK`

```json
{
  "id": "guid"
}
```

#### 5. Delete Book

Soft-deletes a book from the library.
The book is not permanently removed — instead

- IsDeleted is set to true
- IsAvailable is set to false

- **Endpoint:** `DELETE /books/{id}`
- **Authorization:** Required (Admin only)
- **Path Parameters:**

| Parameter | Type | Description                      |
| --------- | ---- | -------------------------------- |
| `id`      | GUID | Identifier of the book to delete |

- **Response:** `204 No Content`
  Book successfully deleted.

---

### Members Endpoints

#### 1. Get All Members

Retrieves a paginated list of all library members.

- **Endpoint:** `GET /members`
- **Authorization:** UserOrAdmin policy required
- **Query Parameters:**

| Name   | Type  | Required | Description                           |
| ------ | ----- | -------- | ------------------------------------- |
| `page` | `int` | No       | Page number. Default: 1.              |
| `size` | `int` | No       | Page size. Default: 10. Max: 100.     |

- **Response:** `200 OK`

```json
{
  "members": [
    {
      "id": "guid",
      "fullName": "string",
      "email": "string",
      "phone": "string",
      "joinDate": "2025-01-01T00:00:00Z",
      "isActive": true
    }
  ],
  "totalCount": 0,
  "page": 1,
  "pageSize": 10
}
```

#### 2. Get Member by ID

Retrieves a specific member by their ID.

- **Endpoint:** `GET /members/{id}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**

| Name | Type   | Required | Description                          |
| ---- | ------ | -------- | ------------------------------------ |
| `id` | `GUID` | Yes      | The unique identifier of the member. |

- **Response:** `200 OK`

```json
{
  "id": "guid",
  "fullName": "string",
  "email": "string",
  "phone": "string",
  "joinDate": "2025-01-01T00:00:00Z",
  "isActive": true
}
```

#### 3. Create Member

Creates a new library member.

- **Endpoint:** `POST /members`
- **Authorization:** UserOrAdmin policy required
- **Request Body:**

```json
{
  "fullName": "string",
  "email": "string",
  "phone": "string",
  "joinDate": "2025-01-01T00:00:00Z",
  "isActive": true
}
```

- **Response:** `201 Created`

```json
{
  "id": "guid"
}
```

#### 4. Update Member

Updates an existing member.

- **Endpoint:** `PUT /members/{id}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**

| Name | Type   | Required | Description           |
| ---- | ------ | -------- | --------------------- |
| `id` | `GUID` | Yes      | Member identifier     |

- **Request Body:**

```json
{
  "fullName": "string",
  "email": "string",
  "phone": "string",
  "joinDate": "2025-01-01T00:00:00Z",
  "isActive": true
}
```

- **Response:** `204 No Content`

#### 5. Delete Member

Soft-deletes a library member. The member is not permanently removed - instead, `IsDeleted` is set to `true`.

- **Endpoint:** `DELETE /members/{id}`
- **Authorization:** AdminOnly (requires Admin role)
- **Path Parameters:**

| Name | Type   | Required | Description       |
| ---- | ------ | -------- | ----------------- |
| `id` | `GUID` | Yes      | Member identifier |

- **Response:** `204 No Content`

---

### Borrowings Endpoints

#### 1. Create Borrowing

Creates a new borrowing record for multiple books.

- **Endpoint:** `POST /borrowings`
- **Authorization:** UserOrAdmin policy required
- **Request Body:**

```json
{
  "memberId": "guid",
  "borrowDate": "2025-01-01T00:00:00Z",
  "dueDate": "2025-01-15T00:00:00Z",
  "bookIds": ["guid1", "guid2"]
}
```

- **Response:** `201 Created`

```json
"guid"
```

#### 2. Return Book

Returns a borrowed book and updates its availability status.

- **Endpoint:** `POST /borrowings/{borrowId}/return/{bookId}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**

| Name       | Type   | Required | Description                         |
| ---------- | ------ | -------- | ----------------------------------- |
| `borrowId` | `GUID` | Yes      | The unique identifier of the borrow |
| `bookId`   | `GUID` | Yes      | The unique identifier of the book   |

- **Response:** `200 OK`

```json
{
  "id": "guid",
  "message": "Book returned successfully"
}
```

#### 3. Get Borrowing Summary

Retrieves borrowing statistics and details for a specific member.

- **Endpoint:** `GET /borrowings/summary`
- **Authorization:** UserOrAdmin policy required
- **Query Parameters:**

| Name       | Type   | Required | Description                                         |
| ---------- | ------ | -------- | --------------------------------------------------- |
| `memberId` | `GUID` | Yes      | The unique identifier of the member to get summary for |

- **Response:** `200 OK`

```json
{
  "memberId": "guid",
  "memberName": "string",
  "totalBorrowings": 0,
  "activeBorrowings": 0,
  "returnedBorrowings": 0,
  "borrowingHistory": [
    {
      "borrowId": "guid",
      "bookId": "guid",
      "bookTitle": "string",
      "borrowDate": "2025-01-01T00:00:00Z",
      "dueDate": "2025-01-15T00:00:00Z",
      "returnDate": "2025-01-10T00:00:00Z"
    }
  ]
}
```

---

## Development Notes

### Logging

The application uses Serilog for structured logging with the following features:

- **Console Sink**: Real-time log output during development
- **File Sink**: Daily rolling logs stored in `__logs/log-{Date}.txt`
- **Correlation IDs**: Each HTTP request gets a unique identifier for tracking across log entries
- **Request Logging**: Automatic logging of all HTTP requests with method, path, status code, and duration
- **Structured Data**: Logs include structured context (user ID, email, request path)

Log levels: Information (default), Warning, Error, Fatal

### Behavior Decorators

The application uses the decorator pattern to add cross-cutting concerns to all command and query handlers:

1. **ValidationDecorator** - Validates all commands/queries using FluentValidation before execution
2. **LoggingDecorator** - Logs the start and completion of all operations with execution time
3. **SimulatedDelayDecorator** - Adds a 2-second delay to all operations (for testing async UI behavior)

These decorators are automatically applied to all handlers via Scrutor's `TryDecorate` method.

### CQRS Pattern

The application separates read and write operations:

- **Commands**: Modify state (Create, Update, Delete) - return `Result<T>` or `Result`
- **Queries**: Read data (Get, GetById, GetSummary) - return `Result<TResponse>`

Each operation has its own:
- Request DTO (Command/Query)
- Handler (implements `ICommandHandler<T>` or `IQueryHandler<T, TResponse>`)
- Validator (FluentValidation rules)

### Result Pattern

All operations return `Result<T>` instead of throwing exceptions:

```csharp
// Success
Result<Book> result = Result.Success(book);

// Failure
Result<Book> result = Result.Failure<Book>(BookErrors.NotFound);
```

Error handling is explicit and type-safe. Errors include:
- `Code`: Error identifier
- `Description`: Human-readable message
- `Type`: Error category (NotFound, Validation, Conflict, etc.)

### Database

**Migrations**: EF Core Code-First migrations track schema changes
- Run `dotnet ef database update` to apply migrations
- Run `dotnet ef migrations add <name>` to create new migrations

**Seeding**: Automatic admin user creation on first startup
- Email: admin@minilibrary.com
- Password: Admin@123
- Role: Admin

**Soft Delete**: Entities implementing `ISoftDeletableEntity` are never physically deleted
- Query filters automatically exclude deleted entities
- Audit trail preserved for compliance

**Async/Await**: All database operations use async methods to avoid thread blocking
