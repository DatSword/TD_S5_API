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
        private IDataRepository<Produit> dataRepository;
        private IMapper mapper;
        private ProduitsController controller;

        private Mock<IDataRepository<Produit>> mockRepository;
        private Mock<IMapper> mockMapper;
        private ProduitsController mockController;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange

            //Pour les tests classiques
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new ProduitManager(context);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Produit, ProduitDto>();
            });
            mapper = config.CreateMapper();
            controller = new ProduitsController(dataRepository, mapper);

            //Pour les tests moqs
            mockRepository = new Mock<IDataRepository<Produit>>();
            mockMapper = new Mock<IMapper>();
            mockController = new ProduitsController(mockRepository.Object, mockMapper.Object);
        }

        [TestMethod()]
        public void GetProduitsTest()
        {
            //Arrange
            List<Produit> lesProduits = context.Produits.ToList();
            List<ProduitDto> expectedProduitDtos = lesProduits.Select(m => mapper.Map<ProduitDto>(m)).ToList();
            // Act
            var res = controller.GetProduits().Result;
            var okResult = res.Result as OkObjectResult;
            var actualProduitDtos = okResult.Value as IEnumerable<ProduitDto>;
            // Assert
            Assert.IsNotNull(actualProduitDtos, "La valeur de la réponse est null.");
            CollectionAssert.AreEqual(expectedProduitDtos, actualProduitDtos.ToList(), "Les listes de produits ne sont pas identiques");
        }

        [TestMethod]
        public void GetProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Produit produit = new Produit
            {
                IdProduit = 1,
                NomProduit = "shampoing",
                Description = "bla",
                NomPhoto = "cool",
                UriPhoto = "https://coool.com",
                IdTypeProduit = 1,
                IdMarque = 1,
                StockReel = 1,
                StockMin = 1,
                StockMax = 1,
            };

            ProduitDto expectedProduitDto = new ProduitDto
            {
                IdProduit = 1,
                NomProduit = "shampoing",
                IdTypeProduit = 1,
                IdMarque = 1,
            };

            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(produit);
            mockMapper.Setup(m => m.Map<ProduitDto>(produit)).Returns(expectedProduitDto);

            // Act
            var actionResult = mockController.GetProduit(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(expectedProduitDto, okResult.Value as ProduitDto);
        }

        [TestMethod]
        public void PostProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            ProduitDto produitDto = new ProduitDto
            {
                IdProduit = 666,
                NomProduit = "Aurora 12",
                IdTypeProduit = 1,
                IdMarque = 1,

            };

            Produit expectedProduit = new Produit
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

            mockMapper.Setup(m => m.Map<Produit>(produitDto)).Returns(expectedProduit);
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Produit>())).Returns(Task.FromResult(expectedProduit));
            mockMapper.Setup(m => m.Map<ProduitDto>(expectedProduit)).Returns(produitDto);

            // Act
            var actionResult = mockController.PostProduit(produitDto).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<ProduitDto>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result, "Le résultat CreatedAtActionResult est null.");
            Assert.IsInstanceOfType(result.Value, typeof(ProduitDto), "Pas un produit");

            var createdProduitDto = result.Value as ProduitDto;
            Assert.AreEqual(produitDto, createdProduitDto, "produits non identiques");
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
            ProduitDto produitEditedDto = new ProduitDto
            {
                IdProduit = 667,
                NomProduit = "Iphone16",
                IdTypeProduit = 1,
                IdMarque = 1,
            };
            Produit prodEdited = new Produit
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

            mockRepository.Setup(x => x.GetByIdAsync(667)).ReturnsAsync(produitToEdit);
            mockRepository.Setup(r => r.UpdateAsync(produitToEdit, It.IsAny<Produit>())).Returns(Task.FromResult(prodEdited));
            mockMapper.Setup(m => m.Map<Produit>(prodEdited)).Returns(prodEdited);
            mockMapper.Setup(m => m.Map<ProduitDto>(It.IsAny<Produit>())).Returns(produitEditedDto);

            // Act
            var actionResult = mockController.PutProduit(667, produitEditedDto).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<ProduitDto>), "Pas un ActionResult<>");

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(produitEditedDto, okResult.Value as ProduitDto);
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

            mockRepository.Setup(x => x.GetByIdAsync(334).Result).Returns(produit);
            // Act
            var actionResult = mockController.DeleteProduit(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }

    }
}