using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TiendaVirtualDelgado.Data;
using TiendaVirtualDelgado.Helpers;
using TiendaVirtualDelgado.Models;

namespace TiendaVirtualBenavides.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly TiendaContext _context;

        public UsuarioController(TiendaContext context)
        {
            _context = context;
        }

        // Listado de usuarios
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var usuarios = _context.usuarios.ToList();
            return View(usuarios);
        }

        // GET: Mostrar formulario de creación
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        // POST: Guardar nuevo usuario con validación
        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            usuario.Clave = HashHelper.ObtenerHash(usuario.Clave);
            _context.usuarios.Add(usuario);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Mostrar formulario de edición
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
{
    return RedirectToAction("Index", "Login");
}
            var usuario = _context.usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Actualizar usuario con validación
        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.usuarios.Update(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            // Si hay errores (ej. correo inválido), regresa a la vista de edición
            return View(usuario);
        }

        // POST: Eliminar usuario
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var usuario = _context.usuarios.Find(id);
            if (usuario != null)
            {
                _context.usuarios.Remove(usuario);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}