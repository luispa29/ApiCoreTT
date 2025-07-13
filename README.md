# Proyecto NetCore

Este proyecto es una aplicación desarrollada con [Net Core 8]
## Requisitos

Antes de comenzar, asegúrate de tener instalado lo siguiente:

- Visual Studio 2022
- SDK NetCore 8
- Docker  para la base de datos

## Crear contenedor para la base de datos
1. Abrir cmd y ejecutar los comandos:
   - docker pull mcr.microsoft.com/mssql/server:2022-latest
   - docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Admin123@" \
   -p 1433:1433 --name test --hostname test \
2. Ingrear a la base de datos y ejecutar el comando:
      - Create database TecnicaTT

## Instalación

1. **Clonar el repositorio:**
2. Abrir el proyecto con Visual Studio 2022
3. Revisar cadena de conexion en el archivo appsettings.Development
4. Levantar el proyecto
 
## Ejecutar Pruebas Unitarias.
1. Ir al proyecto ApiCore dar clic y ejecutar pruebas

## Crear Contenedor para el api
1. Ir a la ruta raiz de la solucion y abrir cmd.
2. Abrir el archivo appsettings.json configurar cadena de conexion para la base de datos
3. Ejecutar el comando docker compose up -d --build
4. Ir al swager http://localhost:5002/swagger/index.html
