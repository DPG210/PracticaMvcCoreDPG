
using Microsoft.EntityFrameworkCore;
using PracticaMvcCoreDPG.Models;

namespace PracticaMvcCoreDPG.Data
{
    public class LibrosContext:DbContext
    {
        public LibrosContext(DbContextOptions<LibrosContext> options) : base(options) { }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<VistaPedidos> VistaPedidos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Generos> Generos { get; set; }
    }
}
