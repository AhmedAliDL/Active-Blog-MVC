# Active Blog Service – ASP.NET MVC

Active Blog Service is a full-featured blogging platform built with **ASP.NET MVC**, **Entity Framework**, and **ASP.NET Identity**. It enables users to publish blog posts, engage through comments, and manage their profiles, all within a fully authenticated and role-based environment.

This project is designed with clean architecture principles and supports scalable, maintainable development.

---

## Features

### User & Authentication

* Secure authentication built with **ASP.NET Identity**
* Role-based authorization (Admin / User)
* User profile management (name, image, address, etc.)

### Blogging System

* Create, edit, and delete blog posts
* Rich blog content (title, categories, images, formatted text)
* Track post creation dates and authors
* Public display of blogs with user attribution

### Comments System

* Authenticated users can comment on blog posts
* Comments linked to both blogs and users

### Admin Features

* Manage users and roles
* Control user permissions

---

## Technology Stack

* **ASP.NET MVC**
* **Entity Framework Code First**
* **SQL Server**
* **ASP.NET Identity**
* **Bootstrap / jQuery** (frontend)

---

## Database Diagram (ERD)

<img height="700" width="1200" src="Active Database Schema.png">

---
## Notes
⚠️ Requirement: .NET 6 or later must be installed to run this application.

