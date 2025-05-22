using GestionIntervention.Data;
using GestionIntervention.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestionIntervention.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            var interventionsByMonth = _context.Interventions
                .GroupBy(i => new { i.DateInterv.Year, i.DateInterv.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Count = g.Count()
                })
                .Where(g => g.Year == DateTime.Now.Year)
                .OrderBy(g => g.Month)
                .ToList();

            var model = new DashboardViewModel
            {
                TotalClients = _context.Clients.Count(),
                TotalTechniciens = _context.Techniciens.Count(),
                TotalProduits = _context.Produits.Count(),
                TotalInterventions = _context.Interventions.Count(),
                InterventionsThisMonth = _context.Interventions
                    .Count(i => i.DateInterv.Month == DateTime.Now.Month && i.DateInterv.Year == DateTime.Now.Year),
                RecentInterventions = _context.Interventions
                    .Include(i => i.Client)
                    .Include(i => i.Technicien)
                    .OrderByDescending(i => i.DateInterv)
                    .Take(5)
                    .ToList()
            };

            // Populate InterventionsByMonth array (0-based index for months)
            foreach (var monthData in interventionsByMonth)
            {
                model.InterventionsByMonth[monthData.Month - 1] = monthData.Count;
            }

            return View(model);
        }
    }
}