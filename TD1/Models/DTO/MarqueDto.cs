namespace TD1.Models.DTO
{
    public class MarqueDto
    {
        public int IdMarque { get; set; }
        public string? NomMarque { get; set; }
        public int NbProduits { get; set; }

        //pour le test GetAll
        public override bool Equals(object? obj)
        {
            return obj is MarqueDto dto &&
                   this.IdMarque == dto.IdMarque &&
                   this.NomMarque == dto.NomMarque &&
                   this.NbProduits == dto.NbProduits;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.IdMarque, this.NomMarque, this.NbProduits);
        }
    }
}
