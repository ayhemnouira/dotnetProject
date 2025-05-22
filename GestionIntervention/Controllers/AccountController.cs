using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GestionIntervention.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var compte = _context.Comptes.FirstOrDefault(c => c.Login == model.Login);
            if (compte == null || !BCrypt.Net.BCrypt.Verify(model.Password, compte.PasswordHash))
            {
                ModelState.AddModelError("", "Login ou mot de passe incorrect");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, compte.Login),
                new Claim(ClaimTypes.Role, compte.Role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Compte compte)
        {
            if (ModelState.IsValid)
            {
                if (_context.Comptes.Any(c => c.Login == compte.Login))
                {
                    ModelState.AddModelError("Login", "Ce login est déjà utilisé");
                    return View(compte);
                }

                compte.PasswordHash = BCrypt.Net.BCrypt.HashPassword(compte.PasswordHash);
                compte.Role = "User";
                _context.Comptes.Add(compte);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(compte);
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}