using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntervention.Controllers
{
    [Authorize]
    public class TechnicienController : Controller
    {
        private readonly AppDbContext _context;

        public TechnicienController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm)
        {
            var techniciens = _context.Techniciens.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                techniciens = techniciens.Where(t => t.Nom.Contains(searchTerm) || t.Prenom.Contains(searchTerm));
            }
            return View(techniciens.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Technicien technicien)
        {
            if (ModelState.IsValid)
            {
                if (_context.Techniciens.Any(t => t.CodeTech == technicien.CodeTech))
                {
                    ModelState.AddModelError("CodeTech", "Ce code technicien existe déjà");
                    return View(technicien);
                }
                _context.Techniciens.Add(technicien);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(technicien);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var technicien = _context.Techniciens.Find(id);
            if (technicien == null)
            {
                return NotFound();
            }
            return View(technicien);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Technicien technicien)
        {
            if (ModelState.IsValid)
            {
                _context.Update(technicien);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(technicien);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var technicien = _context.Techniciens.Find(id);
            if (technicien == null)
            {
                return NotFound();
            }
            return View(technicien);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(string id)
        {
            var technicien = _context.Techniciens.Find(id);
            if (technicien == null)
            {
                return NotFound();
            }
            if (_context.Interventions.Any(i => i.CodeTech == id))
            {
                ModelState.AddModelError("", "Impossible de supprimer ce technicien car il est associé à des interventions.");
                return View(technicien);
            }
            _context.Techniciens.Remove(technicien);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
