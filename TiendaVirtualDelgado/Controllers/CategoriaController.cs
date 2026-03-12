using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TiendaVirtualDelgado.Models;

namespace TiendaVirtualDelgado.Controllers
{
    public class CategoriaController : Controller
    {

        public IActionResult Index()
        {
            var categorias = new List<Categoria>
             {
                new Categoria { Id =1, Nombre = "Tecnologia", Descripcion = "sistemas de tecnologia",Estado= "activo"},
                new Categoria { Id =2, Nombre = "ropa", Descripcion = "prendas de vsestir",Estado= "activo"},
                new Categoria { Id =2, Nombre = "", Descripcion = "prendas de vsestir",Estado= "activo"},
             };
            return View(categorias);

        }
    }
}



