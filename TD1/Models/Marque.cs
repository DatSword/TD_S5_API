using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD1.Models
{
    [Table("marque")]
    public partial class Marque
    {
        [Key]
        [Column("idmarque")]
        public int IdMarque { get; set; }

        [Column("nommarque")]
        public string? NomMarque { get; set; }

        [InverseProperty(nameof(Produit.IdMarqueNavigation))]
        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}
