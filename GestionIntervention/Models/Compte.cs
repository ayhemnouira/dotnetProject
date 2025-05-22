using System.ComponentModel.DataAnnotations;

namespace GestionIntervention.Models
{
    public class Compte
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le login est obligatoire")]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est obligatoire")]
        [DataType(DataType.Password)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = string.Empty;
    }
}
