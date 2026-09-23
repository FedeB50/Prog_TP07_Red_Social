using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP05.Models;

namespace TP05.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult guardarRegistro(string username, string password, string nombre, string apellido)
    {
        DB db = new DB();
        if (db.getUsername(username))
        {
            ViewBag.error = "El usuario ya existe";
            return RedirectToAction("Error");
        }
        db.Registrar(username, password, nombre, apellido);
        HttpContext.Session.SetString("username", username);
        HttpContext.Session.SetString("nombre", nombre);
        HttpContext.Session.SetString("apellido", apellido);
        return RedirectToAction("Index");
    }
    
    public IActionResult cerrarSesion()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult eliminarCuenta()
    {
        DB db = new DB();
        db.EliminarCuenta(HttpContext.Session.GetString("username"));
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    [HttpPost]
    public IActionResult hacerLogin(string username, string password)
    {
        DB db = new DB();
        if (db.getUsername(username))
        {
            if (db.getPassword(username, password))
            {
                Usuarios usuario = db.getUsuario(username);
                HttpContext.Session.SetString("username", username);
                HttpContext.Session.SetString("nombre", usuario.nombre);
                HttpContext.Session.SetString("apellido", usuario.apellido);
                return RedirectToAction("Index");
            }
            HttpContext.Session.SetString("error", "Contraseña incorrecta");
            return View("Error");
        }    
        HttpContext.Session.SetString("error", "Usuario incorrecto");
        return View("Error");
    }

    public IActionResult Index()
    {
        DB bd = new DB();
        if (bd.getUsername(HttpContext.Session.GetString("username")))
        {
            ViewBag.nombre = HttpContext.Session.GetString("nombre");
            ViewBag.apellido = HttpContext.Session.GetString("apellido");
            return View();
        }
        return RedirectToAction ("Login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        ViewBag.error = HttpContext.Session.GetString("error");
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
