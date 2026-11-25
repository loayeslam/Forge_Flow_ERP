# Forge_Flow_ERP
ERP System built with ASP.NET Core Web API, Entity Framework Core, and SQL Server 
# ForgeFlow ERP

### A Modular Monolith ERP Backend Built with ASP.NET Core

ForgeFlow ERP is a backend project designed to simulate core business operations within a manufacturing and inventory management environment.

The system follows the **Modular Monolith Architecture**, organizing business capabilities into independent modules while maintaining a single deployable ASP.NET Core application.

The project focuses on clean code organization, business logic separation, data consistency, and building a scalable foundation for future ERP capabilities.

---

## Architecture

ForgeFlow follows a Modular Monolith approach.

Each business module has its own organization of:

- Controllers – Handle HTTP requests and responses.
- Services – Contain business logic and application operations.
- DTOs – Control data exchange between API clients and the application.
- Models – Represent domain entities.

**Current architecture:**

```text
ForgeFlow ERP
│
├── Modules
│   ├── Inventory
│   │   ├── Controllers
│   │   ├── DTOs
│   │   ├── Models
│   │   └── Services
│   │
│   └── Warehouse
│       ├── Controllers
│       ├── DTOs
│       ├── Models
│       └── Services
│
├── Data
├── Exceptions
├── Middlewares
├── Migrations
│
├── Program.cs
└── appsettings.json
```

---

## Implemented Modules

### 1. Inventory Management

Handles product catalog management and inventory-related operations.

**Features:**
- Product management (CRUD operations).
- Category management.
- Stock In and Stock Out operations.
- Stock transaction history.
- Low-stock monitoring.
- Product search and category filtering.

### 2. Warehouse Management

Manages warehouse structures, physical stock, and internal stock movements.

**Features:**

**Warehouse Structure**
- Warehouse management.
- Warehouse zones management.
- Warehouse locations management.
- Active and inactive status management.

**Warehouse Stock Management**
- Add stock to warehouse locations.
- Issue stock from available inventory.
- Adjust stock based on physical inventory counts.
- Track available and reserved quantities.
- Support for different stock statuses:
  - Available
  - Quarantine
  - Rejected
  - Damaged

**Stock Movement Tracking**
- Record stock In, Out, and Adjustment operations.
- Maintain movement history.
- Track quantity before and after each movement.
- Store reasons for stock movements.

**Warehouse Transfers**
- Create transfers between warehouse locations.
- Support multiple products within a transfer.
- Transfer lifecycle management:
  - Pending
  - Approved
  - Dispatched
  - Received
  - Cancelled
- Validate stock availability during transfer operations.
- Record stock movements associated with transfers.

---

## Technical Stack

| Technology | Purpose |
|---|---|
| C# | Backend programming language |
| ASP.NET Core Web API | RESTful API development |
| Entity Framework Core | ORM and database access |
| SQL Server | Relational database |
| LINQ | Data querying |
| Swagger / OpenAPI | API documentation and testing |
| Git & GitHub | Version control |

---

## Engineering Practices

The project incorporates several backend engineering concepts:

- Modular Monolith Architecture.
- Separation of Controllers, Services, DTOs, and Models.
- Centralized exception handling using custom middleware.
- Business-level validation and custom exceptions.
- Entity Framework Core migrations.
- Database constraints and unique indexes.
- Database transactions for multi-step operations.
- Optimistic concurrency control using RowVersion.
- Stock movement auditing.
- RESTful API design.

---

## Getting Started

### Prerequisites

Make sure you have the following installed:

- .NET 8 SDK.
- SQL Server.
- Visual Studio 2022 or another compatible IDE.
- Git.

### 1. Clone the Repository

```bash
git clone https://github.com/loayeslam/Forge_Flow_ERP.git
```

### 2. Navigate to the Project

```bash
cd Forge_Flow_ERP
```

### 3. Configure the Database

Update the connection string in `appsettings.json` to match your local SQL Server configuration.

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_SQL_SERVER_CONNECTION_STRING"
  }
}
```

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

### 6. Explore the API

Once the application is running, open the Swagger UI using the URL displayed in the application output, usually:

```text
https://localhost:<port>/swagger
```

---

## Project Roadmap

The following modules are planned for future development:

- [ ] Purchasing Management
- [ ] Production Management
- [ ] Sales Management
- [ ] Finance & Accounting
- [ ] Authentication and Authorization
- [ ] Automated Unit and Integration Testing
- [ ] Real-time notifications using SignalR
- [ ] Reporting and Analytics

These features are part of the planned evolution of the project and are not represented as currently implemented functionality.

---

## Project Goals

ForgeFlow ERP is developed as a practical backend engineering project to explore:

- Building structured and maintainable ASP.NET Core applications.
- Translating real-world business workflows into backend logic.
- Managing complex inventory and warehouse operations.
- Understanding database consistency and concurrency.
- Applying software architecture principles in a practical environment.

---

## Author

**Loay Eslam**

Backend Developer | ASP.NET Core | C# | SQL Server

GitHub: [@loayeslam](https://github.com/loayeslam)

---

*ForgeFlow ERP – Building a structured foundation for integrated business operations.*
