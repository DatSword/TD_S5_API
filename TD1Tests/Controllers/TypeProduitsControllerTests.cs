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
    public class TypeProduitsControllerTests
    {
        private ProduitsDBContext context;
        private IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto> dataRepository;
        private IMapper mapper;
        private TypeProduitsController controller;

        private Mock<IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto>> mockRepository;
        private Mock<IMapper> mockMapper;
        private TypeProduitsController mockController;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange

            //Pour les tests classiques
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new TypeProduitManager(context);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TypeProduit, TypeProduitDto>();
            });
            mapper = config.CreateMapper();
            controller = new TypeProduitsController(dataRepository, mapper);

            //Pour les tests moqs
            mockRepository = new Mock<IDataRepository<TypeProduit, TypeProduitDto, TypeProduitDetailDto>>();
            mockMapper = new Mock<IMapper>();
            mockController = new TypeProduitsController(mockRepository.Object, mockMapper.Object);
        }

        [TestMethod()]
        public void GetTypeProduitsTest()
        {
            //Arrange
            var lesTypeProduitDataRepos = dataRepository.GetAllAsync().Result;
            var actualTypeProduitDataRepos = lesTypeProduitDataRepos.Value as IEnumerable<TypeProduitDto>;

            // Act
            var lesTypeProduitDtos = controller.GetTypeProduits().Result;
            var okResult = lesTypeProduitDtos.Result as OkObjectResult;
            var actualTypeProduitDtos = okResult.Value as IEnumerable<TypeProduitDto>;

            // Assert
            Assert.IsNotNull(actualTypeProduitDtos, "La valeur de la réponse est null.");
            CollectionAssert.AreEqual(actualTypeProduitDataRepos.ToList(), actualTypeProduitDtos.ToList(), "Les listes de types ne sont pas identiques");
        }

        [TestMethod]
        public void GetTypeProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            TypeProduit typeProd = new TypeProduit
            {
                IdTypeProduit = 1,
                NomTypeProduit = "Shampoing",
            };

            TypeProduitDetailDto expectedTypeProdDto = new TypeProduitDetailDto
            {
                IdTypeProduit = 1,
                NomTypeProduit = "Shampoing",
                NbProduits = 0,
            };

            mockRepository.Setup(x => x.GetEntityDtoByIdAsync(1)).ReturnsAsync(expectedTypeProdDto);

            // Act
            var actionResult = mockController.GetTypeProduit(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(expectedTypeProdDto, okResult.Value as TypeProduitDetailDto);
        }

        [TestMethod]
        public void PostTypeProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            TypeProduit type = new TypeProduit
            {
                IdTypeProduit = 666,
                NomTypeProduit = "Ordi",
            };

            // Act
            var actionResult = mockController.PostTypeProduit(type).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeProduit>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(TypeProduit), "Pas un type");
            type.IdTypeProduit = ((TypeProduit)result.Value).IdTypeProduit;
            Assert.AreEqual(type, (TypeProduit)result.Value, "Types pas identiques");
        }

        [TestMethod]
        public void PutTypeProduitTestAvecMoq()
        {
            //Arrange
            TypeProduit typeToEdit = new TypeProduit
            {
                IdTypeProduit = 667,
                NomTypeProduit = "Géant",
            };
            TypeProduit typeEdited = new TypeProduit
            {
                IdTypeProduit = 667,
                NomTypeProduit = "Intermarché",
            };

            mockRepository.Setup(x => x.GetEntityByIdAsync(667)).ReturnsAsync(new ActionResult<TypeProduit>(typeToEdit));

            mockRepository.Setup(x => x.UpdateAsync(typeToEdit, typeEdited)).Returns(Task.CompletedTask);

            // Act
            var actionResult = mockController.PutTypeProduit(667, typeEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkObjectResult), "Le résultat attendu est OkObjectResult, mais un autre type a été retourné.");

            var okResult = actionResult as OkObjectResult;
            Assert.IsNotNull(okResult, "La réponse OkObjectResult est null.");

            var updatedTypeProduit = okResult.Value as TypeProduit;
            Assert.IsNotNull(updatedTypeProduit, "Le typeProduit mis à jour est null.");

            Assert.AreEqual(typeEdited, updatedTypeProduit, "Le nom du typeProduit n'a pas été mis à jour correctement.");
        }

        [TestMethod]
        public void DeleteTypeProduitTest_AvecMoq()
        {
            // Arrange
            TypeProduit typeProduit = new TypeProduit
            {
                IdTypeProduit = 334,
                NomTypeProduit = "téléphone mobile",
            };
            mockRepository.Setup(x => x.GetEntityByIdAsync(334).Result).Returns(typeProduit);

            // Act
            var actionResult = mockController.DeleteTypeProduit(334).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }
    }
}