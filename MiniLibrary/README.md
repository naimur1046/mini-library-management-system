# Mini Library Management System

A comprehensive library management system built with .NET 9, implementing Clean Architecture principles with CQRS pattern, PostgreSQL database, and JWT authentication.

## Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Database Schema](#database-schema)
- [Authentication & Authorization](#authentication--authorization)
- [API Endpoints](#api-endpoints)
- [Recent Updates](#recent-updates)
- [Contributing](#contributing)

## Features

### Core Features

- **Book Management**
  - Add, update, delete, and search books
  - Track book availability and copies
  - ISBN-based unique identification
  - Category-based organization
  - Published year validation

- **Member Management**
  - Member registration and profile management
  - Email-based unique identification
  - Active/inactive status tracking
  - Soft delete functionality
  - Member activity tracking

- **Borrowing System**
  - Book checkout and return
  - Multiple books per borrow transaction
  - Due date tracking
  - Automatic availability management
  - Borrowing summary and history

- **User Management**
  - User registration and authentication
  - Role-based access control (Admin/User)
  - JWT token-based authentication
  - Secure password hashing with BCrypt
  - Initial admin seeding on first run

### Advanced Features

- **Audit Tracking**
  - Track who created each record (CreatedBy)
  - Track who modified each record (ModifiedBy)
  - Track who deleted each record (DeletedBy)
  - Timestamp tracking for all operations
  - Uses actual logged-in user email for all operations

- **Soft Delete**
  - All entities support soft delete
  - Deleted records are hidden from queries
  - Audit trail maintained for deletions

- **Data Validation**
  - Email uniqueness validation
  - ISBN uniqueness validation
  - Date validation (join date, published year)
  - Availability validation before borrowing

## Architecture

This project follows **Clean Architecture** principles with clear separation of concerns:

```
┌─────────────────────────────────────────────────────┐
│                   Presentation Layer                 │
│                   (MiniLibrary.API)                  │
│              Minimal APIs, Endpoints                 │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│                 Application Layer                    │
│              (MiniLibrary.Application)               │
│       Commands, Queries, Handlers, Interfaces       │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│                   Domain Layer                       │
│                (MiniLibrary.Domain)                  │
│            Entities, Errors, Enums                   │
└──────────────────────┬──────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────┐
│               Infrastructure Layer                   │
│             (MiniLibrary.Infrastructure)             │
│    Database, Auth, Interceptors, Configurations     │
└─────────────────────────────────────────────────────┘
```

### Design Patterns

- **CQRS** (Command Query Responsibility Segregation)
- **Mediator Pattern** (for command/query handling)
- **Repository Pattern** (via DbContext abstraction)
- **Result Pattern** (for error handling)
- **Specification Pattern** (for query filters)
- **Dependency Injection**

## Technology Stack

### Backend
- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9.0** - ORM
- **PostgreSQL** - Database
- **MediatR** - Mediator pattern implementation

### Security
- **JWT** - JSON Web Tokens for authentication
- **BCrypt.Net** - Password hashing
- **ASP.NET Core Identity** - User context management

### Logging
- **Serilog** - Structured logging

### Documentation
- **Swagger/OpenAPI** - API documentation

## Project Structure

```
MiniLibrary/
├── MiniLibrary.API/                    # Presentation Layer
│   ├── Endpoints/
│   │   ├── Authentication/             # Login, Register
│   │   ├── Books/                      # Book CRUD operations
│   │   ├── Members/                    # Member CRUD operations
│   │   └── Borrowings/                 # Borrow, Return, Summary
│   ├── Extensions/                     # Service extensions
│   ├── Infrastructure/                 # API infrastructure
│   └── Program.cs                      # Application entry point
│
├── MiniLibrary.Application/            # Application Layer
│   ├── Abstractions/
│   │   ├── Authentication/             # IUserContext, IPasswordHasher, ITokenProvider
│   │   ├── Data/                       # IApplicationDbContext
│   │   └── Messaging/                  # ICommand, IQuery, ICommandHandler
│   ├── Books/                          # Book use cases
│   ├── Members/                        # Member use cases
│   ├── Borrowings/                     # Borrowing use cases
│   └── Users/                          # User authentication use cases
│
├── MiniLibrary.Domain/                 # Domain Layer
│   ├── Entities/
│   │   ├── Books/                      # Book entity and errors
│   │   ├── Members/                    # Member entity and errors
│   │   ├── Borrows/                    # Borrow, BorrowItem entities
│   │   └── Users/                      # User entity and errors
│   └── Enums/                          # Role enum
│
├── MiniLibrary.Infrastructure/         # Infrastructure Layer
│   ├── Authentication/                 # UserContext, PasswordHasher, TokenProvider
│   ├── Database/
│   │   ├── Migrations/                 # EF Core migrations
│   │   ├── ApplicationDbContext.cs     # DbContext
│   │   ├── DatabaseSeeder.cs          # Initial data seeding
│   │   └── DatabaseInitializer.cs     # Database initialization
│   ├── Books/                          # Book entity configuration
│   ├── Members/                        # Member entity configuration
│   ├── Borrows/                        # Borrow entity configuration
│   ├── Users/                          # User entity configuration
│   ├── Interceptors/                   # EF Core interceptors
│   └── DependencyInjection.cs         # Service registration
│
└── MiniLibrary.SharedKernel/           # Shared Kernel
    ├── Entity.cs                       # Base entity
    ├── Result.cs                       # Result pattern
    ├── Error.cs                        # Error handling
    └── Interfaces/                     # Shared interfaces
```

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL 14+
- Visual Studio 2022 / JetBrains Rider / VS Code

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/mini-library-management-system.git
   cd mini-library-management-system/MiniLibrary
   ```

2. **Configure Database Connection**

   Update `appsettings.json` in `MiniLibrary.API` project:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=MiniLibrary;Username=postgres;Password=yourpassword"
     }
   }
   ```

3. **Run the Application**
   ```bash
   cd MiniLibrary.API
   dotnet run
   ```

   The application will:
   - Apply pending migrations automatically
   - Create the database if it doesn't exist
   - Seed the initial admin user
   - Start the API on `https://localhost:5001`

4. **Access Swagger Documentation**

   Navigate to: `https://localhost:5001/swagger`

### Initial Admin Credentials

On first run, an admin user is automatically created:

```
Email: admin@minilibrary.com
Password: Admin@123
```

**Important:** Change this password after first login in production!

## Database Schema

### Tables

- **users** - System users with roles
- **members** - Library members
- **books** - Book catalog
- **borrows** - Borrowing transactions
- **borrow_items** - Individual books in a borrow transaction

### Audit Fields (All Tables)

- `created_on_utc` - Creation timestamp
- `created_by` - Email of user who created the record
- `modified_on_utc` - Last modification timestamp
- `modified_by` - Email of user who last modified the record
- `deleted_on_utc` - Deletion timestamp
- `deleted_by` - Email of user who deleted the record
- `is_deleted` - Soft delete flag

## Authentication & Authorization

### JWT Configuration

Update `appsettings.json`:
```json
{
  "Jwt": {
    "Secret": "your-secret-key-min-32-characters-long",
    "Issuer": "MiniLibrary",
    "Audience": "MiniLibraryUsers",
    "ExpirationInMinutes": 60
  }
}
```

### User Roles

- **Admin** - Full system access, can register new users
- **User** - Standard access for library operations

### Protected Endpoints

- **Registration** - Requires Admin role
- **All other endpoints** - Require authentication

## API Endpoints

### Authentication

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/v1/authentication/register` | Register new user | Yes (Admin) |
| POST | `/api/v1/authentication/login` | User login | No |

### Books

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/v1/books` | Get all books | Yes |
| GET | `/api/v1/books/{id}` | Get book by ID | Yes |
| POST | `/api/v1/books` | Create new book | Yes |
| PUT | `/api/v1/books/{id}` | Update book | Yes |
| DELETE | `/api/v1/books/{id}` | Delete book | Yes |

### Members

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| GET | `/api/v1/members` | Get all members | Yes |
| GET | `/api/v1/members/{id}` | Get member by ID | Yes |
| POST | `/api/v1/members` | Create new member | Yes |
| PUT | `/api/v1/members/{id}` | Update member | Yes |
| DELETE | `/api/v1/members/{id}` | Delete member | Yes |

### Borrowings

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| POST | `/api/v1/borrowings` | Borrow books | Yes |
| POST | `/api/v1/borrowings/return` | Return book | Yes |
| GET | `/api/v1/borrowings/summary` | Get borrowing summary | Yes |

## Recent Updates

### Version 1.2.0 (Latest)

**Audit Tracking Enhancements**
- Replaced all hardcoded "System" values with actual logged-in user email
- Updated all command handlers to use `IUserContext.Email`:
  - CreateBorrowingsCommandHandler
  - UpdateMemberCommandHandler
  - ReturnBookCommandHandler
  - CreateBookCommandHandler
  - UpdateBookCommandHandler
  - DeleteBookCommandHandler
  - DeleteMemberCommandHandler

**Database Seeding**
- Implemented automatic database initialization on application startup
- Added `DatabaseSeeder` class for seeding initial admin user
- Added `DatabaseInitializer` extension method for database setup
- Automatic migration application on startup
- Initial admin user created if no users exist

**Authorization Improvements**
- Registration endpoint now requires Admin role
- Only authenticated admins can create new users
- Enhanced security for user management

**Entity Configuration Fixes**
- Added proper max length constraints to entity properties:
  - User.FullName (200 characters)
  - User.Email (100 characters)
  - Book.Title (200 characters)
  - Book.Author (150 characters)
  - Book.ISBN (20 characters)

**Migration Updates**
- Created `AddDatabaseSeeding` migration
- Resolved pending model changes
- Database schema now in sync with entity configurations

### Version 1.1.0

**Authentication & Authorization**
- Implemented JWT-based authentication
- Added role-based authorization (Admin/User)
- Secure password hashing with BCrypt
- User context for tracking logged-in users

**Audit Infrastructure**
- Added audit interceptor for automatic tracking
- Implemented IAuditableEntity interface
- Implemented ISoftDeletableEntity interface
- Added DateTimeProvider for consistent timestamps

### Version 1.0.0

**Initial Release**
- Clean Architecture implementation
- CQRS pattern with MediatR
- Basic CRUD operations for Books, Members, and Borrowings
- PostgreSQL database integration
- Swagger documentation
- Serilog logging

## API Usage Examples

### Login

```bash
POST /api/v1/authentication/login
Content-Type: application/json

{
  "email": "admin@minilibrary.com",
  "password": "Admin@123"
}
```

Response:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@minilibrary.com",
  "role": "Admin"
}
```

### Register New User (Admin only)

```bash
POST /api/v1/authentication/register
Authorization: Bearer {token}
Content-Type: application/json

{
  "fullName": "John Doe",
  "email": "john@example.com",
  "password": "SecurePass123!",
  "role": 1
}
```

### Create Book

```bash
POST /api/v1/books
Authorization: Bearer {token}
Content-Type: application/json

{
  "title": "Clean Architecture",
  "author": "Robert C. Martin",
  "isbn": "978-0134494166",
  "category": "Software Engineering",
  "copiesAvailable": 5,
  "publishedYear": 2017
}
```

### Borrow Books

```bash
POST /api/v1/borrowings
Authorization: Bearer {token}
Content-Type: application/json

{
  "memberId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "bookIds": [
    "1fa85f64-5717-4562-b3fc-2c963f66afa6",
    "2fa85f64-5717-4562-b3fc-2c963f66afa6"
  ],
  "borrowDate": "2025-11-23T00:00:00Z",
  "dueDate": "2025-12-07T00:00:00Z"
}
```

### Return Book

```bash
POST /api/v1/borrowings/return
Authorization: Bearer {token}
Content-Type: application/json

{
  "borrowId": "4fa85f64-5717-4562-b3fc-2c963f66afa6",
  "bookId": "1fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

## Development

### Running Migrations

```bash
# Add a new migration
dotnet ef migrations add MigrationName --project MiniLibrary.Infrastructure --startup-project MiniLibrary.API

# Apply migrations
dotnet ef database update --project MiniLibrary.Infrastructure --startup-project MiniLibrary.API

# Remove last migration
dotnet ef migrations remove --project MiniLibrary.Infrastructure --startup-project MiniLibrary.API
```

### Running Tests

```bash
dotnet test
```

### Code Quality

The project follows:
- Clean Code principles
- SOLID principles
- Domain-Driven Design (DDD)
- Clean Architecture patterns

## Error Handling

The API uses a consistent error response format:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Bad Request",
  "status": 400,
  "errors": {
    "Email": ["Email already exists"]
  }
}
```

Common HTTP Status Codes:
- `200 OK` - Successful GET/PUT request
- `201 Created` - Successful POST request
- `204 No Content` - Successful DELETE request
- `400 Bad Request` - Validation error
- `401 Unauthorized` - Authentication required
- `403 Forbidden` - Insufficient permissions
- `404 Not Found` - Resource not found
- `409 Conflict` - Duplicate resource

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contact

Your Name - [@yourtwitter](https://twitter.com/yourtwitter) - email@example.com

Project Link: [https://github.com/yourusername/mini-library-management-system](https://github.com/yourusername/mini-library-management-system)

## Acknowledgments

- Clean Architecture by Robert C. Martin
- Domain-Driven Design by Eric Evans
- .NET Core documentation
- Entity Framework Core documentation
