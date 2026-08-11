# DuesApi - Sistema de Gestión de Cuotas Condominales

<p align="center">
  <h1 align="center">DuesApi</h1>
  <p align="center">
    Sistema web para la gestión de cuotas, residentes, apartamentos y pagos de condominios.
  </p>
</p>

<p align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge\&logo=dotnet\&logoColor=white)
![C#](https://img.shields.io/badge/C%23-13-239120?style=for-the-badge\&logo=csharp\&logoColor=white)
![React](https://img.shields.io/badge/React-19-61DAFB?style=for-the-badge\&logo=react\&logoColor=black)
![Vite](https://img.shields.io/badge/Vite-8-646CFF?style=for-the-badge\&logo=vite\&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-Database-CC2927?style=for-the-badge\&logo=microsoftsqlserver\&logoColor=white)

</p>

## Descripción

DuesApi es una aplicación web desarrollada para facilitar la administración de cuotas condominales y el control de pagos.

El sistema permite centralizar la información relacionada con residentes, apartamentos, cuotas y pagos dentro de una plataforma que conecta un backend desarrollado en C#/.NET con un frontend desarrollado en React.

El proyecto fue desarrollado aplicando una arquitectura por capas, buscando separar las responsabilidades del sistema y facilitar su mantenimiento, escalabilidad y evolución.

## Características principales

* Gestión de residentes.
* Gestión de apartamentos.
* Administración de cuotas condominales.
* Registro y control de pagos.
* Consulta de información mediante una API REST.
* Separación de responsabilidades mediante arquitectura por capas.
* Interfaz web desarrollada con React.
* Comunicación entre frontend y backend mediante HTTP.
* Visualización de información mediante gráficos y componentes estadísticos.
* Navegación entre diferentes módulos de la aplicación.

## Arquitectura

El proyecto está dividido principalmente en dos grandes componentes:

```text
DuesApi
│
├── DuesBackend
│   ├── DuesApi
│   ├── DuesBusiness
│   ├── DuesDomain
│   └── DuesInfrastructure
│
└── dues-frontend
    ├── public
    ├── src
    ├── index.html
    ├── package.json
    └── vite.config.js
```

### Backend

El backend utiliza una arquitectura separada por responsabilidades:

```text
DuesApi
    │
    ├── Controllers
    └── Dtos

DuesBusiness
    │
    └── Lógica de negocio

DuesDomain
    │
    ├── Core
    └── Entities

DuesInfrastructure
    │
    └── Persistencia y acceso a datos
```

### DuesApi

Es la capa encargada de exponer los endpoints HTTP de la aplicación.

Aquí se encuentran los controladores, DTOs, configuración principal de la aplicación y el punto de entrada `Program.cs`.

### DuesBusiness

Contiene la lógica de negocio de la aplicación.

Su objetivo es evitar que las reglas del sistema estén directamente dentro de los controladores y mantener una separación clara de responsabilidades.

### DuesDomain

Contiene las entidades y elementos principales del dominio de la aplicación.

Entre los conceptos principales del sistema se encuentran:

* Residentes
* Apartamentos
* Cuotas
* Pagos

### DuesInfrastructure

Esta capa se encarga de la comunicación con la infraestructura y la persistencia de datos.

Su función es conectar la lógica de la aplicación con los mecanismos utilizados para almacenar y recuperar información.

## Frontend

El frontend se encuentra dentro de `dues-frontend` y está desarrollado utilizando React y Vite.

Entre las dependencias principales se encuentran:

* React
* React DOM
* React Router DOM
* Axios
* Recharts
* Vite

Axios se utiliza para realizar las comunicaciones HTTP con el backend, mientras que React Router permite manejar la navegación de la aplicación y Recharts permite representar información mediante gráficos.

## Tecnologías utilizadas

### Backend

* C#
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* Arquitectura por capas
* REST API

### Frontend

* React 19
* Vite
* React Router
* Axios
* Recharts
* JavaScript

### Base de datos

* Microsoft SQL Server
* Entity Framework Core

## Requisitos

Antes de ejecutar el proyecto debes tener instalado:

* .NET SDK 10
* Node.js
* npm
* Microsoft SQL Server
* Git

Puedes comprobar las instalaciones con:

```bash
dotnet --version
node --version
npm --version
git --version
```

## Instalación

### 1. Clonar el repositorio

```bash
git clone https://github.com/benjamin23k/C-SHAP-2.git
```

Entrar al proyecto:

```bash
cd C-SHAP-2
```

Cambiar a la rama del proyecto:

```bash
git checkout FinalProyect
```

### 2. Configurar el Backend

Entrar a la carpeta:

```bash
cd DuesBackend
```

Restaurar las dependencias:

```bash
dotnet restore
```

Compilar el proyecto:

```bash
dotnet build
```

### 3. Configurar la base de datos

Configura la cadena de conexión de SQL Server en el archivo:

```text
DuesBackend/DuesApi/appsettings.json
```

Ejemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DuesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> La cadena de conexión debe adaptarse a la configuración de SQL Server de cada entorno.

Si el proyecto contiene migraciones de Entity Framework Core, pueden aplicarse mediante:

```bash
dotnet ef database update
```

### 4. Ejecutar el Backend

Desde la carpeta del proyecto API:

```bash
cd DuesApi
dotnet run
```

Una vez iniciado, la API estará disponible en la dirección indicada por ASP.NET Core en la consola.

## Configuración del Frontend

Abrir otra terminal y entrar al frontend:

```bash
cd dues-frontend
```

Instalar las dependencias:

```bash
npm install
```

Ejecutar el servidor de desarrollo:

```bash
npm run dev
```

Vite iniciará el frontend y mostrará en la terminal la dirección local donde se encuentra disponible.

## Scripts disponibles

Dentro de `dues-frontend`:

```bash
npm run dev
```

Inicia el servidor de desarrollo.

```bash
npm run build
```

Genera la versión de producción del frontend.

```bash
npm run preview
```

Permite visualizar localmente la versión generada para producción.

```bash
npm run lint
```

Ejecuta las comprobaciones de calidad del código configuradas en el proyecto.

Estos scripts están definidos actualmente en el `package.json` del frontend.

## Flujo de funcionamiento

El funcionamiento general de DuesApi puede representarse de la siguiente manera:

```text
             ┌─────────────────────┐
             │       Usuario       │
             └──────────┬──────────┘
                        │
                        ▼
             ┌─────────────────────┐
             │   React Frontend    │
             │       + Vite        │
             └──────────┬──────────┘
                        │
                        │ HTTP / Axios
                        ▼
             ┌─────────────────────┐
             │      DuesApi        │
             │    REST API         │
             └──────────┬──────────┘
                        │
                        ▼
             ┌─────────────────────┐
             │   DuesBusiness      │
             │  Lógica de negocio  │
             └──────────┬──────────┘
                        │
                        ▼
             ┌─────────────────────┐
             │     DuesDomain      │
             │ Entidades / Dominio │
             └──────────┬──────────┘
                        │
                        ▼
             ┌─────────────────────┐
             │ DuesInfrastructure  │
             │   Persistencia      │
             └──────────┬──────────┘
                        │
                        ▼
             ┌─────────────────────┐
             │    SQL Server       │
             └─────────────────────┘
```

## Objetivo del proyecto

El objetivo principal de DuesApi es proporcionar una solución tecnológica para administrar de forma organizada la información relacionada con un condominio.

La aplicación busca reducir la dependencia de procesos manuales y proporcionar una plataforma centralizada para consultar y gestionar información administrativa.

Además del objetivo funcional, el proyecto sirve como aplicación práctica de conceptos de desarrollo de software como:

* Programación orientada a objetos.
* Desarrollo de APIs REST.
* Arquitectura por capas.
* Separación de responsabilidades.
* Acceso a datos mediante Entity Framework Core.
* Desarrollo de interfaces con React.
* Comunicación entre frontend y backend.
* Manejo de rutas y navegación.
* Visualización de datos.

## Estructura del repositorio

```text
C-SHAP-2/
│
├── DuesBackend/
│   │
│   ├── DuesApi/
│   │   ├── Controllers/
│   │   ├── Dtos/
│   │   ├── Properties/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── DuesApi.csproj
│   │
│   ├── DuesBusiness/
│   │
│   ├── DuesDomain/
│   │   ├── Core/
│   │   ├── Entities/
│   │   └── Dues.Domain.csproj
│   │
│   └── DuesInfrastructure/
│
├── dues-frontend/
│   ├── public/
│   ├── src/
│   ├── index.html
│   ├── package.json
│   ├── package-lock.json
│   └── vite.config.js
│
└── README.md
```

La estructura anterior corresponde a la rama `FinalProyect` del repositorio.

## Estado del proyecto

Actualmente el proyecto se encuentra en desarrollo como proyecto académico, con una implementación funcional de frontend y backend.

La arquitectura está preparada para continuar incorporando funcionalidades y mejoras.

## Mejoras futuras

Algunas funcionalidades que pueden incorporarse posteriormente son:

* Sistema completo de autenticación y autorización.
* Roles para administradores, residentes y empleados.
* Notificaciones de cuotas pendientes.
* Generación de reportes en PDF.
* Historial detallado de pagos.
* Dashboard administrativo avanzado.
* Exportación de información a Excel.
* Integración con servicios de correo electrónico.
* Sistema de auditoría.
* Despliegue en la nube.
* Pruebas automatizadas.
* Dockerización de los servicios.

## Autor

Desarrollado por **Wilson Benjamin Rosa Rosario**.

GitHub: [@benjamin23k](https://github.com/benjamin23k)

## Repositorio

Proyecto disponible en GitHub:

https://github.com/benjamin23k/C-SHAP-2/tree/FinalProyect

## Licencia

Este proyecto fue desarrollado con fines académicos y educativos.
