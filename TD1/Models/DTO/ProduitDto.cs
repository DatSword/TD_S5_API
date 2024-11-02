
namespace TD1.Models.DTO
{
    public class ProduitDto
    {
        public int IdProduit { get; set; }
        public string? NomProduit { get; set; }
        public string? IdTypeProduit { get; set; }
        public string? IdMarque { get; set; }

        //pour le test GetAll
        public override bool Equals(object? obj)
        {
            return obj is ProduitDto dto &&
                   this.IdProduit == dto.IdProduit &&
                   this.NomProduit == dto.NomProduit &&
                   this.IdTypeProduit == dto.IdTypeProduit &&
                   this.IdMarque == dto.IdMarque;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.IdProduit, this.NomProduit, this.IdTypeProduit, this.IdMarque);
        }
    }
}
