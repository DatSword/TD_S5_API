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
            List<Marque> lesMarques = context.Marques.ToList();

            // Act
            var res = controller.GetMarques().Result;

            // Assert
            Assert.IsNotNull(res);
            CollectionAssert.AreEqual(lesMarques, res.Value.ToList(), "Les listes de marques ne sont pas identiques");
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

            mockRepository.Setup(x => x.GetEntityDtoByIdAsync(1).Result).Returns(marque);

            // Act
            var actionResult = mockController.GetMarque(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(marque, actionResult.Value as MarqueDetailDto);
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
            Marque userEdited = new Marque
            {
                IdMarque = 667,
                NomMarque = "Intermarché",
            };

            mockRepository.Setup(x => x.GetEntityByIdAsync(667).Result).Returns(marqueToEdit);

            // Act
            var actionResult = mockController.PutMarque(667, userEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
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