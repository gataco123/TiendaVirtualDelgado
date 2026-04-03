using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TiendaVirtualDelgado.Data;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualDelgado.Models;

public class UsuarioController : Controller
{
    private readonly TiendaContext _context;

    public UsuarioController(TiendaContext context)
    {
        _context = context;
    }

    // 🔹 LISTAR USUARIOS (ADMIN)
    public IActionResult Index()
    {
        /*var rol = HttpContext.Session.GetString("UsuarioRol");

        if (rol != "Admin")
        {
            return Unauthorized();
        }*/

        var usuarios = _context.usuarios.ToList();
        return View(usuarios);
    }

    // 🔹 CREAR
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Usuario usuario)
    {
        _context.usuarios.Add(usuario);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // 🔹 EDITAR
    public IActionResult Edit(int id)
    {
        var usuario = _context.usuarios.Find(id);
        return View(usuario);
    }

    [HttpPost]
    public IActionResult Edit(Usuario usuario)
    {
        _context.usuarios.Update(usuario);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // 🔹 ELIMINAR
    public IActionResult Delete(int id)
    {
        var usuario = _context.usuarios.Find(id);

        _context.usuarios.Remove(usuario);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}