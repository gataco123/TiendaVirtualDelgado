using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using TiendaVirtualDelgado.Data;
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

        // comprarrrr
        public IActionResult Comprar()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");

            if (carritoJson == null)
                return RedirectToAction("Index");

            var carrito = JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            foreach (var item in carrito)
            {
                var producto = _context.productos.Find(item.ProductoId);

                if (producto != null)
                {
                    if (producto.Stock >= item.Cantidad)
                    {
                        producto.Stock -= item.Cantidad;
                    }
                }
            }

            _context.SaveChanges();

            HttpContext.Session.Remove("Carrito");

            return RedirectToAction("Index");
        }


        // agregar al carrito
        [HttpPost]
        public IActionResult AgregarCarrito(int id, int cantidad)
        {
            var producto = _context.productos.Find(id);

            // ⚠️ VALIDAR STOCK
            if (producto == null || producto.Stock == 0)
            {
                TempData["Error"] = "Producto sin existencias";
                return RedirectToAction("Index");
            }

            // ⚠️ VALIDAR CANTIDAD
            if (cantidad > producto.Stock)
            {
                TempData["Error"] = "No hay disponibles tantas unidades";
                return RedirectToAction("Index");
            }

            var carritoJson = HttpContext.Session.GetString("Carrito");

            List<CarritoItem> carrito;

            if (carritoJson == null)
            {
                carrito = new List<CarritoItem>();
            }
            else
            {
                carrito = System.Text.Json.JsonSerializer
                          .Deserialize<List<CarritoItem>>(carritoJson);
            }

            var item = carrito.FirstOrDefault(p => p.ProductoId == id);

            if (item != null)
            {
                // ⚠️ VALIDAR SUMA TOTAL
                if ((item.Cantidad + cantidad) > producto.Stock)
                {
                    TempData["Error"] = "No hay suficientes unidades disponibles";
                    return RedirectToAction("Index");
                }

                item.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoId = id,
                    Cantidad = cantidad
                });
            }

            HttpContext.Session.SetString(
                "Carrito",
                System.Text.Json.JsonSerializer.Serialize(carrito)
            );

            // ✅ MENSAJE ÉXITO
            TempData["Mensaje"] = "Producto agregado al carrito";

            return RedirectToAction("Index");
        }

        // carrito agregar
        public IActionResult Carrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito;

            if (carritoJson == null)
                carrito = new List<CarritoItem>();
            else
                carrito = JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            var productos = new List<(Producto producto, int cantidad)>();

            foreach (var item in carrito)
            {
                var producto = _context.productos.Find(item.ProductoId);

                if (producto != null)
                {
                    productos.Add((producto, item.Cantidad));
                }
            }

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