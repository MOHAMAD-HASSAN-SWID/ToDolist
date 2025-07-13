# 📝 To-Do List API

This is a simple and secure Web API built with ASP.NET Core for managing a personal to-do list. It supports user login, task management, and permission control using JWT tokens.

## 🚀 Features

- User authentication with JWT
- Sign in and register new users
- Add, update, and delete tasks (to-do items)
- Mark tasks as complete or incomplete
- Filter tasks by category, priority, or ID
- View all tasks with pagination and optional sorting
- Role-based permissions using bitwise flags


## 🔐 Permission System

Permissions use bit flags. Each action has a value:
- 1 = Show all tasks  
- 2 = Show by category  
- 4 = Show by priority  
- 8 = Show by ID  
- 16 = Change task state  
- 32 = Add users  
- 64 = Add tasks  
- 128 = Update tasks  
- 256 = Remove tasks  
- -1 = All permissions

A user's permission is stored in their JWT token. For example, permission 97 = 64 (add task) + 32 (add user) + 1 (view all tasks).

## 📚 API Endpoints

- POST /api/users/SignIn → Sign in and receive JWT token  
- POST /api/users/add → Add new user (requires permission)  
- GET /api/dolist/ShowDolistAsync → Show all tasks  
- GET /api/dolist/ShowDolistByCategoryAsync → Filter tasks by category  
- GET /api/dolist/ShowDolistByPriorityAsync → Filter tasks by priority  
- GET /api/dolist/ShowDolistByIdAsync → Get task by ID  
- POST /api/dolist/AddDolistAsync → Add a task  
- PUT /api/dolist/UpdateDolistAsync → Update a task  
- PUT /api/dolist/ChangeStateDolistAsync → Mark task complete/incomplete  
- DELETE /api/dolist/RemoveDolistAsync → Delete a task

Most endpoints require authentication and valid permissions.

## ✅ Swagger

Swagger is available at:
https://localhost:5001/swagger

Use it to test and explore all endpoints. Insert your JWT token to authorize requests.

## 📜 License

This project is open-source and free to use or modify.

## 👤 Author

Developed by [MOHAMAD HASSAN SWEID]  
	
https://github.com/MOHAMAD-HASSAN-SWID
