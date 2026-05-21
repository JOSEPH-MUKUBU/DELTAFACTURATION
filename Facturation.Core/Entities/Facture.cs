using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturation.Core.Entities;

public class Facture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public string NumeroFacture { get; set; } = string.Empty;

    [Required(ErrorMessage = "La date de facture est obligatoire")]
    public DateTime DateFacture { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Le client est obligatoire")]
    public int ClientId { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal TotalHT { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal TotalTVA { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal TotalTTC { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal TimbreFiscal { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal MontantTotal { get; set; }

    [Required]
    [StringLength(20)]
    public string Statut { get; set; } = "Validée";

    // Navigation
    [ForeignKey("ClientId")]
    public virtual Client? Client { get; set; }

    public virtual ICollection<LigneFacture> Lignes { get; set; } = new List<LigneFacture>();
}
