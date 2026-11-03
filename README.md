# Lab 3 – User Authentication System

A full-stack user authentication application built with **ReactJS**, **ASP.NET Core Web API**, and **Microsoft SQL Server**.

## 1. Project Overview

This project demonstrates how to build a full-stack web application with user registration, login, database integration, and JWT-based authentication.

### Main Features

- User Registration
- User Login
- Password Hashing
- JWT Authentication
- Protected Routes
- User Session Management
- Logout
- Form Validation
- RESTful API Integration

## 2. Technologies Used

### Frontend
- ReactJS
- Vite
- JavaScript
- Axios
- React Router DOM
- React Context API
- Local Storage

### Backend
- ASP.NET Core Web API (.NET 8)
- Entity Framework Core 8
- C#
- JWT (JSON Web Token)
- PasswordHasher

### Database
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

## 3. Project Structure

```text
Lab3/
├── Lab3API/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Migrations/
│   ├── Program.cs
│   └── appsettings.json
│
├── lab3-frontend/
│   ├── src/
│   │   ├── api/
│   │   ├── components/
│   │   ├── context/
│   │   ├── pages/
│   │   ├── App.jsx
│   │   └── main.jsx
│   └── package.json
│
├── .gitignore
└── README.md
```

## 4. Database Configuration

Database name: `Lab3DB`

Main table: `Users`

| Column | Data Type | Description |
|---|---|---|
| Id | int | Primary Key |
| Email | nvarchar(450) | User email |
| PasswordHash | nvarchar(max) | Hashed password |

The database is managed using Entity Framework Core Migrations.

## 5. Backend Setup

Navigate to the backend directory:

```bash
cd Lab3API
```

Restore dependencies:

```bash
dotnet restore
```

Configure the SQL Server connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Lab3DB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Configure JWT secret using User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "Jwt:Key" "YOUR_SECRET_KEY"
```

Use a randomly generated secret of at least 32 bytes in production.

Apply database migrations:

```bash
dotnet ef database update
```

Run the backend:

```bash
dotnet run
```

Backend URL:

`http://localhost:5206`

Swagger API documentation:

`http://localhost:5206/swagger`

## 6. Frontend Setup

Open another terminal and navigate to the frontend directory:

```bash
cd lab3-frontend
```

Install dependencies:

```bash
npm install
npm install axios react-router-dom
```

Start the development server:

```bash
npm run dev
```

Open the local URL displayed by Vite, usually:

`http://localhost:5173`

## 7. API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | /api/auth/register | Register a new user |
| POST | /api/auth/login | Authenticate a user |
| GET | /api/auth/me | Get current authenticated user |

## 8. Authentication Workflow

1. A user submits registration information.
2. The backend validates the input.
3. The password is hashed before being stored in SQL Server.
4. The user logs in using their email and password.
5. The backend verifies the password hash.
6. A JWT token is generated after successful authentication.
7. The frontend stores the token in Local Storage.
8. Axios attaches the JWT token to authenticated API requests.
9. React Context manages the authentication state.
10. Protected Routes restrict access to authenticated users.

## 9. Security Notes

- Passwords are stored as hashes, not plain text.
- JWT secrets are stored using .NET User Secrets.
- Sensitive configuration files should not be committed to GitHub.
- Authenticated endpoints validate JWT tokens.
- Input validation is implemented on both frontend and backend.

## 10. Author

**Project:** Lab 3 – Authentication System

**Technologies:** ReactJS, ASP.NET Core, Entity Framework Core, SQL Server

---

*This project was developed for educational purposes as part of Lab 3.*