# ExpenseApp – Personal Expense Tracker

A web-based personal expense tracking application built with **ASP.NET Core 8 MVC**, **Entity Framework Core**, and **SQL Server**.  
Users can register, log in, record income and expenses, view dashboard analytics, search transactions, generate reports, and manage their personal finances securely.

---

## Features

- 🔐 **Authentication**
  - User registration and login
  - Cookie-based authentication
  - Password hashing for secure password storage
  - Change password feature
  - User-specific session management

- 💸 **Transaction Management**
  - Separate pages for **Income** and **Expenses**
  - Add, edit, and delete transactions
  - Category, amount, date, and description fields
  - User ownership check for transaction security

- 📊 **Dashboard & Analytics**
  - Summary cards for **Total Income**, **Total Expense**, and **Balance**
  - Recent transactions table
  - Monthly income vs. expense chart
  - Expense breakdown by category
  - Transaction trend visualization

- 🔍 **Search & Filter**
  - Search transactions by date, type, category, amount, and description
  - Dedicated **Search Transactions** page

- 🧾 **Reports & Export**
  - Report page for financial summaries and transaction data
  - PDF report generation using **jsPDF**, **html2canvas**, and related client-side tools

- 🛡️ **Security & Validation**
  - Anti-forgery token validation
  - Server-side form validation
  - Authorization for protected pages
  - Hashed password storage
  - User-specific transaction access control

---

## Tech Stack

**Web Application**

- ASP.NET Core 8 MVC
- C#
- Razor Views (.cshtml)
- Bootstrap 5
- Chart.js
- jsPDF
- html2canvas

**Backend / Server**

- ASP.NET Core MVC Controllers
- Cookie Authentication
- PasswordHasher from Microsoft.AspNetCore.Identity
- Dependency Injection
- Middleware-based request pipeline

**Database**

- SQL Server / SQL Server Express
- Entity Framework Core 8
- EF Core Migrations
- LINQ for querying, filtering, grouping, and aggregation

---

## Project Structure

```text
ExpenseApp/
├── Controllers/                  # MVC controllers
│   ├── AccountController.cs       # Login, registration, logout, change password
│   ├── DashboardController.cs     # Dashboard summary and chart data
│   ├── HomeController.cs
│   └── TransactionsController.cs  # Income, expense, search, report, CRUD operations
│
├── Data/                         # Database context and seed data
│   ├── ApplicationDbContext.cs
│   └── DbSeeder.cs
│
├── Migrations/                   # EF Core migration files
│
├── Models/                       # Entities and ViewModels
│   ├── AppUser.cs
│   ├── Transaction.cs
│   └── ViewModels/
│
├── Services/                     # Application services
│   └── PasswordService.cs
│
├── Views/                        # Razor views
│   ├── Account/
│   ├── Dashboard/
│   ├── Home/
│   ├── Shared/
│   └── Transactions/
│
├── wwwroot/                      # Static files
│   ├── css/
│   ├── js/
│   └── lib/
│
├── appsettings.json              # Database connection and app configuration
├── appsettings.Development.json
├── Program.cs                    # Application startup and middleware configuration
├── ExpenseTracker.Web.csproj
└── ExpenseTracker.Web.sln
```

---

## Getting Started

### Prerequisites

* **Visual Studio 2022** or later
* **.NET 8 SDK**
* **SQL Server** or **SQL Server Express**
* **Entity Framework Core tools**

---

## 1. Clone the Repository

```bash
git clone https://github.com/bengalbee25/ExpenseApp-Personal-Expense-Tracker.git
cd ExpenseApp-Personal-Expense-Tracker
```

---

## 2. Database Setup

1. Open the project in **Visual Studio**.
2. Open `appsettings.json`.
3. Update the connection string according to your SQL Server setup:

```json
"ConnectionStrings": {
  "ExpenseApp": "Server=localhost\\SQLEXPRESS;Database=ExpenseAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

> Note: The server name may be different on your computer. Common examples are `localhost`, `localhost\\SQLEXPRESS`, or `(localdb)\\MSSQLLocalDB`.

---

## 3. Apply Database Migration

Open **Package Manager Console** in Visual Studio and run:

```powershell
Update-Database
```

Or using .NET CLI:

```bash
dotnet ef database update
```

This will create the required database tables, including:

* `Users`
* `Transactions`

---

## 4. Run the Project

Using Visual Studio:

1. Open the solution file: `ExpenseTracker.Web.sln`
2. Set the project as the startup project.
3. Press **F5** or click **Run**.

Using .NET CLI:

```bash
dotnet run
```

The application will run on a local URL such as:

```text
https://localhost:xxxx
```

---

## 5. Using the App

1. Open the application in your browser.
2. Register a new user account.
3. Log in using your credentials.
4. Use the sidebar/menu to navigate:

   * **Dashboard** – view income, expense, balance, charts, and recent transactions
   * **Income** – add and manage income records
   * **Expenses** – add and manage expense records
   * **Search Transactions** – search financial records
   * **Generate Report** – view and download financial reports
   * **Change Password** – update account password

---

## Available Features / Modules

### Account Module

* Register new user
* Login existing user
* Logout
* Change password
* Password hashing and cookie authentication

### Dashboard Module

* Total income calculation
* Total expense calculation
* Balance calculation
* Recent transaction list
* Monthly income and expense chart
* Expense category chart

### Transaction Module

* Add income
* Add expense
* Edit transaction
* Delete transaction
* User-specific transaction filtering

### Report Module

* View financial report
* Show summary and transaction details
* Download report as PDF

---

## Important Commands

### Restore Dependencies

```bash
dotnet restore
```

### Build Project

```bash
dotnet build
```

### Run Project

```bash
dotnet run
```

### Apply Migration

```bash
dotnet ef database update
```

---

## Security Notes

* Passwords are stored using hashed format, not plain text.
* Cookie authentication is used for login sessions.
* Anti-forgery tokens are used in POST forms.
* Only authenticated users can access dashboard and transaction pages.
* Each user can only access and manage their own transactions.

---

## Deployment Notes

* Publish the ASP.NET Core project using Visual Studio or the .NET CLI.
* Configure the production SQL Server connection string.
* Apply EF Core migrations on the production database.
* Do not upload real passwords, private keys, or secret connection strings to GitHub.
* Use environment variables or secure hosting configuration for sensitive values.

---

## Future Improvements

* Add admin panel
* Add monthly budget limit
* Add export to Excel
* Add role-based authorization
* Add advanced date-range filters
* Add unit testing and integration testing
* Add server-side PDF generation
* Add audit log for transaction changes

---

## Project Purpose

This project was developed as an academic web application project to demonstrate **ASP.NET Core MVC**, **Entity Framework Core**, **SQL Server database design**, **authentication**, **CRUD operations**, **report generation**, **data visualization**, and **software engineering standards**.

---

## License

This project is for academic and learning purposes. Add a LICENSE file if you want to publish it under a specific open-source license.

---

## Author

Developed by **Your Name**
