using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TiendaVirtualDelgado.Data;
using Microsoft.EntityFrameworkCore;
using TiendaVirtualDelgado.Models;

namespace TiendaVirtualDelgado.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var productos = _context.productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        //formulario crear
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Categorias = _context.categorias.ToList();
            return View();
        }

        // guardar producto
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
           
            _context.productos.Add(producto);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        // formulario editar
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var producto = _context.productos.Find(id);
            ViewBag.Categorias = _context.categorias.ToList();
            return View(producto);
        }

        // actualizar producto
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            _context.productos.Update(producto);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //elinar producto
        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var rol = HttpContext.Session.GetString("Rol");
            if (rol != "admin")
            {
                return RedirectToAction("Index");
            }

            var producto = _context.productos.Find(id);
            
                _context.productos.Remove(producto);
                _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}