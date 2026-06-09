<div align="center">

# 🏋️ Gym Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/en-us/sql-server/)
[![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-8.0-5C2D91?style=for-the-badge&logo=nuget&logoColor=white)](https://learn.microsoft.com/en-us/ef/core/)

*A comprehensive web-based application for managing gym operations — built with clean architecture, separation of concerns, and enterprise design patterns.*


</div>


## 🎯 Overview

**Gym Management System** is a full-stack web application designed to streamline gym operations by providing an intuitive interface for managing members, trainers, workout sessions, and membership plans. The system follows a **Three-Tier Architecture** with **Repository Pattern** and **Unit of Work** to ensure maintainability, scalability, and testability.

Whether you're a gym owner tracking member attendance, a manager assigning trainers to sessions, or an administrator managing subscription plans — this system handles it all from a single dashboard.

---

## ✨ Features

### 👤 Member Management
- Register new gym members with personal details and emergency contacts
- Track membership status, activation dates, and expiration
- View member attendance history and session participation
- Search and filter members by name, plan, or status

### 🏃 Trainer Management
- Add and manage trainer profiles with specializations
- Assign trainers to specific workout sessions and time slots
- Track trainer schedules and availability
- View trainer performance metrics and session counts

### 📅 Session Management
- Create, schedule, and manage workout sessions (Yoga, HIIT, CrossFit, etc.)
- Assign trainers and set capacity limits for each session
- Track real-time session enrollment and availability
- Manage recurring sessions and time slots

### 💳 Membership Plans
- Create flexible membership plans (Monthly, Quarterly, Annual)
- Define pricing, duration, and included features per plan
- Track plan subscriptions and renewal dates
- Generate revenue reports based on active subscriptions

### 📊 Dashboard & Reports
- Centralized dashboard with key metrics and KPIs
- Visual charts for member growth, revenue, and session popularity
- Quick-access widgets for recent activities and upcoming sessions
- Export reports for accounting and business analysis

---

## 🏗️ Architecture

The project follows a strict **Three-Tier Architecture** ensuring clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│                  GymManagementPL                         │
│              (Presentation Layer - MVC)                   │
│                                                          │
│   Controllers  │  Views  │  ViewModels  │  wwwroot       │
│   ─────────────────────────────────────────────────────  │
│   • Handles HTTP requests and responses                  │
│   • Renders Razor Views with Bootstrap UI                │
│   • Input validation and ViewModel mapping               │
└──────────────────────┬──────────────────────────────────┘
                       │ Dependency Injection
                       ▼
┌─────────────────────────────────────────────────────────┐
│                  GymManagementBLL                        │
│              (Business Logic Layer)                      │
│                                                          │
│   Services  │  DTOs  │  Interfaces  │  AutoMapper       │
│   ─────────────────────────────────────────────────────  │
│   • Business rules and validation logic                  │
│   • DTO ↔ Entity mapping with AutoMapper                 │
│   • Service interfaces for dependency injection           │
│   • Orchestrates data access through Unit of Work        │
└──────────────────────┬──────────────────────────────────┘
                       │ Dependency Injection
                       ▼
┌─────────────────────────────────────────────────────────┐
│                  GymManagementDAL                        │
│              (Data Access Layer)                         │
│                                                          │
│   DbContext  │  Entities  │  Repositories  │  Migrations │
│   ─────────────────────────────────────────────────────  │
│   • Entity Framework Core DbContext                      │
│   • Generic & Specific Repository implementations        │
│   • Unit of Work pattern for transaction management      │
│   • Database migrations and seed data                    │
└──────────────────────┬──────────────────────────────────┘
                       │
                       ▼
              ┌─────────────────┐
              │   SQL Server    │
              │   (Database)    │
              └─────────────────┘
```

### Data Flow

```
User Request → Controller → Service (BLL) → Repository (DAL) → DbContext → SQL Server
                                                                                    │
User Response ← View ← ViewModel ← DTO ← Service (BLL) ← Repository (DAL) ←──────┘
```

---

## 💻 Tech Stack

| Category | Technology | Version |
|----------|-----------|---------|
| **Backend Framework** | ASP.NET Core MVC | 8.0 |
| **Language** | C# | 12.0 |
| **ORM** | Entity Framework Core | 8.0 |
| **Database** | SQL Server | 2022 |
| **Object Mapping** | AutoMapper | 13.x |
| **Frontend Framework** | Bootstrap | 5.3 |
| **Validation** | Data Annotations + FluentValidation | — |
| **Architecture** | Three-Tier Architecture | — |
| **Design Patterns** | Repository Pattern, Unit of Work | — |

---

 If you found this project helpful, please give it a star!

</div>
