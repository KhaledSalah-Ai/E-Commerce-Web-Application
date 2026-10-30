# 🛒 NovaCart API

> A professional, secure, and scalable E-commerce Back-End API built with **ASP.NET Core 8**, following **Clean Architecture** principles and implementing enterprise-level design patterns.

---

## 🏗️ Architecture & Design Patterns

The project strictly follows **Clean Architecture** to ensure high maintainability, testability, and separation of concerns:
- **`Domain`**: Contains enterprise entities, business logic rules, and core models.
- **`Application`**: Contains DTOs, AutoMapper profiles, interfaces, and business application logic.
- **`Infrastructure`**: Handles database contexts, Entity Framework Core configurations, and repository implementations.
- **`API`**: Presentation layer containing controllers, middleware, and dependency injection setup.

### 🛠️ Design Patterns Used:
- **Repository Pattern & Unit of Work**: To abstract data access operations and maintain database transaction consistency.
- **DTO Pattern**: For secure and decoupled data transfer between layers.

---

## 🚀 Tech Stack & Libraries

- **Framework**: .NET 8 (C#)
- **Database & ORM**: SQL Server, Entity Framework Core (EF Core)
- **Authentication & Security**: ASP.NET Core Identity & **JWT Bearer Token**
- **Mapping**: AutoMapper
- **Documentation**: Swagger / OpenAPI

---

## ✨ Features

- **User Authentication**: Secure Register and Login endpoints with JWT token generation and Role-based readiness.
- **Category Management**: Full CRUD operations for product categories.
- **Product Management**: Full CRUD operations with automatic foreign key validation and category mapping.
- **Smart Order Management**: 
  - Automated stock checking and deduction.
  - Precise price calculation directly from the database (prevents price tampering).
  - Order status updates and safe order cancellation with automatic stock restoration.
- **Security**: Protected endpoints using `[Authorize]` middleware.

---

## ⚙️ Getting Started & Installation

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB or SQL Server Express)

### Steps to Run Locally

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/YOUR_USERNAME/NovaCart.git](https://github.com/YOUR_USERNAME/NovaCart.git)
   cd NovaCart
