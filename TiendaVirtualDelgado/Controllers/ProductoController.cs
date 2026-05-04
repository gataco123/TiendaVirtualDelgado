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
        public IActionResult Create(Producto producto, IFormFile imagen)
        {
            if (imagen != null)
            {
                var ruta = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/images", imagen.FileName);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }
                producto.ImagenUrl = "/images/" + imagen.FileName;
            }
           
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
        // ACTUALIZAR PRODUCTO
        [HttpPost]
        public IActionResult Edit(Producto producto, IFormFile imagen)
        {
            var productoBD = _context.productos.Find(producto.Id);
            if (productoBD == null)
                return NotFound();

            // Actualizar datos normales
            productoBD.Nombre = producto.Nombre;
            productoBD.Precio = producto.Precio;
            productoBD.Stock = producto.Stock;
            productoBD.CategoriaId = producto.CategoriaId;

            // Si sube nueva imagen
            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(carpeta))
                {
                    Directory.CreateDirectory(carpeta);
                }

                var ruta = Path.Combine(carpeta, imagen.FileName);

                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    imagen.CopyTo(stream);
                }

                productoBD.ImagenUrl = "/images/" + imagen.FileName;
            }

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