# School Management System API

This project is a School Management System API built using ASP.NET Core. The API provides endpoints to manage various entities in a school environment, such as Students, Courses, Teachers, Departments, and Faculties. It also includes JWT authentication for user registration and login.

## Features

- CRUD operations for Students, Courses, Teachers, Departments, and Faculties.
- Many-to-many relationship between Students and Courses.
- One-to-many relationships between Department and Students, Faculty and Departments, Course and Teachers, Department and Students, Department and Courses.
- JWT authentication system for user registration and login.
- Usage of Data Transfer Objects (DTOs) to encapsulate data exchanged between client and server.
- Generic repository pattern to manage common database operations.

## Endpoints

### Students

- `GET /api/students`: Get a list of all students.
- `GET /api/students/{id}`: Get details of a specific student by ID.
- `GET /api/students/{studentId}/courses`: Get courses enrolled by a specific student.
- `POST /api/students`: Create a new student.
- `PUT /api/students/{id}`: Update student details.
- `DELETE /api/students/{studentId}`: Delete a student.

### Courses

- `GET /api/courses`: Get a list of all courses.
- `GET /api/courses/{id}`: Get details of a specific course by ID.
- `GET /api/courses/{courseId}/students`: Get students enrolled in a specific course.
- `GET /api/courses/{courseId}/teachers`: Get teachers assigned to a specific course.
- `POST /api/courses`: Create a new course.
- `POST /api/courses/{courseId}/enrollstudents`: Enroll students in a course.
- `PUT /api/courses/{id}`: Update course details.
- `DELETE /api/courses/{courseId}`: Delete a course.

### Teachers

- `GET /api/teachers`: Get a list of all teachers.
- `GET /api/teachers/{id}`: Get details of a specific teacher by ID.
- `GET /api/teachers/{teacherId}/course`: Get the course taught by a specific teacher.
- `GET /api/teachers/{teacherId}/students`: Get students taught by a specific teacher.
- `POST /api/teachers`: Create a new teacher.
- `PUT /api/teachers/{teacherId}`: Update teacher details.
- `DELETE /api/teachers/{teacherId}`: Delete a teacher.

### Departments

- `GET /api/departments`: Get a list of all departments.
- `GET /api/departments/{id}`: Get details of a specific department by ID.
- `GET /api/departments/{departmentId}/courses`: Get courses offered by a specific department.
- `GET /api/departments/{departmentId}/teachers`: Get teachers in a specific department.
- `GET /api/departments/{departmentId}/students`: Get students in a specific department.
- `POST /api/departments`: Create a new department.
- `PUT /api/departments/{id}`: Update department details.
- `DELETE /api/departments/{departmentId}`: Delete a department.

### Faculty

- `GET /api/faculty`: Get a list of all faculties.
- `GET /api/faculty/{id}`: Get details of a specific faculty by ID.
- `POST /api/faculty`: Create a new faculty.
- `PUT /api/faculty/{id}`: Update faculty details.
- `DELETE /api/faculty/{facultyId}`: Delete a faculty.

## Authentication

This API uses JWT authentication for user registration and login. Users need to register and login to obtain a token, which they will use to access the various entity endpoints.

## Technologies Used

- ASP.NET Core for building the API.
- AutoMapper for mapping between entity models and DTOs.
- JWT authentication for user security.
- Entity Framework Core for database operations.
- Generic repository pattern for common operations.
- Logging for error handling and debugging.

## How to Run This Project

1. Clone this repository to your local machine.
2. Open the project in your preferred IDE (Visual Studio, Visual Studio Code, etc.).
3. Configure your database connection in the `appsettings.json` file.
4. Run database migrations to create the necessary tables.
5. Build and run the project.

## API Documentation

For detailed information about the API endpoints, authentication, and request/response formats, refer to the API documentation provided.

## Contributing

Contributions are welcome! If you find a bug or want to enhance the functionality, feel free to open an issue or submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
