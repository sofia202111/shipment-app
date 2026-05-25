# ShipmentsApp

Plataforma web para registrar shipments, donde se puede crear, consultar, editar y cambiarle el estado a cada envío. También cuenta con un sistema de login con correo y contraseña para acceder a la aplicación.

---

## Tecnologías utilizadas
Visual studio code version
 1.121.0 
Backend  version: .NET 10 
Base de datos: SQL Server / SQL Server Express version 2022
Acceso a datos: EF Core (DbContext) + SqlCommand directo version 10.0.8 
Frontend: Razor Views + Bootstrap version 5.3

---

## Arquitectura y estructura de carpetas

La solución está organizada en carpetas con responsabilidades claras:

```
ShipmentsApp/
Controllers/
AccountController.cs      # Controla el login, compara correo y contraseña
 ShipmentsController.cs    # Todo el backend: CRUD, validaciones y estados
 Models/
Shipment.cs               # Modelo principal, datos del envío y validaciones
ErrorViewModel.cs         # Maneja y muestra información de errores
Usuario.cs                # Entidad con email y contraseña (públicos para conectar con los demás)
Data/
AppDbContext.cs           # Maneja la conexión entre ASP.NET Core y SQL Server, registra las entidades Shipments y Usuarios
 Views/
 Account/Login.cshtml      # Pantalla de login
 Shipments/                # Create, Edit, Delete, Details, Index
_ViewImports.cshtml       # Importa configuraciones comunes a todas las vistas
 _ViewStart.cshtml         # Define el layout principal de la aplicación
 database/
01_create_tables.sql      # Crea la base de datos y las tablas Usuarios y Shipments
 03_queries.sql            # Consultas y registro de  datos de prueba
appsettings.json           # Cadena de conexión con la base de datos
```
lo quise hacer de la siguiente manera:
En la carpeta de controllers puse dos en el archivo con nombre AccountController.cs ahi se controla la parte de logueo que compare el correo y la contraseña en el archivo llamado ShipmentsController hizo todo el back end con las validaciones requeridas , el crud completo, en la carpeta de data en el archivo AppDbContext es la clase encargada de manejar la conexión entre ASP.NET Core y SQL Server.Allí se registran las entidades principales del sistema como Shipments y Usuarios, en la carpera database estan don archivos uno donde esta la creacion de la base de datos con as tablas como  lo son el de usuario y el de shipments en la carpeta de models estas tres archivos donde el ErrorViewModeles para manejar y mostrar información de errores en la aplicación, en el archivo Shipment.cs es el modelo principal de la aplicación.Representa la entidad shipment y contiene los datos principales del envío, así como validaciones backend y en el archivo de usuario.cs estan las entidades de email y contraseña donde son publicas para que se pueda conectar con los demas, en la carpeta de views es el front  estan los archivos html donde esta la ventana de login,create, delete y  edit donde use un framework front  de boostratp con version 5.3 en cada uno de los archivos puse el color de botones y organizacion front en los dos archivos como en el archivo _ViewImports.cshtml sirve para importar configuraciones comunes a todas las vistas y en el archivo _ViewStart.cshtml define el layout principal de la aplicacióny finalmente el archivo json la conexion netamente de la base de datos
---

## Modelo de datos

**Tabla `Shipments`**

Id  INT IDENTITY  #Clave primaria 
TrackingNumber  NVARCHAR(50) UNIQUE  #Número de guía, obligatorio y único |
PaisOrigen  NVARCHAR(100) # País de origen |
PaisDestino  NVARCHAR(100) # País de destino
CiudadOrigen  NVARCHAR(100) #Ciudad de origen |
CiudadDestino  NVARCHAR(100) #Ciudad de destino |
NombreRemitente  NVARCHAR(150) # Nombre del remitente |
NombreDestinatario  NVARCHAR(150) # Nombre del destinatario |
DescripcionMercancia  NVARCHAR(500) # Descripción de la carga |
PesoKg  DECIMAL(10,2) # Peso en kg, debe ser mayor que cero |
Estado  INT  0=Creado, 1=En tránsito, 2=Entregado, 3=Cancelado |
FechaCreacion  DATETIME # Se asigna automáticamente al crear |
FechaEstimadaEntrega  DATETIME # No puede ser menor a la fecha de creación |

