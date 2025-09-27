🛒 ###**E_Commerce_Microservices**

This is a **Microservices-based E-Commerce Platform** built with **.NET Core**.  
The solution is divided into multiple independent services with a **Shared Library** and an **API Gateway**.


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
