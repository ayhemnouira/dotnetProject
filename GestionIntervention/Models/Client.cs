using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionIntervention.Models
{
    public class Client
    {
        [Key]
        [StringLength(50)]
        public string CodeClt { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères")]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [StringLength(50, ErrorMessage = "Le prénom ne peut pas dépasser 50 caractères")]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'adresse est obligatoire")]
        [StringLength(100)]
        public string Adresse { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le code postal est obligatoire")]
        [StringLength(10, ErrorMessage = "Le code postal ne peut pas dépasser 10 caractères")]
        public string CP { get; set; } = string.Empty;

        public ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();
    }
}
