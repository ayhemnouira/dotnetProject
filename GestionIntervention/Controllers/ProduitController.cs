using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionIntervention.Controllers
{
    [Authorize]
    public class ProduitController : Controller
    {
        private readonly AppDbContext _context;

        public ProduitController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchTerm)
        {
            var produits = _context.Produits.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                produits = produits.Where(p => p.Designation.Contains(searchTerm));
            }
            return View(produits.ToList());
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Produit produit)
        {
            if (ModelState.IsValid)
            {
                if (_context.Produits.Any(p => p.ReferencePd == produit.ReferencePd))
                {
                    ModelState.AddModelError("ReferencePd", "Ce code produit existe déjà");
                    return View(produit);
                }
                _context.Produits.Add(produit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produit);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            var produit = _context.Produits.Find(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Update(produit);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(produit);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var produit = _context.Produits.Find(id);
            if (produit == null)
            {
                return NotFound();
            }
            return View(produit);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(string id)
        {
            var produit = _context.Produits.Find(id);
            if (produit == null)
            {
                return NotFound();
            }
            if (_context.Interventions.Any(i => i.ReferencePd == id))
            {
                ModelState.AddModelError("", "Impossible de supprimer ce produit car il est associé à des interventions.");
                return View(produit);
            }
            _context.Produits.Remove(produit);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
