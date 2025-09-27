# 🛒 E_Commerce_Microservices

`E_Commerce_Microservices` is a **scalable and modular E-Commerce Platform** built using **.NET Core Microservices Architecture**.  
Instead of creating one big monolithic application, the solution is divided into **independent, loosely coupled services** that can be developed, deployed, and scaled individually.  

This architecture ensures:  
- **Scalability** → Each service (Auth, Product, Order) can scale independently  
- **Flexibility** → Easy to add new features without affecting existing services  
- **Maintainability** → Code is organized and easy to debug  
- **Security** → JWT Authentication & centralized API Gateway  
- **Reliability** → Logging, Exception Handling, and Retry Policies  

---



 🚀 Tech Stack
- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)  
- **JWT Authentication**  
- **Serilog Logging**  # 🛒 E_Commerce_Microservices

This is a **Microservices-based E-Commerce Platform** built with **.NET Core**.  
The solution is divided into multiple independent services with a **Shared Library** and an **API Gateway**.

---

## 🚀 Tech Stack
- **.NET 8 / ASP.NET Core Web API**
- **Entity Framework Core (SQL Server)**
- **JWT Authentication**
- **Serilog Logging**
- **Microservices Architecture**
- **API Gateway Pattern**

---

## 📂 Project Structure

### 1. 🧩 **E_CommerceSharedLibrary**
A common library used across all microservices.  
Includes:
- **JWT Authentication Setup** → Extension method for token validation  
- **Serilog Logging** → Console + File logging with templates  
- **Global Exception Middleware** → Handles errors & returns custom JSON response  
- **API Gateway Header Middleware** → Ensures requests come only via API Gateway  
- **Generic Repository Interface** → Common CRUD operations  
- **Common Response Wrapper** → Unified response format  

---

### 2. 🔐 **AuthService**
Handles **authentication & authorization** for the platform.  
- User registration & login  
- JWT token generation  
- Role-based authentication  

---

### 3. 📦 **ProductService**
Manages **product-related operations**.  
- Product CRUD APIs  
- Category & inventory management  
- Integration with shared logging & middleware  

---

### 4. 🛍️ **OrderService**
Handles **customer orders**.  
- Order placement & tracking  
- Payment status updates  
- Communication with ProductService for stock updates  

---

### 5. 🌐 **ApiGateway**
Acts as the **single entry point** for all client requests.  
- Request routing to respective microservices  
- Centralized authentication & authorization  
- Rate limiting and security middleware  

---
- **Microservices + API Gateway Pattern**
