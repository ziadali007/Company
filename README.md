# Company Management System

A full-stack web application built using **.NET Core** with a clean **3-tier architecture**, designed to help business owners efficiently manage departments, employees, and users.

## 🔧 Technologies Used

- **.NET Core 7**
- **ASP.NET Core MVC**
- **Entity Framework Core**
- **SQL Server**
- **Razor Views**
- **Bootstrap**
- **AJAX**
- **AutoMapper**
- **Dependency Injection**
- **External Login (Google & Facebook)**

## 📁 Architecture Overview

The solution follows a 3-Tier Architecture pattern:

### 1. Data Access Layer (DAL)
- Entities: `Department`, `Employee`, `User`
- Entity configurations using Fluent API
- EF Core Migrations
- `DbContext` for database access
- SQL Server as the backend

### 2. Business Logic Layer (BLL)
- Repository Pattern
- Unit of Work Pattern
- Handles core business logic
- Injected into the presentation layer via built-in **Dependency Injection**

### 3. Presentation Layer (PL)
- ASP.NET Core MVC
- Razor Views and Bootstrap for UI
- **Real-time Search using AJAX**
- **Full CRUD Operations**
- Uses **DTOs** to interact with the BLL
- **AutoMapper** for mapping between DTOs and entities
- External authentication via **Google** and **Facebook**

## ✨ Features

- 🔍 Real-time search for employees and departments using AJAX  
- ✅ Full CRUD functionality for managing departments, employees, and users  
- 🔐 Secure authentication with support for Google and Facebook logins  
- 📊 Clean and responsive UI built with Razor and Bootstrap  
- 📦 Scalable and maintainable code with proper layering and separation of concerns

## 🚀 Getting Started

### Prerequisites

- [.NET SDK 7+](https://dotnet.microsoft.com/en-us/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- Visual Studio or any C# IDE

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/ziadali007/Company.git
   cd Company
