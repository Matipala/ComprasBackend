# InventorySaaS - Compras

Servicio de compras para el ecosistema InventorySaaS. Gestiona la recepcion de items, registro de compras y sincronizacion con Inventario.

## Tecnologias utilizadas
- Runtime: .NET 10.0
- ORM: Entity Framework Core 9.0
- Base de Datos: PostgreSQL 15+
- Documentacion: Swagger

## Puertos por defecto
- HTTP: http://localhost:5006

## Ejecucion con Docker (recomendado)
Desde la raiz del workspace:
```bash
docker compose up --build
```
Esto levanta PostgreSQL y los tres backends. Este servicio queda disponible en http://localhost:5006.

### Archivo compose
Desde la raiz del workspace:
```bash
services:
  db:
    image: postgres:15
    environment:
      POSTGRES_DB: ${POSTGRES_DB}
      POSTGRES_USER: ${POSTGRES_USER}
      POSTGRES_PASSWORD: ${POSTGRES_PASSWORD}
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

  inventory-api:
    build:
      context: ./InventorySaaSBackend
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: >
        Host=db;
        Port=5432;
        Database=${POSTGRES_DB};
        Username=${POSTGRES_USER};
        Password=${POSTGRES_PASSWORD}
    ports:
      - "5140:8080"
    depends_on:
      - db

  ventas-api:
    build:
      context: ./VentasBackend
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: >
        Host=db;
        Port=5432;
        Database=${POSTGRES_DB};
        Username=${POSTGRES_USER};
        Password=${POSTGRES_PASSWORD}
      InventoryApi__BaseUrl: http://inventory-api:8080
      Integration__InventarioBaseUrl: http://inventory-api:8080
    ports:
      - "5005:8080"
    depends_on:
      - db
      - inventory-api

  compras-api:
    build:
      context: ./ComprasBackend
    environment:
      ASPNETCORE_ENVIRONMENT: Development
      ConnectionStrings__DefaultConnection: >
        Host=db;
        Port=5432;
        Database=${POSTGRES_DB};
        Username=${POSTGRES_USER};
        Password=${POSTGRES_PASSWORD}
      InventoryApi__BaseUrl: http://inventory-api:8080
    ports:
      - "5006:8080"
    depends_on:
      - db
      - inventory-api

volumes:
  pgdata:
```

## Ejecucion local (sin Docker)
1. Configura la conexion a la base de datos en appsettings.json o appsettings.Development.json:
	```json
	"ConnectionStrings": {
	  "DefaultConnection": "Host=localhost;Port=5432;Database=inventorysaas;Username=postgres;Password=postgres"
	}
	```
2. Configura la URL del API de inventario:
	```json
	"InventoryApi": {
	  "BaseUrl": "http://localhost:0000"
	}
	```
3. Restaura dependencias y corre migraciones:
	```bash
	dotnet restore
	dotnet ef database update
	```
4. Inicia el servicio:
	```bash
	dotnet run
	```

## Salud
- GET /health