**Tabla `Usuarios`**


 Id  INT IDENTITY 
Email  NVARCHAR(100) UNIQUE 
Password  NVARCHAR(100) 

---

## Pasos para crear la base de datos

Ejecute los scripts en orden desde SQL Server Management Studio (SSMS):

```
1. database/01_create_tables.sql  
2. database/03_queries.sql      
```

---

## Configurar la cadena de conexión

En `appsettings.json` ajuste el valor de `DefaultConnection` según el servidor local:

json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=ShipmentDB;Trusted_Connection=True;TrustServerCertificate=True"
}
---

## Instalar dependencias


 `Microsoft.Data.SqlClient`, instalarlo con:

```bash
dotnet add package Microsoft.Data.SqlClient
```

Este paquete es el que permite usar `SqlConnection` y `SqlCommand` para conectar con SQL Server desde el código C#.

---

## Pasos para ejecutar el backend
El proyecto fue creado desde cero usando la terminal con el siguiente comando:
bashdotnet new mvc -n ShipmentsApp
despues se crea la base de datos ejecutando los scripts SQL en SSMS
Verificar que la cadena de conexión en appsettings.json apunte a tu servidor
Probar que la conexión funcione antes de correr el proyecto
y se ejecuta con el siguiente comando 
dotnet run

---

## Usuario de prueba

Email: admin@test.com 
Contraseña: Admin123 

---

## Pantallas principales

/views
 `/Account/Login`# Pantalla de login 
 `/Shipments` # Listado de todos los envíos 
 `/Shipments/Create` # Crear un nuevo envío 
 `/Shipments/Details/{id}` # Ver detalle de un envío 
 `/Shipments/Edit/{id}` # Editar un envío existente 
 `/Shipments/Delete/{id}` # Cancelar un envío 
---

## Explicación de las consultas SQL

El archivo `03_queries.sql` incluye consultas para:

- Consultar shipments por estado, entonces lo tomo el estado 1 que es en transito 
- Filtrar por país de origen o país de destino

---

## Decisiones técnicas y limitaciones

Intenté mantener la solución simple, funcional y fácil de entender.

Usé ASP.NET Core MVC desde visual studio codeporque me permitía separar la lógica, las vistas y el acceso a datos de forma organizada.

Para el login usé sesiones con `HttpContext.Session` porque el objetivo era tener autenticación básica y control de acceso sencillo, sin agregar complejidad innecesaria.

Para la persistencia usé SQL Server con consultas manuales usando `SqlConnection` y `SqlCommand` porque quería tener más control sobre las consultas y practicar directamente el acceso a datos.

Decidí cancelar shipments cambiando el estado a "Cancelado" en vez de borrar el registro porque en un contexto logístico es importante conservar el historial de los envíos.

Las validaciones principales las hice en el controller para asegurar que las reglas de negocio se cumplieran independientemente del frontend. También usé DataAnnotations en el modelo para complementar las validaciones y simplificar los mensajes de error en las vistas.

Para el frontend usé Razor Views con Bootstrap porque me permitía construir una interfaz sencilla

**Limitación conocida:** la parte que más me costó fue lograr la conexión con la base de datos y que los datos fluyeran correctamente entre capas. 

---
## Uso de inteligencia artificial

Usé IA como apoyo en dos puntos específicos: para saber cómo instalar la librería `Microsoft.Data.SqlClient` y para entender cómo estructurar correctamente la cadena de conexión con SQL Server Express. El resto del código, la lógica, las validaciones y las decisiones del proyecto las tomé yo.

