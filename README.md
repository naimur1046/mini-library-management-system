# Mini Library Management System – Backend

A secure, RESTful .NET 9 Web API for managing a small library, including books, members, borrow/return operations, and reporting — with JWT authentication and Swagger documentation.

## Features

- **Book Management**: CRUD operations with auto-status update (`Available` to `Not Available`)
- **Member Management**: CRUD with unique email validation
- **Borrowing System**:
  - Borrow multiple books in one transaction
  - Enforce: max **5 active borrows** per member
  - Enforce: **book availability** (`CopiesAvailable > 0`)
  - Simulated **2-second processing delay** on borrow/return
- **Reporting Endpoint**: Borrow summary by date range (total borrowed, returned, active records, most borrowed book)
- **JWT Authentication**: Login with crediential
- **Secure**: All endpoints (except login) require valid JWT token
- **ExtraOperation**: Soft delete support

---

## Tech Stack

- **Framework**: .NET 8 Web API
- **ORM**: Entity Framework Core
- **Database**: SQL Server (compatible with PostgreSQL with minor config change)
- **Authentication**: JWT Bearer Tokens
- **API Documentation**: Swagger UI + Postman Collection

---

## 📦 Setup & Run Instructions

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- SQL Server (LocalDB, Express, or Docker) **or** PostgreSQL
- (Optional) [Postman](https://www.postman.com/)

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/mini-library-api.git
cd MiniLibrary
```
