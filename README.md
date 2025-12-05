# ywBookStore

## Project Description
A desktop application for a book store, developed as a project for COMP4220 (Agile Software Development). This application allows users to browse books, manage orders, and provides administrative functionalities.

## Features
- User authentication (Login/Signup)
- Admin Panel for managing books and users
- Book browsing and search functionality
- Shopping cart and checkout process
- Order management
- User management
- Settings management

## Technologies Used
- C#
- .NET Framework (WPF for GUI)
- MSBuild
- NuGet
- MSTest (for unit testing)

## Setup and Installation

### Prerequisites
- Visual Studio (2019 or 2022 recommended) with .NET Desktop Development workload.
- SQL Server (for database backend)

### Steps
1.  **Clone the repository:**
    ```bash
    git clone https://github.com/Risheek10/COMP4220-Agile-Project.git
    cd COMP4220-Agile-Project
    ```
2.  **Restore NuGet Packages:**
    Open the solution file (`ywBookStore.sln`) in Visual Studio. NuGet packages should restore automatically. If not, right-click the solution in Solution Explorer and select "Restore NuGet Packages".
3.  **Database Setup:**
    - The project expects a SQL Server database. You will need to create and configure your database.
    - Update the connection string in `YWUnitTest/Properties/Settings.settings` and `assignment 1/App.config` to point to your SQL Server instance.
    - Ensure your SQL Server instance is accessible and populated with necessary tables and seed data (if applicable).
    
    *Note: A future update might provide SQL scripts for database creation and seeding.*

## How to Run the Application
1.  Open `ywBookStore.sln` in Visual Studio.
2.  Set `assignment 1` (ywBookStoreGUI) as the startup project.
3.  Build the solution (Build > Build Solution).
4.  Run the application (Debug > Start Debugging or F5).

## Testing
Unit tests are located in the `YWUnitTest` project (specifically the `ywBookStoreLIB` assembly).
- To run tests in Visual Studio: Test > Test Explorer > Run All Tests.
- **Note on CI:** Database-dependent tests are categorized with `[TestCategory("Database")]` and are excluded from the automated CI pipeline to allow non-database tests to pass.

## CI/CD Status
The project uses GitHub Actions for Continuous Integration and Continuous Deployment.
- **`.github/workflows/dotnet-test.yml`**: Runs unit tests.
- **`.github/workflows/ci-cd.yml`**: Builds, formats, and publishes the portable application executable.

## Contributing
Contributions are welcome! Please fork the repository and submit pull requests.

## License
[License Information Here, e.g., MIT License]