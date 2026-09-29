# InParadise 🌴

[Persian (فارسی)](#-فارسی) | [English](#-english)

---

## 🇬🇧 English

A comprehensive and scalable Villa & Resort Booking Management System built with **ASP.NET Core MVC** and **Clean Architecture**. This platform handles end-to-end accommodation management, secure bookings, online payments, and administrative analytics.

### 🚀 Key Features
* **Property Management**: Full CRUD operations for managing Villas, Villa Numbers, and Amenities.
* **Advanced Booking Flow**: Seamless reservation system tracking booking statuses and details (`BookingController`, `BookingService`).
* **Online Payment Integration**: Integrated with the **ZarinPal** payment gateway for secure transaction processing and verification (`ZarinPalRequestDto`, `PaymentVerifyResultDto`).
* **Interactive Admin Dashboard**: Visual analytics and reporting using dynamic Pie, Line, and Radial Bar charts (`DashboardController`, `apexcharts.css`).
* **Identity & Security**: Secure user registration, authentication, and authorization using ASP.NET Core Identity (`AccountController`, `ApplicationUser`).

### 🏗️ Architecture & Design Patterns
The project strictly adheres to **Clean Architecture** principles to ensure separation of concerns, testability, and maintainability. It is divided into four main layers:
1. **Domain Layer (`InParadise.Domain`)**: Contains the core enterprise logic and entities (`Villa`, `Booking`, `Amenity`, `ApplicationUser`).
2. **Application Layer (`InParadise.Application`)**: Contains business logic, DTOs, Service Interfaces, and implementations (`IVillaService`, `PaymentService`, `DashboardService`).
3. **Infrastructure Layer (`InParadise.Infrastructure`)**: Handles data access, database context, and EF Core Migrations (`ApplicationDbContext`, `DbInitializer`).
4. **Presentation Layer (`InParadise.Web`)**: The ASP.NET Core MVC web application containing Controllers, Views, and ViewModels.

#### Implemented Patterns
* **Repository Pattern**: Abstracts data access logic (`IVillaRepository`, `BookingRepository`).
* **Unit of Work Pattern**: Manages database transactions efficiently across multiple repositories (`IUnitOfWork`).
* **Dependency Injection (DI)**: Extensively used to inject services and repositories for loose coupling.

### 💻 Tech Stack
* **Backend**: C#, ASP.NET Core MVC
* **Database**: SQL Server, Entity Framework Core (Code-First Approach)
* **Frontend**: HTML5, CSS3, Razor Views (`.cshtml`)
* **Charting**: ApexCharts

---

## 🇮🇷 فارسی

یک سیستم جامع و توسعه‌پذیر برای مدیریت رزرو ویلا و اقامتگاه که با استفاده از **ASP.NET Core MVC** و **Clean Architecture (معماری تمیز)** ساخته شده است. این پلتفرم مدیریت صفر تا صد اقامتگاه‌ها، رزروهای امن، پرداخت‌های آنلاین و گزارش‌گیری‌های مدیریتی را بر عهده دارد.

### 🚀 ویژگی‌های کلیدی
* **مدیریت اقامتگاه‌ها**: عملیات کامل CRUD برای مدیریت ویلاها، شماره‌های ویلا و امکانات رفاهی (Amenities).
* **سیستم رزرو پیشرفته**: سیستم رزرو یکپارچه برای پیگیری وضعیت‌ها و جزئیات رزرو (`BookingController` و `BookingService`).
* **پرداخت آنلاین**: ادغام شده با درگاه پرداخت **زرین‌پال** جهت پردازش و اعتبارسنجی امن تراکنش‌ها (`ZarinPalRequestDto` و `PaymentVerifyResultDto`).
* **داشبورد تعاملی مدیریت**: گزارش‌گیری و تحلیل‌های بصری با استفاده از نمودارهای پویا (پای، خطی، و... با ابزار ApexCharts).
* **هویت و امنیت**: ثبت‌نام، احراز هویت و سطح‌بندی دسترسی کاربران با استفاده از ASP.NET Core Identity.

### 🏗️ معماری و الگوهای طراحی
این پروژه دقیقاً از اصول **معماری کلین (Clean Architecture)** پیروی می‌کند تا جداسازی دغدغه‌ها (Separation of Concerns)، تست‌پذیری و نگهداری آسان کدها تضمین شود. این پروژه به چهار لایه اصلی تقسیم شده است:
1. **لایه Domain (`InParadise.Domain`)**: شامل منطق اصلی و موجودیت‌ها (Entities) مانند ویلا، رزرو، امکانات و کاربر.
2. **لایه Application (`InParadise.Application`)**: شامل بیزینس لاجیک، DTOها، اینترفیس سرویس‌ها و پیاده‌سازی آن‌ها.
3. **لایه Infrastructure (`InParadise.Infrastructure`)**: مدیریت دسترسی به داده‌ها، DbContext و دیتابیس (EF Core Migrations).
4. **لایه Presentation (`InParadise.Web`)**: برنامه تحت وب ASP.NET Core MVC که شامل کنترلرها، ویوها و ویومدل‌ها است.

#### الگوهای استفاده شده (Patterns)
* **الگوی Repository**: برای انتزاع منطق ارتباط با دیتابیس.
* **الگوی Unit of Work**: برای مدیریت یکپارچه و بهینه تراکنش‌های دیتابیس در چندین ریپازیتوری.
* **تزریق وابستگی (DI)**: استفاده گسترده برای تزریق سرویس‌ها و ریپازیتوری‌ها جهت کاهش وابستگی (Loose Coupling).

### 💻 تکنولوژی‌های استفاده شده
* **بک‌اند**: C#, ASP.NET Core MVC
* **دیتابیس**: SQL Server, Entity Framework Core (رویکرد Code-First)
* **فرانت‌اند**: HTML5, CSS3, Razor Views (`.cshtml`)
* **نمودارها**: ApexCharts
