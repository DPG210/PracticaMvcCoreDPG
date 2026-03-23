using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PracticaMvcCoreDPG.Models;
using PracticaMvcCoreDPG.Repositories;
using System.Security.Claims;

namespace PracticaMvcCoreDPG.Controllers
{
    public class ManagedController : Controller
    {
        private RepositoryLibros repo;
        public ManagedController(RepositoryLibros repo)
        {
            this.repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> Login (string username, string password)
        {
            
            Usuarios usuario = await this.repo.LogInUsuarioAsync(username, password);
            if(usuario != null)
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.Role);
                Claim claimName =
                    new Claim(ClaimTypes.Name, username);
                identity.AddClaim(claimName);
                Claim claimId =
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString());
                identity.AddClaim(claimId);
                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                string controller =
                        TempData["controller"].ToString();
                string action =
                        TempData["action"].ToString();
                
                
                
                if (TempData["id"] != null)
                {
                    string id =
                    TempData["id"].ToString();
                    return RedirectToAction
                    (action, controller, new { id = id });
                }
                else
                {
                    return RedirectToAction(action, controller);
                }
            }
            else
            {
                ViewData["MENSAJE"] = "Credenciales incorrectas";
                return View();
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Managed" );
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
        
        
    }
 }
