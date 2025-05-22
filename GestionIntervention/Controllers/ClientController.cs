using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntervention.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm)
        {
            var clients = _context.Clients.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                clients = clients.Where(c => c.Nom.Contains(searchTerm) || c.Prenom.Contains(searchTerm) || c.Adresse.Contains(searchTerm));
            }
            return View(clients.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Client client)
        {
            if (ModelState.IsValid)
            {
                if (_context.Clients.Any(c => c.CodeClt == client.CodeClt))
                {
                    ModelState.AddModelError("CodeClt", "Ce code client existe déjà");
                    return View(client);
                }
                _context.Clients.Add(client);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Update(client);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(string id)
        {
            var client = _context.Clients.Find(id);
            if (client == null)
            {
                return NotFound();
            }
            if (_context.Interventions.Any(i => i.CodeClt == id))
            {
                ModelState.AddModelError("", "Impossible de supprimer ce client car il est associé à des interventions.");
                return View(client);
            }
            _context.Clients.Remove(client);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}