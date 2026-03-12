using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TiendaVirtualDelgado.Models;
namespace TiendaVirtualDelgado.Data
{
    public class TiendaContext : DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options)
            : base(options)
        {
        }
        public DbSet<Producto> productos { get; set; }
        public DbSet<Categoria> categorias { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
    }
}

