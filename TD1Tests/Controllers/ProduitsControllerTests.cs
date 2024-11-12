using Microsoft.AspNetCore.Mvc;
using TD1.Models;
using Microsoft.EntityFrameworkCore;
using TD1.Models.Repository;
using TD1.Models.DataManager;
using Moq;
using AutoMapper;
using TD1.Models.DTO;

namespace TD1.Controllers.Tests
{
    [TestClass()]
    public class ProduitControllerTests
    {

        private ProduitsDBContext context;
        private IDataRepository<Produit, ProduitDto, ProduitDetailDto> dataRepository;
        private IMapper mapper;
        private ProduitsController controller;

        private Mock<IDataRepository<Produit, ProduitDto, ProduitDetailDto>> mockRepository;
        private Mock<IMapper> mockMapper;
        private ProduitsController mockController;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange

            //Pour les tests classiques
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Produit, ProduitDto>();
            });
            mapper = config.CreateMapper();
            dataRepository = new ProduitManager(context, mapper);
            controller = new ProduitsController(dataRepository, mapper);

            //Pour les tests moqs
            mockRepository = new Mock<IDataRepository<Produit, ProduitDto, ProduitDetailDto>>();
            mockMapper = new Mock<IMapper>();
            mockController = new ProduitsController(mockRepository.Object, mockMapper.Object);
        }

        [TestMethod()]
        public void GetProduitsTest()
        {
            //Arrange
            var lesProduitDataRepos = dataRepository.GetAllAsync().Result;
            var actualProduitDataRepos = lesProduitDataRepos.Value as IEnumerable<ProduitDto>;

            // Act
            var lesProduitDtos = controller.GetProduits().Result;
            var okResult = lesProduitDtos.Result as OkObjectResult;
            var actualProduitDtos = okResult.Value as IEnumerable<ProduitDto>;

            // Assert
            Assert.IsNotNull(actualProduitDtos, "La valeur de la réponse est null.");
            CollectionAssert.AreEqual(actualProduitDataRepos.ToList(), actualProduitDtos.ToList(), "Les listes de produits ne sont pas identiques");
        }

        [TestMethod]
        public void GetProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Produit produit = new Produit
            {
                IdProduit = 1,
                NomProduit = "shampoing 3 en 1",
                Description = "bla",
                NomPhoto = "cool",
                UriPhoto = "https://coool.com",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 1,
                StockMin = 1,
                StockMax = 1,
            };

            ProduitDetailDto expectedProduitDto = new ProduitDetailDto
            {
                IdProduit = 1,
                NomProduit = "shampoing 3 en 1",
                NomTypeProduit = null,
                NomMarque = null,
            };

            mockRepository.Setup(x => x.GetEntityDtoByIdAsync(1)).ReturnsAsync(expectedProduitDto);

            // Act
            var actionResult = mockController.GetProduit(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(expectedProduitDto, okResult.Value as ProduitDetailDto);
        }

        [TestMethod]
        public void PostProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            Produit prod = new Produit
            {
                IdProduit = 666,
                NomProduit = "Aurora 12",
                Description = "bla",
                NomPhoto = "cool",
                UriPhoto = "https://coool.com",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 1,
                StockMin = 1,
                StockMax = 1,
            };
            // Act
            var actionResult = mockController.PostProduit(prod).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Produit>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Produit), "Pas un prod");
            prod.IdProduit = ((Produit)result.Value).IdProduit;
            Assert.AreEqual(prod, (Produit)result.Value, "Prods pas identiques");
        }

        [TestMethod]
        public void PutMarqueTestAvecMoq()
        {
            Produit produitToEdit = new Produit
            {
                IdProduit = 667,
                NomProduit = "Iphone15",
                Description = "bla",
                NomPhoto = "cool",
                UriPhoto = "https://coool.com",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 1,
                StockMin = 1,
                StockMax = 1,

            };

            Produit produitEdited = new Produit
            {
                IdProduit = 667,
                NomProduit = "Iphone16",
                Description = "bla",
                NomPhoto = "cool",
                UriPhoto = "https://coool.com",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 1,
                StockMin = 1,
                StockMax = 1,
            };

            mockRepository.Setup(x => x.GetEntityByIdAsync(667)).ReturnsAsync(new ActionResult<Produit>(produitToEdit));

            mockRepository.Setup(x => x.UpdateAsync(produitToEdit, produitEdited)).Returns(Task.CompletedTask);

            // Act
            var actionResult = mockController.PutProduit(667, produitEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Le résultat attendu est OkObjectResult, mais un autre type a été retourné.");

            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult, "La réponse OkObjectResult est null.");

            var updatedProduit = okResult.Value as Produit;
            Assert.IsNotNull(updatedProduit, "Le produit mis à jour est null.");

            Assert.AreEqual(produitEdited, updatedProduit, "Le nom du produit n'a pas été mis à jour correctement.");
        }

        [TestMethod]
        public void DeleteProduitTest_AvecMoq()
        {
            // Arrange

            Produit produit = new Produit
            {
                IdProduit = 334,
                NomProduit = "Galaxy 69",
                Description = "bla",
                NomPhoto = "cool",
                UriPhoto = "https://coool.com",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 1,
                StockMin = 1,
                StockMax = 1,
            };

            mockRepository.Setup(x => x.GetEntityByIdAsync(334).Result).Returns(produit);
            // Act
            var actionResult = mockController.DeleteProduit(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }

    }
}