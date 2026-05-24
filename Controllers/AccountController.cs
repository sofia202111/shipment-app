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

    public IActionResult Login() => View();

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        using var con = new SqlConnection(_conn);
        con.Open();

        var cmd = new SqlCommand(
            "SELECT * FROM Usuarios WHERE Email = @email AND Password = @password", con);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);

        int encontrado = (int)cmd.ExecuteScalar();

        if (encontrado == 0)
        {
            ViewBag.Error = "Correo o contraseña incorrectos.";
            return View();
        }

        HttpContext.Session.SetString("usuario", email);
        return RedirectToAction("Index", "Shipments");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}