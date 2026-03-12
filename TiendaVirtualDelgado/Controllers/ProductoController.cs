using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TiendaVirtualDelgado.Models;

namespace TiendaVirtualDelgado.Controllers
{
    public class ProductoController : Controller
    {
        public IActionResult Index()
        {
            var productos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "laptop", Precio = 3000, Stock = 5},
                new Producto { Id = 2, Nombre = "Mouse", Precio = 80, Stock = 0 },
            };
            return View(productos);
        }
    }
}