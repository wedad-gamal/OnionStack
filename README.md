# 🧠 Modular HR System Starter Template

A reusable backend starter for HR systems built with modern .NET architecture patterns. Designed to be scalable, testable, and ready for production workflows.

## 🌟 Key Features

- ✅ **Onion Architecture** for clean separation of concerns  
- ✅ **Dependency Injection** with per-layer BinderModules  
- ✅ **Hangfire** integration for background onboarding tasks  
- ✅ **MailKit** for automated email communication  
- ✅ **Structured Logging** with correlation ID propagation  
- ✅ **Unit Testing** with xUnit and Moq  
- ✅ **Framework-Agnostic Service Contracts**  

## 🛠️ Technologies

`.NET 8`, `Hangfire`, `MailKit`, `Serilog`, `xUnit`, `Moq`, `EF Core`, `FluentValidation`

## 🚀 Getting Started

```bash
git clone https://github.com/<your-username>/modular-hr-starter.git
cd modular-hr-starter
dotnet restore
dotnet run
```

## 📦 Architecture Overview

- **Core**: Domain models, abstractions, DTOs  
- **Application**: Service contracts, business logic, validators  
- **Infrastructure**: DB context, repositories, third-party integrations  
- **Presentation**: API controllers, middleware, DI configuration  
- **BackgroundJobs**: Hangfire jobs & orchestration logic  

![Architecture Diagram](assets/architecture-diagram.png) <!-- Add this later when ready -->

## 🔧 Roadmap

- [x] Modular DI binders  
- [x] Hangfire onboarding flow  
- [x] Unit tests for services and controllers  
- [ ] Identity integration with UserManager & RoleManager  
- [ ] CI/CD pipeline with GitHub Actions  
- [ ] Interactive API docs with Swagger  

## 📄 License

MIT – free to use and modify with attribution

## 💬 Let’s Connect

If you’re a fellow backend engineer, recruiter, or open-source enthusiast, feel free to reach out or collaborate!
