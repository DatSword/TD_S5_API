﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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


        [ForeignKey(nameof(IdMarque))]
        [InverseProperty(nameof(Marque.Produits))]
        public virtual Marque? IdMarqueNavigation { get; set; }

        [ForeignKey(nameof(IdTypeProduit))]
        [InverseProperty(nameof(TypeProduit.Produits))]
        public virtual TypeProduit? IdtypeProduitNavigation { get; set; }

    }
}
