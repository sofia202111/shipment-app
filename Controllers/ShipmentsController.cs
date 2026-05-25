using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ShipmentsApp.Models;

namespace ShipmentsApp.Controllers;

public class ShipmentsController : Controller
{
    private readonly string _conn;

    public ShipmentsController(IConfiguration config)
    {
        _conn = config.GetConnectionString("DefaultConnection")!;
    }

    private bool Login()
        => HttpContext.Session.GetString("usuario") != null;

    // ── LISTADO 
    public IActionResult Index()
    {
        if (!Login()) return RedirectToAction("Login", "Account");

        var lista = new List<Shipment>();

        using var con = new SqlConnection(_conn);
        con.Open();

        var cmd = new SqlCommand(
            "SELECT * FROM Shipments ORDER BY FechaCreacion DESC", con);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(LeerShipment(reader));
        }

        return View(lista);
    }

    // ── DETALLE 
    public IActionResult Details(int id)
    {
        if (!Login()) return RedirectToAction("Login", "Account");

        var envi = BuscarPorId(id);
        if (envi == null) return NotFound();
        return View(envi);
    }

    // ── CREAR FORMULARIO
    public IActionResult Create()
    {
        if (!Login()) return RedirectToAction("Login", "Account");
        return View();
    }

    // ── CREAR 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Shipment envio)
    {
        if (!Login()) return RedirectToAction("Login", "Account");

        if (envio.PaisOrigen == envio.PaisDestino)
            ModelState.AddModelError("PaisDestino",
                "El país destino no puede ser igual al origen.");

        if (envio.FechaEstimadaEntrega < DateTime.Today)
            ModelState.AddModelError("FechaEstimadaEntrega",
                "La fecha no puede ser en el pasado.");

        if (!ModelState.IsValid) return View(envio);

        using var con = new SqlConnection(_conn);
        con.Open();

        var cmd = new SqlCommand(@"
            INSERT INTO Shipments (
                TrackingNumber, PaisOrigen, PaisDestino,
                CiudadOrigen, CiudadDestino,
                NombreRemitente, NombreDestinatario,
                DescripcionMercancia, PesoKg,
                Estado, FechaCreacion, FechaEstimadaEntrega)
            VALUES (
                @tracking, @paisOrigen, @paisDestino,
                @ciudadOrigen, @ciudadDestino,
                @remitente, @destinatario,
                @descripcion, @peso,
                0, GETDATE(), @fechaEntrega)", con);

        cmd.Parameters.AddWithValue("@tracking",      envio.TrackingNumber);
        cmd.Parameters.AddWithValue("@paisOrigen",    envio.PaisOrigen);
        cmd.Parameters.AddWithValue("@paisDestino",   envio.PaisDestino);
        cmd.Parameters.AddWithValue("@ciudadOrigen",  envio.CiudadOrigen);
        cmd.Parameters.AddWithValue("@ciudadDestino", envio.CiudadDestino);
        cmd.Parameters.AddWithValue("@remitente",     envio.NombreRemitente);
        cmd.Parameters.AddWithValue("@destinatario",  envio.NombreDestinatario);
        cmd.Parameters.AddWithValue("@descripcion",   envio.DescripcionMercancia);
        cmd.Parameters.AddWithValue("@peso",          envio.PesoKg);
        cmd.Parameters.AddWithValue("@fechaEntrega",  envio.FechaEstimadaEntrega);

        cmd.ExecuteNonQuery();

        TempData["Exito"] = "Envío creado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    // ── EDITAR FORMULARIO ──────────────────────────────────
    public IActionResult Edit(int id)
    {
        if (!Login()) return RedirectToAction("Login", "Account");

        var envio = BuscarPorId(id);
        if (envio == null) return NotFound();

        if (envio.Estado == 2 || envio.Estado == 3)
        {
            TempData["Error"] = "No se puede editar un envío Entregado o Cancelado.";
            return RedirectToAction(nameof(Index));
        }

        return View(envio);
    }

    // ── EDITAR 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Shipment envio)
    {
        if (!Login()) return RedirectToAction("Login", "Account");

        var existente = BuscarPorId(id);
        if (existente == null) return NotFound();

        if (existente.Estado == 2 || existente.Estado == 3)
        {
            TempData["Error"] = "No se puede editar un envío Entregado o Cancelado.";
            return RedirectToAction(nameof(Index));
        }

        if (envio.PaisOrigen == envio.PaisDestino)
            ModelState.AddModelError("PaisDestino",
                "El país destino no puede ser igual al origen.");

        if (!ModelState.IsValid) return View(envio);

        using var con = new SqlConnection(_conn);
        con.Open();

        var cmd = new SqlCommand(@"
            UPDATE Shipments SET
                PaisOrigen           = @paisOrigen,
                PaisDestino          = @paisDestino,
                CiudadOrigen         = @ciudadOrigen,
                CiudadDestino        = @ciudadDestino,
                NombreRemitente      = @remitente,
                NombreDestinatario   = @destinatario,
                DescripcionMercancia = @descripcion,
                PesoKg               = @peso,
                Estado               = @estado,
                FechaEstimadaEntrega = @fechaEntrega
            WHERE Id = @id", con);

        cmd.Parameters.AddWithValue("@paisOrigen",   envio.PaisOrigen);
        cmd.Parameters.AddWithValue("@paisDestino",  envio.PaisDestino);
        cmd.Parameters.AddWithValue("@ciudadOrigen", envio.CiudadOrigen);
        cmd.Parameters.AddWithValue("@ciudadDestino",envio.CiudadDestino);
        cmd.Parameters.AddWithValue("@remitente",    envio.NombreRemitente);
        cmd.Parameters.AddWithValue("@destinatario", envio.NombreDestinatario);
        cmd.Parameters.AddWithValue("@descripcion",  envio.DescripcionMercancia);
        cmd.Parameters.AddWithValue("@peso",         envio.PesoKg);
        cmd.Parameters.AddWithValue("@estado",       envio.Estado);
        cmd.Parameters.AddWithValue("@fechaEntrega", envio.FechaEstimadaEntrega);
        cmd.Parameters.AddWithValue("@id",           id);

        cmd.ExecuteNonQuery();

        TempData["Exito"] = "Envío actualizado.";
        return RedirectToAction(nameof(Index));
    }

    // ── CANCELAR FORMULARIO ────────────────────────────────
    public IActionResult Delete(int id)
    {
        if (!Login()) return RedirectToAction("Login", "Account");
        var envio = BuscarPorId(id);
        if (envio == null) return NotFound();
        return View(envio);
    }

    // ── CANCELAR CONFIRMAR ─────────────────────────────────
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!Login()) return RedirectToAction("Login", "Account");

        using var con = new SqlConnection(_conn);
        con.Open();

        // Cambia estado a Cancelado (3), no borra el registro
        var cmd = new SqlCommand(
            "UPDATE Shipments SET Estado = 3 WHERE Id = @id", con);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.ExecuteNonQuery();

        TempData["Exito"] = "Envío cancelado.";
        return RedirectToAction(nameof(Index));
    }


    // Busca un shipment por Id
    private Shipment? BuscarPorId(int id)
    {
        using var con = new SqlConnection(_conn);
        con.Open();

        var cmd = new SqlCommand(
            "SELECT * FROM Shipments WHERE Id = @id", con);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = cmd.ExecuteReader();
        if (reader.Read())
            return LeerShipment(reader);

        return null;
    }

// lee los tipos de datoa en la base de datos 
    private static Shipment LeerShipment(SqlDataReader r) => new()
    {
        Id                   = (int)r["Id"],
        TrackingNumber       = r["TrackingNumber"].ToString()!,
        PaisOrigen           = r["PaisOrigen"].ToString()!,
        PaisDestino          = r["PaisDestino"].ToString()!,
        CiudadOrigen         = r["CiudadOrigen"].ToString()!,
        CiudadDestino        = r["CiudadDestino"].ToString()!,
        NombreRemitente      = r["NombreRemitente"].ToString()!,
        NombreDestinatario   = r["NombreDestinatario"].ToString()!,
        DescripcionMercancia = r["DescripcionMercancia"].ToString()!,
        PesoKg               = (decimal)r["PesoKg"],
        Estado               = (int)r["Estado"],
        FechaCreacion        = (DateTime)r["FechaCreacion"],
        FechaEstimadaEntrega = (DateTime)r["FechaEstimadaEntrega"]
    };
}