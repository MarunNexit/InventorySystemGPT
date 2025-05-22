# 📦 Inventory Management System

This is an ASP.NET Core MVC Inventory Management System built with Entity Framework Core and MySQL. The application allows you to manage product inventories efficiently and is containerized using Docker for easy setup and deployment.

---

## 🚀 Features

- Manage products (Create, Read, Update, Delete)
- Entity Framework Core with MySQL database
- Clean and responsive UI
- Docker support for MySQL
- Unit tests with NUnit

---

## 🛠️ Technologies

- ASP.NET Core
- Entity Framework Core
- MySQL 8.0
- Docker
- NUnit (for unit testing)

---

## ⚙️ Prerequisites

- [.NET SDK 6.0 or later](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/get-started)

---

## 🐳 Docker Setup

1. Clone the repository:

```bash
git clone https://github.com/yourusername/inventory-management.git
cd inventory-management
```

2. Create a `docker-compose.yml` file or use the provided one:

```yaml
version: '3.8'

services:
  mysql:
    image: mysql:8.0
    container_name: inventory-mysql
    restart: always
    environment:
      MYSQL_ROOT_PASSWORD: rootpass
      MYSQL_DATABASE: InventoryDb
      MYSQL_USER: inventory_user
      MYSQL_PASSWORD: 123
    ports:
      - "3306:3306"
    volumes:
      - db_data:/var/lib/mysql
    networks:
      - inventory-net

volumes:
  db_data:

networks:
  inventory-net:
```

3. Start MySQL container:

```bash
docker-compose up -d
```

---

## 🔧 Application Configuration

Update your `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=InventoryDb;user=inventory_user;password=123;"
}
```

Apply EF Core migrations:

```bash
dotnet ef database update
```

---

## ▶️ Run the App

Build and run the application:

```bash
dotnet run
```

Navigate to `https://localhost:5001` or `http://localhost:5000` in your browser.

---

## ✅ Run Tests

Unit tests are written using NUnit.

```bash
dotnet test
```

---

## 📂 Project Structure

```
InventorySystem/
├── Models/              # Data models
├── Data/                # EF DbContext
├── Pages/               # Razor Pages
├── Tests/               # NUnit Test Project
├── appsettings.json     # Configuration file
└── Program.cs
```

---

## 🧼 Clean Up

To stop and remove the MySQL container:

```bash
docker-compose down -v
```

---

## 📜 License

MIT License. See LICENSE for details.