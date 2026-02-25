# 📦 Inventory Management System (.NET Console Application)

## 📖 Overview

This project is a **Console-Based Inventory Management System** built using **.NET Framework**.  
The system allows users to manage products using a JSON file as data storage.

The main objective of this project was not only to implement CRUD functionality, but also to understand:

- Clean architecture principles  
- Reflection-based dynamic input collection  
- Attribute-driven validation  
- Dictionary-based scalable menu system  
- Event-driven low stock alert mechanism  

The system is designed in a scalable way so it can be extended into a large enterprise-level application.

---

## 🚀 Features

- ✅ Add Product
- ✅ Update Product
- ✅ Deliver Product (Stock Reduction)
- ✅ Receive Product (Stock Increase)
- ✅ Search Products by Keyword
- ✅ View Complete Inventory
- ✅ View Low Stock Products
- ✅ Automatic Low Stock Alert Event
- ✅ JSON File-Based Data Persistence
- ✅ Attribute-Based Input Skipping and Validation
- ✅ Clean and Scalable Menu Navigation System

---

## 🏗 Architecture & Design Concepts Used

### 1️⃣ Dictionary-Based Menu System

Instead of using `if-else` or `switch`, menu options are mapped using:

```csharp
Dictionary<string, Action>