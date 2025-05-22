using System.ComponentModel.DataAnnotations;

namespace GestionIntervention.Models
{
    public class Produit
    {
        [Key]
        [StringLength(50)]
        public string ReferencePd { get; set; } = string.Empty;

        [Required(ErrorMessage = "La désignation est obligatoire")]
        [StringLength(100)]
        public string Designation { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [Range(0, double.MaxValue, ErrorMessage = "Le prix doit être positif")]
        public decimal Prix { get; set; }

        public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    }
}