using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturation.Core.Entities;

public class Client
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères")]
    public string Nom { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Adresse { get; set; }

    [StringLength(20)]
    public string? MatriculeFiscal { get; set; }

    [StringLength(20)]
    [Phone(ErrorMessage = "Numéro de téléphone invalide")]
    public string? Telephone { get; set; }

    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Adresse email invalide")]
    public string? Email { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.Now;

    // Navigation
    public virtual ICollection<Facture> Factures { get; set; } = new List<Facture>();
}
