🛒 E_Commerce_Microservices

This is a **Microservices-based E-Commerce Platform** built with **.NET Core**.  
The solution is divided into multiple independent services with a **Shared Library** and an **API Gateway**.


## 🚀 Tech Stack
- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)  
- **JWT Authentication**  
- **Serilog Logging**  
- **Microservices + API Gateway Pattern**

- ## 📂 Project Structure

### 1. **E_CommerceSharedLibrary**
Common library used across all services.  
Includes:
- **JWT Authentication setup** (extension method for token validation)  
- **Serilog Logging** (console + file logging)  
- **Global Exception Middleware** (handles errors & returns custom JSON response)  
- **API Gateway Header Middleware** (ensures requests come only via API Gateway)  
- **Generic Repository Interface** for CRUD operations  
- **Common Response Wrapper** 
