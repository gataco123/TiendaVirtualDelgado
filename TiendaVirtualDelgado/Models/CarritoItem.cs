using Microsoft.AspNetCore.Mvc;

namespace TiendaVirtualDelgado.Models
{
    public class CarritoItem 
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}
