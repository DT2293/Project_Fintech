# Project Fintech

A Fintech application built with **.NET 9**.  
The goal is to develop a financial management system including wallet services, transactions, and payment features.

---

## 🚀 Technologies

- ASP.NET Core 9
- Entity Framework Core
- PostgreSQL
- Docker / Docker Compose
- Clean Architecture (Domain, Application, Infrastructure, API)
- Design Patterns

---

## 📂 Project Structure

```mermaid
%%{init: {'theme':'neutral'}}%%
flowchart TD
    A[Project_Fintech] --> B[src]
    B --> C[FintechApp.Presentation<br/>API layer]
    B --> D[FintechApp.Application<br/>Application layer]
    B --> E[FintechApp.Domain<br/>Domain layer]
    B --> F[FintechApp.Infrastructure<br/>Infrastructure layer]
    A --> G[docker-compose.yml]
    A --> H[FintechApp.sln]
    A --> I[README.md]

