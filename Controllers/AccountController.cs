using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ShipmentsApp.Controllers;

public class AccountController : Controller
{
    private readonly string _conn;

    public AccountController(IConfiguration config)
    {
        _conn = config.GetConnectionString("DefaultConnection")!;
    }

    // ── FORMULARIO LOGIN ─────────────────────────────
    public IActionResult Login()
    {
        return View();
    }

    // ── LOGIN ────────────────────────────────────────
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        using var con = new SqlConnection(_conn);
        con.Open();

        var cmd = new SqlCommand(
            "SELECT * FROM Usuarios WHERE Email = @email AND Password = @password",
            con);

        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);

        using var reader = cmd.ExecuteReader();

        // Verifica si existe usuario
        if (reader.Read())
        {
            HttpContext.Session.SetString("usuario", email);

            return RedirectToAction("Index", "Shipments");
        }

        // Si no existe
        ViewBag.Error = "Correo o contraseña incorrectos.";

        return View();
    }

    // ── LOGOUT ───────────────────────────────────────
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login");
    }
}