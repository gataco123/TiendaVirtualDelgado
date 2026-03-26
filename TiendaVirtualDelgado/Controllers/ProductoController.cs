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
            var productos = _context.productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        //formulario crear
        public IActionResult Create()
        {
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
            var producto = _context.productos.Find(id);
            ViewBag.Categorias = _context.categorias.ToList();
            return View(producto);
        }

        // actualizar producto
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            _context.productos.Update(producto);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        //elinar producto
        public IActionResult Delete(int id)
        {
            var producto = _context.productos.Find(id);
            
                _context.productos.Remove(producto);
                _context.SaveChanges();
            
            return RedirectToAction("Index");
        }
    }
}