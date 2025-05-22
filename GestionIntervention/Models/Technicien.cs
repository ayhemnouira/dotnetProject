using System.ComponentModel.DataAnnotations;

namespace GestionIntervention.Models
{
    public class Technicien
    {
        [Key]
        [StringLength(50)]
        public string CodeTech { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    }
}
