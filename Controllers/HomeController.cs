using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using TP07_Barg.Models;

namespace TP07_Barg.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
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
            return RedirectToAction("Error");
        }    
        HttpContext.Session.SetString("error", "Usuario incorrecto");
        return RedirectToAction("Error");
    }

    public IActionResult Index()
    {
        DB bd = new DB();
        if (bd.getUsername(HttpContext.Session.GetString("username")))
        {
            ViewBag.publicaciones = bd.getPublicaciones();
            ViewBag.comentarios = bd.getComentarios();
            ViewBag.Likes = new Dictionary<int, int>();
            foreach(Publicaciones publicacion in ViewBag.publicaciones)
            {
                ViewBag.Likes[publicacion.id] = bd.GetLikes(publicacion.id);
            }
            ViewBag.arroba = "@";
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

    [HttpPost]
    public async Task<IActionResult> hacerPublicacion(IFormFile img, string titulo, string descripcion)
    {
        int idUsuario = int.Parse(HttpContext.Session.GetString("idUsuario"));
        DateTime fechaHora = DateTime.Now;

        //Le pregunté a copilot cómo subir la imagen a la carpeta wwwroot/images
        string uploadsFolder = Path.Combine("wwwroot", "images");
        Directory.CreateDirectory(uploadsFolder);

        string ext = Path.GetExtension(img.FileName);
        string fileNameOnly = Path.GetFileNameWithoutExtension(img.FileName);
        string nombreImagen = fileNameOnly + "_" + Guid.NewGuid().ToString("N") + ext;
        string filePath = Path.Combine(uploadsFolder, nombreImagen);

        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            await img.CopyToAsync(stream);
        }

        // Guardar en la base de datos la publicación
        DB db = new DB();
        db.CrearPublicacion(nombreImagen, idUsuario, descripcion, titulo, fechaHora);

        return RedirectToAction("Index");
    }

    public IActionResult DevPublicacion()
    {
        DB bd = new DB();
        if (bd.getUsername(HttpContext.Session.GetString("username")))
        {
            return View();
        }
        return RedirectToAction("Login");
    }

    [HttpGet]
    public string Like(int IdPublicacion)
    {
        DB MiBD = new DB();
        MiBD.DarLike(IdPublicacion, int.Parse(HttpContext.Session.GetString("idUsuario")));
        return MiBD.GetLikes(IdPublicacion).ToString();
    }
}
