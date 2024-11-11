
namespace TD1.Models.DTO
{
    public class ProduitDto
    {
        public int IdProduit { get; set; }
        public string? NomProduit { get; set; }
        public string? NomTypeProduit { get; set; }
        public string? NomMarque { get; set; }

        //pour le test GetAll
        public override bool Equals(object? obj)
        {
            return obj is ProduitDto dto &&
                   this.IdProduit == dto.IdProduit &&
                   this.NomProduit == dto.NomProduit &&
                   this.NomTypeProduit == dto.NomTypeProduit &&
                   this.NomMarque == dto.NomMarque;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.IdProduit, this.NomProduit, this.NomTypeProduit, this.NomMarque);
        }
    }
}
