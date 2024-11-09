
namespace TD1.Models.DTO
{
    public class TypeProduitDto
    {
        public int IdTypeProduit { get; set; }
        public string? NomTypeProduit { get; set; }

        //pour le test GetAll
        public override bool Equals(object? obj)
        {
            return obj is TypeProduitDto dto &&
                   this.IdTypeProduit == dto.IdTypeProduit &&
                   this.NomTypeProduit == dto.NomTypeProduit;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.IdTypeProduit, this.NomTypeProduit);
        }
    }
}
