
# 🧅 OnionStack

A demo ASP.NET Core application following the **Onion Architecture** with modular separation of concerns, MediatR, Identity, and BinderModule integration. Designed to demonstrate scalable, maintainable, and testable code for enterprise-level applications.

---

## 📐 Architecture

This project implements **Onion Architecture**, also known as **Clean Architecture**, which emphasizes:

- Separation of concerns
- Dependency inversion
- Domain-centric development
- Infrastructure and UI as plug-ins to the core

### 📁 Layered Structure

```

OnionStack/
├── Core/            # Domain entities, value objects, domain interfaces
├── Application/     # Use cases, business logic, DTOs, contracts
├── Infrastructure/  # EF Core, Identity, Email services, implementations
├── Web/             # MVC application (Presentation/Host layer)
├── Shared/          # Cross-cutting concerns (common types, utils)
├── Tests/           # Unit and integration tests

````

---

## 🚀 Features

- ✅ ASP.NET Core MVC
- ✅ Onion Architecture
- ✅ MediatR for CQRS and decoupled communication
- ✅ ASP.NET Core Identity with custom abstraction
- ✅ BinderModule pattern for model binding across layers
- ✅ Dependency Injection with module-wise configuration
- ✅ Separation of concerns by strict project references

---

## 🔧 Technologies

- .NET 8
- ASP.NET Core MVC
- Entity Framework Core
- MediatR
- FluentValidation (optional)
- Hangfire (optional background jobs)
- xUnit (for testing)

---

## 🧪 Tests

All core services and handlers are covered with unit tests:

```bash
cd Tests
dotnet test
````

---

## 🏗️ Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/wedad-gamal/OnionStack.git
cd OnionStack
```

### 2. Run the application

Make sure `.NET 8 SDK` is installed.

```bash
cd Web
dotnet run
```

The app will run at `https://localhost:5001` or `http://localhost:5000`.

---

## 📷 UI Screenshots

*Coming soon:* Bootstrap-based responsive layout for HR system (employee list, role editing, etc.)

---

## ✍️ Author

**Wedad Gamal Elden**
Senior Web Developer
📧 [wedadgamal@gmail.com](mailto:wedadgamal@gmail.com)
🌍 [GitHub Profile](https://github.com/wedad-gamal)

---

## ⭐ Contributions

Contributions are welcome. If you'd like to improve something, feel free to open a pull request or issue.

---

## 📜 License

This project is open-source and available under the [MIT License](LICENSE).



