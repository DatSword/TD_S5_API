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
    public class MarquesControllerTests
    {

        private ProduitsDBContext context;
        private IDataRepository<Marque, MarqueDto, MarqueDetailDto> dataRepository;
        private IMapper mapper;
        private MarquesController controller;

        private Mock<IDataRepository<Marque, MarqueDto, MarqueDetailDto>> mockRepository;
        private Mock<IMapper> mockMapper;
        private MarquesController mockController;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange

            //Pour les tests classiques
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new MarqueManager(context);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Marque, MarqueDto>();
            });
            mapper = config.CreateMapper();
            controller = new MarquesController(dataRepository, mapper);

            //Pour les tests moqs
            mockRepository = new Mock<IDataRepository<Marque, MarqueDto, MarqueDetailDto>>();
            mockMapper = new Mock<IMapper>();
            mockController = new MarquesController(mockRepository.Object, mockMapper.Object);
        }

        [TestMethod()]
        public void GetMarquesTest()
        {
            //Arrange
            var lesMarqueDataRepos = dataRepository.GetAllAsync().Result;
            var actualMarqueDataRepos = lesMarqueDataRepos.Value as IEnumerable<MarqueDto>;

            // Act
            var lesMarqueDtos = controller.GetMarques().Result;
            var okResult = lesMarqueDtos.Result as OkObjectResult;
            var actualMarqueDtos = okResult.Value as IEnumerable<MarqueDto>;

            // Assert
            Assert.IsNotNull(actualMarqueDtos, "La valeur de la réponse est null.");
            CollectionAssert.AreEqual(actualMarqueDataRepos.ToList(), actualMarqueDtos.ToList(), "Les listes de marques ne sont pas identiques");
        }

        [TestMethod]
        public void GetMarqueById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            MarqueDetailDto marque = new MarqueDetailDto
            {
                IdMarque = 1,
                NomMarque = "Loréal",
                NbProduits = 1,
            };

            mockRepository.Setup(x => x.GetEntityDtoByIdAsync(1)).ReturnsAsync(marque);

            // Act
            var actionResult = mockController.GetMarque(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(marque, okResult.Value as MarqueDetailDto);
        }

        [TestMethod]
        public void PostMarque_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            Marque marque = new Marque
            {
                IdMarque = 666,
                NomMarque = "VCOUT_Industries",
            };

            // Act
            var actionResult = mockController.PostMarque(marque).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Marque>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Marque), "Pas une marque");
            marque.IdMarque = ((Marque)result.Value).IdMarque;
            Assert.AreEqual(marque, (Marque)result.Value, "Marques pas identiques");
        }

        [TestMethod]
        public void PutMarqueTestAvecMoq()
        {
            // Arrange
            Marque marqueToEdit = new Marque
            {
                IdMarque = 667,
                NomMarque = "Géant",
            };
            Marque marqueEdited = new Marque
            {
                IdMarque = 667,
                NomMarque = "Intermarché",
            };

            mockRepository.Setup(x => x.GetEntityByIdAsync(667)).ReturnsAsync(new ActionResult<Marque>(marqueToEdit));

            mockRepository.Setup(x => x.UpdateAsync(marqueToEdit, marqueEdited)).Returns(Task.CompletedTask);

            // Act
            var actionResult = mockController.PutMarque(667, marqueEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Le résultat attendu est OkObjectResult, mais un autre type a été retourné.");

            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult, "La réponse OkObjectResult est null.");

            var updatedMarque = okResult.Value as Marque;
            Assert.IsNotNull(updatedMarque, "La marque mise à jour est null.");

            Assert.AreEqual(marqueEdited, updatedMarque, "Le nom de la marque n'a pas été mis à jour correctement.");
        }

        [TestMethod]
        public void DeleteMarqueTest_AvecMoq()
        {
            // Arrange
            Marque marque = new Marque
            {
                IdMarque = 334,
                NomMarque = "QBINDUSTRIES",
            };
            mockRepository.Setup(x => x.GetEntityByIdAsync(334).Result).Returns(marque);
            // Act
            var actionResult = mockController.DeleteMarque(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }
    }
}