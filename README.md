# API Testing Project - Go REST API

A comprehensive C# API testing framework using NUnit for testing the [GoRest API](https://gorest.co.in). This project demonstrates modern API testing practices with proper test organization, configuration management, and dependency injection.

## 🏗️ Project Structure

```
Project/
├── Framework/
│   ├── Helpers/
│   │   └── TestDataGenerator.cs          # Generates random test data
│   ├── HttpClientFactory/
│   │   └── HttpClientProvider.cs         # HTTP client configuration
│   ├── Models/
│   │   └── UserResponse.cs               # API models and DTOs
│   └── Service/
│       ├── ServiceRegistry.cs            # DI container configuration
│       └── UserService.cs                # User API operations
├── Tests/
│   ├── DebugTest.cs                      # Debugging and connectivity tests
│   ├── TestBase.cs                       # Base test class
│   └── UsersTests.cs                     # Main user API tests
├── config.json                           # Test configuration
└── FinalProject.csproj                   # Project file
```

## 🚀 Features

- **Comprehensive CRUD Testing**: Full coverage of Create, Read, Update, Delete operations
- **Data-Driven Tests**: Random test data generation for diverse testing scenarios
- **HTTP Method Testing**: Tests for GET, POST, PUT, PATCH, and DELETE
- **Error Handling**: Validation of error responses and edge cases
- **Clean Architecture**: Separation of concerns with service layer and models
- **Dependency Injection**: Modern DI patterns for better testability
- **Flexible Configuration**: JSON-based configuration management

## 🛠️ Technology Stack

- **.NET 8.0**: Latest .NET framework
- **NUnit**: Testing framework
- **FluentAssertions**: Fluent assertion library for better readability
- **System.Text.Json**: Built-in JSON serialization
- **Microsoft.Extensions**: Configuration and dependency injection

## 📋 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 / VS Code / JetBrains Rider
- GoRest API Token (get it from [gorest.co.in](https://gorest.co.in))

## ⚙️ Configuration

1. Update the `config.json` file with your GoRest API token:

```json
{
  "TestConfiguration": {
    "BaseUrl": "https://gorest.co.in/public/v2/",
    "UsersEndpoint": "users",
    "Token": "YOUR_API_TOKEN_HERE"
  }
}
```

## 🏃‍♂️ Running Tests

### Command Line
```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "ClassName=UsersTests"

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

### Visual Studio
- Open the project in Visual Studio
- Use Test Explorer to run individual tests or test suites
- Right-click on test methods/classes and select "Run Tests"

## 📝 Test Scenarios

### User Management Tests
- **Get All Users**: Retrieve and validate user list
- **Create User**: Create new users with validation
- **Get User by ID**: Retrieve specific users
- **Update User (PUT)**: Complete user updates
- **Update User (PATCH)**: Partial user updates
- **Delete User**: User deletion and verification
- **Error Handling**: Invalid data, non-existent users, duplicate emails

### Debug Tests
- **API Connectivity**: Verify API token and basic connectivity
- **Create User Debug**: Detailed user creation with logging

## 🔧 Key Components

### TestDataGenerator
Generates realistic random test data:
- Random names, emails, genders, and statuses
- Timestamp-based unique email generation
- Configurable field updates for PATCH operations

### UserService
Handles all API operations:
- HTTP client management
- JSON serialization/deserialization
- Support for all HTTP methods (GET, POST, PUT, PATCH, DELETE)

### TestBase
Provides common setup and teardown:
- Dependency injection configuration
- HTTP client initialization
- Service registration

## 📊 Test Execution Order

Tests are executed in a specific order using `[Order]` attributes:
1. Get All Users
2. Create User (stores ID for subsequent tests)
3. Get User by ID
4. Update User (PUT)
5. Update User (PATCH)
6. Delete User

## 🐛 Debugging

The project includes comprehensive debugging features:
- Console output for request/response details
- JSON logging for troubleshooting
- Detailed error messages for failed assertions

## 📈 Best Practices Implemented

- **Page Object Model**: Service layer acts as page objects for API
- **Single Responsibility**: Each class has a clear, focused purpose
- **DRY Principle**: Reusable test data generation and common setup
- **Clear Assertions**: FluentAssertions for readable test results
- **Proper Error Handling**: Comprehensive error scenario testing
---

**Note**: This is an educational project demonstrating API testing best practices. Make sure to keep your API tokens secure and never commit them to version control.
