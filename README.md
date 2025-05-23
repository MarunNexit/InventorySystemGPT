# 📦 Inventory Management System

This is an ASP.NET Core MVC Inventory Management System built with Entity Framework Core and MySQL. The application allows you to manage product inventories efficiently and is containerized using Docker for easy setup and deployment.

---

## 🚀 Task
Create a ASP.NET Core application to manage a simple inventory system for a store. The system should allow users to view a list of available products, add new products, and update existing ones. Each product should have a name, description, price, and quantity. Use EF to persist the product information in a MySQL database.

## Acceptance Criteria: 
There are all the required codes and configs in the repository to run the application. 
There is a readme.md file with the application description and instructions on how to run it. 
Code is of good quality and easy to read and understand. 
There are unit tests in place, coverage >80% 
There are quality checks (coverage, complexity, check style) 
ChatGPT conversation logs are attached in the file chat.log 
Short feedback for each task added to readme.md in the following format: - Was it easy to complete the task using AI? - How long did task take you to complete? (Please be honest, we need it to gather anonymized statistics) - Was the code ready to run after generation? What did you have to change to make it usable?- Which challenges did you face during completion of the task?- Which specific prompts you learned as a good practice to complete the task?

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
git clone https://github.com/MarunNexit/InventorySystemGPT.git
cd InventorySystem
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



## 📝 Feedback on Task Completion

- **Was it easy to complete the task using AI?**  
  Yes, AI provided clear guidance for most parts, though some manual adjustments were needed.

- **How long did the task take you to complete?**  
  Approximately 2.5 hours.

- **Which challenges did you face during completion of the task?**  
  The most challenging part was setting up and configuring the database correctly.

- **Which specific prompts did you learn as a good practice to complete the task?**  
  Asking for detailed step-by-step instructions and error fixes proved most helpful.