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
    }
}