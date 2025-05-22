using System.Collections.Generic;
namespace GestionIntervention.Models
{
    public class DashboardViewModel
    {
        public int TotalClients { get; set; }
        public int TotalTechniciens { get; set; }
        public int TotalProduits { get; set; }
        public int TotalInterventions { get; set; }
        public int InterventionsThisMonth { get; set; }
        public List<Intervention> RecentInterventions { get; set; } = new List<Intervention>();
        public int[] InterventionsByMonth { get; set; } = new int[12];
    }

}
