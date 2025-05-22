using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionIntervention.Controllers
{
    [Authorize]
    public class InterventionController : Controller
    {
        private readonly AppDbContext _context;

        public InterventionController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm, string statusFilter, DateTime? dateFilter)
        {
            var interventions = _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Technicien)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                interventions = interventions.Where(i =>
                    (i.Client != null && (i.Client.Nom.Contains(searchTerm) || i.Client.Prenom.Contains(searchTerm))) ||
                    (i.Produit != null && i.Produit.Designation.Contains(searchTerm)) ||
                    (i.Technicien != null && (i.Technicien.Nom.Contains(searchTerm) || i.Technicien.Prenom.Contains(searchTerm))));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                interventions = interventions.Where(i => i.Statut == statusFilter);
            }

            if (dateFilter.HasValue)
            {
                interventions = interventions.Where(i => i.DateInterv.Date == dateFilter.Value.Date);
            }

            return View(interventions.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Produits = _context.Produits.ToList();
            ViewBag.Techniciens = _context.Techniciens.ToList();
            ViewBag.Statuses = new List<string> { "Planifiée", "En cours", "Terminée" };
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Intervention intervention)
        {
            if (ModelState.IsValid)
            {
                _context.Interventions.Add(intervention);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Produits = _context.Produits.ToList();
            ViewBag.Techniciens = _context.Techniciens.ToList();
            ViewBag.Statuses = new List<string> { "Planifiée", "En cours", "Terminée" };
            return View(intervention);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var intervention = _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Technicien)
                .FirstOrDefault(i => i.NumInterv == id);
            if (intervention == null)
            {
                return NotFound();
            }
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Produits = _context.Produits.ToList();
            ViewBag.Techniciens = _context.Techniciens.ToList();
            ViewBag.Statuses = new List<string> { "Planifiée", "En cours", "Terminée" };
            return View(intervention);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Intervention intervention)
        {
            if (ModelState.IsValid)
            {
                _context.Update(intervention);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Clients = _context.Clients.ToList();
            ViewBag.Produits = _context.Produits.ToList();
            ViewBag.Techniciens = _context.Techniciens.ToList();
            ViewBag.Statuses = new List<string> { "Planifiée", "En cours", "Terminée" };
            return View(intervention);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var intervention = _context.Interventions
                .Include(i => i.Client)
                .Include(i => i.Produit)
                .Include(i => i.Technicien)
                .FirstOrDefault(i => i.NumInterv == id);
            if (intervention == null)
            {
                return NotFound();
            }
            return View(intervention);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var intervention = _context.Interventions.Find(id);
            if (intervention == null)
            {
                return NotFound();
            }
            _context.Interventions.Remove(intervention);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}