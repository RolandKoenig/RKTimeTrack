# RKTimeTrack Project Guidelines

This document provides essential information for developers working on the RKTimeTrack project.

## Build and Configuration

### Prerequisites
- **.NET 10 SDK**
- **Node.js** (v20 or newer recommended)
- **npm** (v10 or newer recommended)

### Backend (.NET)
The backend is a .NET 10 solution. 
- To restore dependencies: `dotnet restore`
- To build the solution: `dotnet build`
- To run the service: `dotnet run --project src\RolandK.TimeTrack.Service\RolandK.TimeTrack.Service.csproj`

### Frontend (Vue.js)
The frontend is located in `src/RolandK.TimeTrack.Service/ClientApp`.
- To install dependencies: `npm install` (within the `ClientApp` directory)
- To run in development mode: `npm run dev`
- To build for production: `npm run build`

## Testing

### Backend Tests (xUnit)
Backend tests are located in various `.Tests` projects within the `src` directory.
- To run all tests: `dotnet test`
- To run a specific test project: `dotnet test path\to\Project.Tests.csproj`
- To run a specific test: `dotnet test --filter "FullyQualifiedName=Namespace.Class.Method"`

#### Adding New Backend Tests
1. Create a new test class in the appropriate `.Tests` project.
2. Use the `[Fact]` attribute for simple tests or `[Theory]` for parameterized tests.
3. Example:
   ```csharp
   using Xunit;
   public class MyTests {
       [Fact]
       public void MyTest() {
           Assert.True(true);
       }
   }
   ```

### Frontend Tests (ASP.NET Core + xUnit + Playwright)
Frontend unit tests are done using Playwright together with ASP.NET Core and xUnit in the project src/RolandK.TimeTrack.Service.Tests.
All frontend tests are located in `src/RolandK.TimeTrack.Service.Tests/UiTests`.

## Development Information

### Code Style
- **C#**: The project uses C# 14. `Directory.Build.props` enforces `TreatWarningsAsErrors`. Implicit usings and nullable reference types are enabled.
- **Frontend**: ESLint and Prettier are used for linting and formatting. Run `npm run lint` or `npm run format` in the `ClientApp` directory.

### Project Structure
- `src/`: Contains all source code and tests.
- `testdata/`: Contains data used for integration and manual testing.
- `docs/`: Contains project documentation and decision records.
