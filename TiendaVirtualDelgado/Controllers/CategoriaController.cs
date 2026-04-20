using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TiendaVirtualDelgado.Data;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualDelgado.Models;

namespace TiendaVirtualDelgado.Controllers
{
    public class CategoriaController : Controller
    {
        // index categoria

        private readonly TiendaContext _context;

        public CategoriaController(TiendaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var categorias = _context.categorias.ToList();
            return View(categorias);
        }

        //formulario crear
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }


        // guardar categoria
        [HttpPost]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _context.categorias.Add(categoria);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            // Si el modelo no es válido, regresa a la vista para mostrar errores
            return View(categoria);
        }

        // formulario editar
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var categoria = _context.categorias.Find(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // actualizar categoria
        [HttpPost]
        public IActionResult Edit(int id, Categoria categoria)
        {
          
                _context.Update(categoria);
                _context.SaveChanges();
                return RedirectToAction("Index");
          
        }

        // eliminar categoria

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var categoria = _context.categorias.Find(id);

            _context.categorias.Remove(categoria);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}



