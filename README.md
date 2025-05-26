# Proyecto Blog de Artículos

Sistema de gestión de blog con arquitectura distribuida compuesto por múltiples APIs y frontend para la creación, gestión y visualización de artículos con sistema de comentarios.

## Arquitectura del Sistema

El proyecto está dividido en **3 soluciones independientes** que trabajan en conjunto:

### BlogApp_Auth
**API de Autenticación y Gestión de Usuarios**
- Manejo de registro y autenticación de usuarios
- Generación y validación de tokens JWT
- Gestión de perfiles de usuario

### BlogApp_Articles
**API de Gestión de Contenido**
- CRUD completo de artículos
- Sistema de comentarios
- Gestión de categorías y etiquetas
  
### BlogApp_FrontEnd  
**Frontend con Razor Pages**
- Interfaz web para visualización de contenido
- Experiencia de usuario optimizada para lectura de artículos
- Navegación intuitiva y responsive

## Instrucciones de Ejecución

### Prerrequisitos
- .NET 8 SDK instalado
- Visual Studio 2022 o Visual Studio Code
- Git (para clonar el repositorio)

### Pasos para ejecutar
1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/DiloGunz/MicroservicesItelligent
   ```

2. **Ejecutar las 3 soluciones simultáneamente**
   - Abrir cada proyecto en una instancia separada de Visual Studio/VS Code
   - Ejecutar cada proyecto usando `dotnet run` o F5 en Visual Studio
   - Asegurarse de que los 3 servicios estén corriendo antes de usar la aplicación

3. **Verificar que los servicios estén activos**
   - BlogApp_Auth: `https://localhost:7274`
   - BlogApp_Articles: `https://localhost:7292`
   - BlogApp_FrontEnd: `https://localhost:7158`  
   
## 👥 Usuarios de Prueba

El sistema incluye usuarios precargados para testing:

| Usuario | Contraseña | Rol |
|---------|------------|-----|
| `admin` | `admin` | admin |
| `user1` | `user1` | usuario |
| `user2` | `user2` | usuario |
| `user3` | `user3` | usuario |
| `user4` | `user4` | usuario |

> **Nota:** La contraseña de cada usuario es idéntica a su nombre de usuario.

## 🛠️ Stack Tecnológico

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM con enfoque Code First
- **SQLite** - Base de datos embebida
- **JWT (JSON Web Tokens)** - Autenticación y autorización
- **MediatR** - Implementación del patrón Mediator
- **ErrorOr** - Manejo funcional de errores
- **Repository Pattern** - Abstracción de la capa de acceso a datos
- **Unit of Work Pattern** - Gestión de transacciones y consistencia de datos

### Frontend
- **Razor Pages** - Framework de UI server-side
- **Bootstrap** - Estructura y estilos
- **JavaScript** - Interactividad del cliente

## 🚦 Estado del Proyecto

- Sistema de autenticación implementado
- CRUD de artículos funcional  
- Sistema de comentarios operativo
- Frontend responsive
- Integración JWT completada
