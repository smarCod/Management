# Management System API

## Übersicht
Dieses Projekt ist eine .NET WebAPI-Anwendung, die nach dem Prinzip der Clean Architecture aufgebaut ist. Ziel ist es, eine robuste und skalierbare Architektur für das Management-System bereitzustellen.

## Projektstruktur
Die Projektlösung besteht aus mehreren Projekten, die jeweils spezifische Rollen und Verantwortlichkeiten haben:

- **Management.Api**: Enthält die API-Endpunkte und die Konfiguration der Dependency Injection. Diese Schicht dient als Schnittstelle für externe Clients und bietet Zugriff auf die Geschäftslogik.
- **Management.Application**: Definiert die Anwendungsdienste, die die Geschäftslogik implementieren. Beispiele sind Dienste für die Authentifizierung und JWT-Verwaltung.
- **Management.Core**: Enthält die Domänenmodelle und Schnittstellen. Dieses Projekt stellt die zentralen Komponenten der Geschäftslogik bereit.
- **Management.Infrastructure**: Ein spezialisiertes Infrastrukturprojekt zur Verwaltung von Identity. Hier ist die Anordnung "out of the box".
- **Management.Infrastructure.Department**: Implementiert Datenzugriff, Datenmigrationen und Repositories. Es stellt die Verbindung zwischen der Geschäftslogik und der Datenbank her.
- **Request**: Enthält Test-API-Calls für die verschiedenen Endpunkte der Anwendung.
  
### Detaillierte Struktur
Die detaillierte Verzeichnisstruktur des Projekts ist wie folgt:

```bash
├───request
│   ├───Authentication
│   └───Department
├───src
│   ├───Management.Api
│   │   ├───Extensions
│   │   └───Groups
│   ├───Management.Application
│   │   └───Services
│   │       ├───Authentication
│   │       ├───DepartmentMediatorService
│   │       └───JWTService
│   ├───Management.Core
│   │   ├───Interfaces
│   │   │   ├───DepartmentInterfaces
│   │   │   │   └───RepositoryInterfaces
│   │   │   └───JWT
│   │   ├───Models
│   │   │   ├───Authentication
│   │   │   └───DepartmentModels
│   │   │       ├───Datenbank
│   │   │       └───DepartmentMediator
│   │   │           ├───Command
│   │   │           ├───Queries
│   │   │           └───Responses
│   ├───Management.Infrastructure
│   │   ├───Authentication
│   │   ├───Data
│   │   ├───Migrations
│   │   └───Repositories
│   │       └───SettingsRepository
│   ├───Management.Infrastructure.Department
│   │   ├───Data
│   │   ├───Migrations
│   │   └───Repositories
│   │       └───DepartmentRepositories
│   └───Management.Infrastructure.Energy
└───test
    ├───IntegrationTests
    │   ├───Management.Api.IntegrationTests
    │   │   └───Groups
    │   │       └───GroupDepartmentTests
    │   ├───Management.Application.IntegrationTests
    │   │   └───Services
    │   │       ├───DepartmentCommandHandlerTests
    │   │       └───DepartmentQueryHandlerTests
    │   └───Management.Infrastructure.Department.IntegrationTests
    │       ├───Helpers
    │       └───Repositories
    │           └───Department
    └───UnitTests
        └───Management.Infrastructure.Department.UnitTests
            ├───Helpers
            └───Repositories
                └───Department
