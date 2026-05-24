create database use  ShipmentDB;
use  ShipmentDB;
CREATE TABLE Usuarios (
    Id       INT IDENTITY(1,1) PRIMARY KEY,
    Email    NVARCHAR(100) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL
);

-- Usuario de prueba
INSERT INTO Usuarios (Email, Password) VALUES ('admin@test.com', 'Admin123');


USE ShipmentDB;
GO

CREATE TABLE Shipments (
    Id                   INT IDENTITY(1,1) PRIMARY KEY,
    TrackingNumber       NVARCHAR(50)  NOT NULL UNIQUE,
    PaisOrigen           NVARCHAR(100) NOT NULL,
    PaisDestino          NVARCHAR(100) NOT NULL,
    CiudadOrigen         NVARCHAR(100) NOT NULL,
    CiudadDestino        NVARCHAR(100) NOT NULL,
    NombreRemitente      NVARCHAR(150) NOT NULL,
    NombreDestinatario   NVARCHAR(150) NOT NULL,
    DescripcionMercancia NVARCHAR(500) NOT NULL,
    PesoKg               DECIMAL(10,2) NOT NULL,
    Estado               INT           NOT NULL DEFAULT 0,
    FechaCreacion        DATETIME      NOT NULL DEFAULT GETDATE(),
    FechaEstimadaEntrega DATETIME      NOT NULL
);

