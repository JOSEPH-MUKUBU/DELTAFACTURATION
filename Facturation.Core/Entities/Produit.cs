using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturation.Core.Entities;

public class Produit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Le code est obligatoire")]
    [StringLength(20)]
    public string Code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le libellé est obligatoire")]
    [StringLength(150)]
    public string Libelle { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le prix unitaire HT est obligatoire")]
    [Column(TypeName = "decimal(10,3)")]
    [Range(0.001, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
    public decimal PrixUnitaireHT { get; set; }

    [Required(ErrorMessage = "Le taux de TVA est obligatoire")]
    [Column(TypeName = "decimal(5,2)")]
    public decimal TauxTVA { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal StockActuel { get; set; } = 0;

    [Column(TypeName = "decimal(10,3)")]
    public decimal SeuilMinimal { get; set; } = 5;

    public bool EstActif { get; set; } = true;

    // Navigation
    public virtual ICollection<LigneFacture> LigneFactures { get; set; } = new List<LigneFacture>();
}
