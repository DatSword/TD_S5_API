using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TD1.Models;
using TD1.Models.DTO;
using TD1.Models.Repository;

namespace TD1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        //private readonly ProduitsDBContext _context;

        private readonly IDataRepository<Produit, ProduitDto, ProduitDetailDto> dataRepository;

        public ProduitsController(IDataRepository<Produit, ProduitDto, ProduitDetailDto> dataRepo, IMapper mapper)
        {
            dataRepository = dataRepo;
        }

        // GET: api/Produits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProduitDto>>> GetProduits()
        {
            var produits = await dataRepository.GetAllAsync();
            return Ok(produits.Value);
        }

        // GET: api/Produits/{id}
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProduitDetailDto>> GetProduit(int id)
        {
            var produit = await dataRepository.GetEntityDtoByIdAsync(id);

            if (produit.Value == null)
            {
                return NotFound();
            }

            return Ok(produit.Value);
        }

        // POST: api/Produits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(produit);

            return CreatedAtAction("GetProduit", new { id = produit.IdProduit }, produit);
        }

        // PUT: api/Produits/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.IdProduit)
            {
                return BadRequest();
            }

            var produitToUpdate = await dataRepository.GetEntityByIdAsync(id);
            if (produitToUpdate.Value == null)
            {
                return NotFound();
            }

            await dataRepository.UpdateAsync(produitToUpdate.Value, produit);

            return NoContent();
        }

        // DELETE: api/Produits/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await dataRepository.GetEntityByIdAsync(id);
            if (produit.Value == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(produit.Value);
            return NoContent();
        }
    }
}
