using System.ComponentModel.DataAnnotations;
using GestionIntervention.Models;

namespace GestionIntervention.Models
{
    public class Intervention
    {
        [Key]
        public int NumInterv { get; set; }

        [Required(ErrorMessage = "La date d'intervention est obligatoire")]
        [DataType(DataType.Date)]
        public DateTime DateInterv { get; set; }

        [Required(ErrorMessage = "Le client est obligatoire")]
        public string CodeClt { get; set; } = string.Empty;

        public Client? Client { get; set; }

        [Required(ErrorMessage = "Le produit est obligatoire")]
        public string ReferencePd { get; set; } = string.Empty;

        public Produit? Produit { get; set; }

        [Required(ErrorMessage = "Le technicien est obligatoire")]
        public string CodeTech { get; set; } = string.Empty;

        public Technicien? Technicien { get; set; }

        [Required(ErrorMessage = "Le statut est obligatoire")]
        public string Statut { get; set; } = string.Empty;
    }
}
