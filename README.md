# ProjectCookie

## Setup Instructions

### Frontend

1. Navigate to the Frontend directory and install dependencies:
    ```bash
    cd ./Frontend
    npm install
    ```

### Secrets

1. Add necessary secrets. In order to do that, copy the zipped folder from the email into Backend/DAL the folder structure should look like this:
    ```
    Backend
    ├── DAL
    │   ├── _Secrets
    │   ├── BaseClasses
    │   ├── BaseInterfaces
    │   ├── ...
    ```
  
### Backend & Docker

1. Start the docker container:
    ```bash
    cd ../Backend/.docker
    docker-compose up
    ```
2. If the cookie-timescaledb fails to start, copy the contents of the 'zzz-after-up-patch' folder into the .docker folder. This should insert som missing folders.

### Consul Setup

1. Open [Consul UI](http://localhost:8500/).
2. Navigate to Key/Value section.

#### Create `CookieData/`

1. Create a new entry named `Database` with the following content:
    ```json
    {
        "DatabaseName": "cookie",
        "Port": "5433",
        "Password": "pass",
        "Username": "admin",
        "Server": "host.docker.internal"
    }
    ```

2. Create a new entry named `Logger` with the following content:
    ```json
    {
        "Host": {
            "Port": "80",
            "Protocol": "http",
            "SettingsFile": "SimulationSettings/settings.json"
        },
        "Serilog": {
            "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Enrichers.Environment"],
            "MinimumLevel": {
                "Default": "Verbose",
                "Override": {
                    "Microsoft": "Debug",
                    "System": "Debug"
                }
            },
            "WriteTo": [
                {
                    "Name": "Console",
                    "Args": {
                        "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console",
                        "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                    }
                },
                {
                    "Name": "File",
                    "Args": {
                        "path": "Logs/log.log",
                        "rollingInterval": "Day"
                    }
                }
            ],
            "Enrich": ["FromLogContext", "WithMachineName", "WithThreadId"]
        }
    }
    ```

### Database Setup

1. Optionally, for safety, restart the container.
2. Update the database:
    ```bash
    dotnet ef database update --context PostgresDbContext
    ```

### Troubleshooting

- If you encounter the following error:
    ```bash
    Your startup project 'ProjectCookie' doesn't reference Microsoft.EntityFrameworkCore.Design. This package is required for the Entity Framework Core Tools to work. Ensure your startup project is correct, install the package, and try again.
    ```
    Install the required package:
    ```bash
    dotnet add package Microsoft.EntityFrameworkCore.Design
    ```

- If you encounter version mismatch errors:
    ```bash
    The Entity Framework tools version '8.0.4' is older than that of the runtime '9.0.0-preview.4.24267.1'. Update the tools for the latest features and bug fixes. See https://aka.ms/AAc1fbw for more information.
    Unable to create a 'DbContext' of type 'PostgresDbContext'. The exception 'Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[ProjectCookie.DAL.UnitOfWork.PostgresDbContext]' while attempting to activate 'ProjectCookie.DAL.UnitOfWork.PostgresDbContext'.' was thrown while attempting to create an instance. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
    ```
    Update the EF tools:
    ```bash
    dotnet tool update --global dotnet-ef --version 9.0.0-preview.4.24267.1
    ```

### Starting the Application

1. Start both the Backend and Frontend:
    ```bash
    cd ./Backend
    dotnet run
    ```

    ```bash
    cd ./Frontend
    npm start
    ```

2. Alternatively, create a Compound Configuration to start both services together.

### Testing

1. Execute `CreateDataTest`.
2. Optionally, you can run all other tests as well

### Accessing the Application

1. Open [http://localhost:3000](http://localhost:3000) in your browser.

### Questions

If you have any questions regarding the project setup process, do not hesitate to contact the developers:
- Martin Haring (martin.haring3@edu.fh-joanneum.at)
- Dominik Jäger (dominik.jaeger@edu.fh-joanneum.at)
- Raphael Klein (raphael.klein@edu.fh-joanneum.at)
