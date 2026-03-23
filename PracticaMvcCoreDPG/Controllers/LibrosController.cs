using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PracticaMvcCoreDPG.Extensions;
using PracticaMvcCoreDPG.Filters;
using PracticaMvcCoreDPG.Models;
using PracticaMvcCoreDPG.Repositories;
using System.Security.Claims;


namespace PracticaMvcCoreDPG.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Libros(int? idgenero, int? idlibro)
        {
            if (idlibro != null)
            {
                List<int> idsLibros;
                if (HttpContext.Session.GetObject<List<int>>("IDSLIBROS") != null)
                {
                    idsLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
                }
                else
                {
                    idsLibros = new List<int>();
                }
                idsLibros.Add(idlibro.Value);
                HttpContext.Session.SetObject("IDSLIBROS", idsLibros);
            }
            if (idgenero == null)
            {
                List<Libros> libros = await this.repo.GetAllLibrosAsync();
                return View(libros);
            }
            else
            {
                List<Libros> libros = await this.repo.GetLibrosGenerosAsync(idgenero.Value);
                return View(libros);
            }
        }
        public async Task<IActionResult> Detalles(int idlibro)
        {
            Libros libro = await this.repo.DetailsLibroAsync(idlibro);
            return View(libro);
        }
        public async Task<IActionResult> Carrito (int? ideliminar)
        {
            List<int> idsLibros =
                HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            if(idsLibros == null)
            {
                ViewData["Mensaje"] = "Sin libros";
            }
            else
            {
                if(ideliminar != null)
                {
                    idsLibros.Remove(ideliminar.Value);
                    if(idsLibros.Count == 0)
                    {
                        HttpContext.Session.Remove("IDSLIBROS");
                    }
                    else
                    {
                        HttpContext.Session.SetObject("IDSLIBROS", idsLibros);
                    }
                }
            }
            List<Libros> libros =
                await this.repo.GetLibrosSessionAsync(idsLibros);
            return View(libros);
        }
        [AuthorizeUsuarios]
        [HttpPost]
        public async Task<IActionResult> Carrito(string accion)
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<int> idsLibros =
                HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            if (idsLibros == null)
            {
                ViewData["Mensaje"] = "Sin libros";
            }
            else
            {
                List<Libros> libros = await this.repo.GetLibrosSessionAsync(idsLibros);
                await this.repo.InsertarCompraAsync(libros, idUsuario);
                HttpContext.Session.Remove("IDSLIBROS");
            }
            return RedirectToAction("Pedidos","Libros");
        }
        [AuthorizeUsuarios]
        public async Task<IActionResult> Perfil()
        {
            
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            Usuarios user = await this.repo.DetallesUsuarioAsync(idUsuario);
            return View(user);
        }
        [AuthorizeUsuarios]
        public async Task <IActionResult> Pedidos()
        {
            int idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            List<VistaPedidos> pedidos = await this.repo.GetPedidosAsync(idUsuario);
            return View(pedidos);
        }
        
    }
}
