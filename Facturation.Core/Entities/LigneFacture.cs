using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturation.Core.Entities;

public class LigneFacture
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int FactureId { get; set; }

    [Required(ErrorMessage = "Le produit est obligatoire")]
    public int ProduitId { get; set; }

    [Required(ErrorMessage = "La quantité est obligatoire")]
    [Column(TypeName = "decimal(10,3)")]
    [Range(0.001, double.MaxValue, ErrorMessage = "La quantité doit être supérieure à 0")]
    public decimal Quantite { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal PrixUnitaireHT { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal TauxTVA { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal MontantHT { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal MontantTVA { get; set; }

    [Column(TypeName = "decimal(12,3)")]
    public decimal MontantTTC { get; set; }

    // Navigation
    [ForeignKey("FactureId")]
    public virtual Facture? Facture { get; set; }

    [ForeignKey("ProduitId")]
    public virtual Produit? Produit { get; set; }
}
