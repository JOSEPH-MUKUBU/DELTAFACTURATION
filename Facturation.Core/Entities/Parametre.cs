using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facturation.Core.Entities;

public class Parametre
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "La clé est obligatoire")]
    [StringLength(50)]
    public string Cle { get; set; } = string.Empty;

    [StringLength(255)]
    public string? Valeur { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }
}
