using Microsoft.EntityFrameworkCore;
using PracticaMvcCoreDPG.Data;
using PracticaMvcCoreDPG.Models;

namespace PracticaMvcCoreDPG.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public object UInt32 { get; private set; }

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libros>> GetAllLibrosAsync()
        {
            return await this.context.Libros.ToListAsync();
        }
        public async Task <List<Libros>> GetLibrosGenerosAsync(int idGenero)
        {
            return await this.context.Libros.Where(z => z.IdGenero == idGenero).ToListAsync();
        }
        public async Task <Libros> DetailsLibroAsync(int idLibro)
        {
            return await this.context.Libros.Where(z => z.IdLibro == idLibro).FirstOrDefaultAsync();
        }
        public async Task<List<Generos>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }
        public async Task<List<Libros>> GetLibrosSessionAsync(List<int> idsLibros)
        {
            var consulta = from datos in this.context.Libros
                           where idsLibros.Contains(datos.IdLibro)
                           select datos;
            var librosDb = await consulta.ToListAsync();
            List<Libros> listaLibros = new List<Libros>();
            foreach (int id in idsLibros)
            {
                Libros libro = librosDb.FirstOrDefault(z => z.IdLibro == id);
                if(libro != null)
                {
                    listaLibros.Add(libro);
                }
            }
            return listaLibros;
        }
        public int GetMaxIdPedido()
        {
            var consulta = from datos in this.context.Pedidos
                           select datos;
            if (this.context.Pedidos.Any())
            {
                return this.context.Pedidos.Max(z => z.IdPedido);
            }
            else
            {
                return 0;
            }
        }
        public int GetMaxIdFactura()
        {
            var consulta = from datos in this.context.Pedidos
                           select datos;
            if (this.context.Pedidos.Any())
            {
                return this.context.Pedidos.Max(z => z.IdFactura);
            }
            else
            {
                return 0;
            }
        }
        public async Task InsertarCompraAsync(List<Libros> libros, int idUsuario)
        {
            
            int nuevoFactura = GetMaxIdFactura() + 1;
            DateTime fechaActual = DateTime.Now;

            foreach(var item in libros)
            {
                Pedido ped = new Pedido
                {
                    IdPedido = GetMaxIdPedido() + 1,
                    IdFactura = nuevoFactura,
                    Fecha = fechaActual,
                    IdLibro = item.IdLibro,
                    IdUsuario = idUsuario,
                    Cantidad = 1
                };
                this.context.Pedidos.Add(ped);
                await this.context.SaveChangesAsync();
            }
           
        }

        public async Task<Usuarios> LogInUsuarioAsync(string username, string password)
        {
            Usuarios usuario = await this.context.Usuarios.FirstOrDefaultAsync
                (z => z.Nombre == username && z.Pass == password);
            return usuario;
        }
        public async Task <Usuarios> DetallesUsuarioAsync(int idUsuario)
        {
            return await this.context.Usuarios.Where(z => z.IdUsuario == idUsuario).FirstOrDefaultAsync();
        }
        public async Task<List<VistaPedidos>> GetPedidosAsync(int idUsuario)
        {
            
            return await this.context.VistaPedidos.Where(z => z.IdUsuario == idUsuario).ToListAsync();
        }
    }
}
