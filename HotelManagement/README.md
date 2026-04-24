# HotelManagement

![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)

## 📑 Table of Contents

- [Description](#description)
- [Tech Stack](#tech-stack)
- [Quick Start](#quick-start)
- [Key Dependencies](#key-dependencies)
- [Screenshots](#screenshots)
- [Project Structure](#project-structure)
- [Development Setup](#development-setup)
- [Contributing](#contributing)

## 📝 Description

HotelManagement is a comprehensive administrative solution built with the .NET framework, designed to streamline hospitality operations and enhance guest service efficiency. This robust platform provides a centralized system for managing essential tasks such as room reservations, guest check-ins and check-outs, billing, and inventory tracking. By leveraging the scalability and security of .NET, the application ensures high-performance handling of daily hotel activities, offering staff an intuitive interface to optimize occupancy and improve overall operational workflow.

## 🛠️ Tech Stack

- 🔷 .NET

## ⚡ Quick Start

```bash

# Clone the repository
git clone <repository-url>

# Restore and run
dotnet restore && dotnet run

#If using nuget package manager console, execute
Update-Database -Context DatabaseContext
```

## 📦 Key Dependencies

```
Microsoft.AspNetCore.Identity.EntityFrameworkCore: 8.0.25
Microsoft.EntityFrameworkCore: 8.0.25
Microsoft.EntityFrameworkCore.SqlServer: 8.0.25
System.Configuration.ConfigurationManager: 10.0.5
```

## 📸 Screenshots

#### Login Page

![Login Page](./Website%20Demo/login.png)

#### Admin Dashboard

![Admin Dashboard](./Website%20Demo/admin.png)
#### Add Employee

![Add Employee](./Website%20Demo/Admin_employee.png)

#### Receptionist Dashboard

![Receptionist](./Website%20Demo/Receptionist.png)

#### Check in Customer

![Check In](./Website%20Demo/CheckIn_Customer.png)

  #### Cab Service

![Cab Service](./Website%20Demo/Cabservice.png)

## 📁 Project Structure

```
HotelManagement
├── HotelManagement.WebApp
│   ├── Application
│   │   ├── Dtos
│   │   ├── Facades
│   │   ├── Interfaces
│   │   └── Services
│   │       ├── Customers
│   │       ├── Drivers
│   │       ├── DropPickRequests
│   │       ├── Employees
│   │       ├── Rooms
│   │       └── Stays
│   ├── Controllers
│   ├── Domain
│   │   ├── Enums
│   │   └── Models
│   ├── Persistance
│   │   ├── DataSeeder
│   │   ├── DbContext
│   │   │   ├── AuthDbContext.cs
│   │   │   └── HotelDbContext.cs
│   │   ├── Interfaces
│   │   └── Repositories
│   ├── Program.cs
│   ├── ViewModels
│   │   ├── Customers
│   │   ├── DropPickRequests
│   │   ├── EmployeeFormViewComponent.cs
│   │   ├── Login
│   │   └── Stays
│   ├── Views
│   │   ├── Account
│   │   ├── AdminHome.cshtml
│   │   ├── Auth
│   │   ├── CabDrivers
│   │   ├── Customers
│   │   ├── DropPickRequests
│   │   ├── Employees
│   │   ├── Ho
│   │   ├── ReceptionistHome.cshtml
│   │   ├── Rooms
│   │   ├── Shared
│   │   │   ├── _Layout.cshtml
│   │   │   └── _Sidebar.cshtml
│   │   ├── Stays
│   │   ├── _ViewImports.cshtml
│   │   └── _ViewStart.cshtml
├── HotelManagement.sln
└── SRS - Hotel Management System.pdf

```

## 🛠️ Development Setup

### .NET Setup
1. Install [.NET SDK](https://dotnet.microsoft.com/)
2. Restore dependencies: `dotnet restore`
3. Build the project: `dotnet build`
4. Run the project: `dotnet run`