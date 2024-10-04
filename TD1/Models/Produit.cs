using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TD1.Models
{
    [Table("produit")]
    public partial class Produit
    {
        [Key]
        [Column("idproduit")]
        public int IdProduit { get; set; }

        [Column("nomproduit")]
        public string NomProduit { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("idnomphoto")]
        public string NomPhoto { get; set; }

        [Column("iduriproduit")]
        public string UriPhoto { get; set; }

        [Column("idtypeproduit")]
        public int IdTypeProduit { get; set; }

        [Column("idmarque")]
        public int IdMarque { get; set; }

        [Column("stockreel")]
        public int StockReel { get; set; }

        [Column("stockmin")]
        public int StockMin { get; set; }

        [Column("stockmax")]
        public int StockMax { get; set; }


        [ForeignKey("idmarque")]
        [InverseProperty(nameof(Marque.Produits))]

        public virtual Marque? IdmarqueNavigation { get; set; }

        [ForeignKey("idtypeproduit")]
        [InverseProperty(nameof(TypeProduit.Produits))]

        public virtual TypeProduit? IdtypeProduitNavigation { get; set; }

    }
}
