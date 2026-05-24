
INSERT INTO Shipments
(
    TrackingNumber,
    PaisOrigen,
    PaisDestino,
    CiudadOrigen,
    CiudadDestino,
    NombreRemitente,
    NombreDestinatario,
    DescripcionMercancia,
    PesoKg,
    Estado,
    FechaCreacion,
    FechaEstimadaEntrega
)
VALUES
(
    'TRK001',
    'Colombia',
    'USA',
    'Bogotá',
    'Miami',
    'Sofía',
    'Karen',
    'Ropa y accesorios',
    2.50,
    0,
    GETDATE(),
    '2026-05-30'
);
--Consulta por estado
SELECT *
FROM Shipments
WHERE Estado = 1;
--Consulta por Origen y por Destino
select * from Shipments where PaisOrigen='Colombia';
select * from Shipments where PaisDestino='Estados Unidos ';
-- Consulta por fecha de creación 
SELECT *
FROM Shipments
WHERE FechaCreacion
BETWEEN '2026-05-01' AND '2026-05-31';

-- Consulta por fehca estimada de entrega 

select *  from Shipments where FechaEstimadaEntrega ='2026-05-31';