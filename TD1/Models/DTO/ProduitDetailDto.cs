namespace TD1.Models.DTO
{
    public class ProduitDetailDto
    {
        public int IdProduit { get; set; }
        public string? NomProduit { get; set; }
        public string? NomTypeProduit { get; set; }
        public string? NomMarque { get; set; }
        public string? Description { get; set; }
        public string? Nomphoto { get; set; }
        public string? Uriphoto { get; set; }
        public int? Stock { get; set; }
        public bool EnReappro { get; set; }
    }
}
