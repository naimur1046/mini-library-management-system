# Mini Library Management System

Library management system built with .NET 9.0, PostgreSQL, and Clean Architecture principles. This system provides comprehensive book management, member management, and borrowing tracking capabilities with JWT authentication.

## Table of Contents

- [Features](#Features)
- [Tech Stack](#Tech-Stack)
- [Prerequisites](#prerequisites)
- [Setup & Run Instructions](#setup--run-instructions)
- [Sample Login Credentials](#sample-login-credentials)
- [API Workflow Documentation](#api-workflow-documentation)
- [Project Structure](#project-structure)

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

## Technologies Used

- **.NET 9.0** - Application framework
- **PostgreSQL** - Database
- **Entity Framework Core 9.0** - ORM
- **JWT Bearer Authentication** - Security
- **Serilog** - Logging
- **Swagger/OpenAPI** - API documentation
- **Clean Architecture** - Project structure
- **CQRS Pattern** - Command/Query separation
- **Result Pattern** - Error handling

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

Creates a new user account.

- **Endpoint:** `POST /authentication/register`
- **Authorization:** None
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

Updates an existing book with partial update support. Only the fields provided in the request body will be updated.

- **Endpoint:** `PATCH /books/{id}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**
  | Parameter | Type | Description |
  | --------- | ---- | ----------------------------- |
  | `id` | GUID | Unique identifier of the book |

- **Request Body:**
  All fields are optional. Only the provided fields will be updated.

```json
{
  "title": "string (optional)",
  "author": "string (optional)",
  "isbn": "string (optional)",
  "category": "string (optional)",
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

#### 1. Create Member

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

#### 2. Update Member

Updates an existing member.

- **Endpoint:** `PUT /members/{id}`
- **Authorization:** UserOrAdmin policy required
- **Path Parameters:**
  - `id`: GUID - Member identifier
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

#### 3. Delete Member

Deletes a library member.

- **Endpoint:** `DELETE /members/{id}`
- **Authorization:** Required (Admin only)
- **Path Parameters:**

  - `id`: GUID - Member identifier

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

#### 2. Delete Borrowing

Deletes a borrowing record (returns books).

- **Endpoint:** `DELETE /borrowings/{id}`
- **Authorization:** Required (Admin only)
- **Path Parameters:**

  - `id`: GUID - Borrowing identifier

- **Response:** `204 No Content`

#### 3. Get Borrowing Summary

Retrieves borrowing statistics for a date range.

- **Endpoint:** `GET /borrowings/summary`
- **Authorization:** UserOrAdmin policy required
- **Query Parameters:**

  - `startDate`: DateTime - Start of the date range
  - `endDate`: DateTime - End of the date range

- **Response:** `200 OK`

```json
{
  "totalBorrowings": 0,
  "activeBorrowings": 0,
  "overdueBorrowings": 0,
  "returnedBorrowings": 0,
  "mostBorrowedBooks": [
    {
      "bookId": "guid",
      "title": "string",
      "borrowCount": 0
    }
  ]
}
```

---

## Development Notes

### Logging

Logs are written to:

- Console (real-time)
- File system: `__logs/log-{Date}.txt`

### CORS

The API is configured with an "AllowAll" CORS policy for development. Update this for production use.

### Swagger

Swagger UI is available only in Development mode. Access it at `/swagger` when running locally.
