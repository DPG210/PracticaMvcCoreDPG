using Microsoft.AspNetCore.Mvc;
using PracticaMvcCoreDPG.Models;
using PracticaMvcCoreDPG.Repositories;

namespace PracticaMvcCoreDPG.ViewComponents
{
    public class MenuGenerosViewComponent: ViewComponent
    {
        private RepositoryLibros repo;
        public MenuGenerosViewComponent(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public async Task <IViewComponentResult> InvokeAsync()
        {
            List<Generos> generos = await this.repo.GetGenerosAsync();
            return View(generos);
        }
    }
}
