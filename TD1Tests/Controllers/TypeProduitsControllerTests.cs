//using Microsoft.AspNetCore.Mvc;
//using TD1.Models;
//using Microsoft.EntityFrameworkCore;
//using TD1.Models.Repository;
//using TD1.Models.DataManager;
//using Moq;
//using AutoMapper;
//using TD1.Models.DTO;

//namespace TD1.Controllers.Tests
//{
//    [TestClass()]
//    public class TypeProduitsControllerTests
//    {
//        private ProduitsDBContext context;
//        private IDataRepository<TypeProduit> dataRepository;
//        private IMapper mapper;
//        private TypeProduitsController controller;

//        private Mock<IDataRepository<TypeProduit>> mockRepository;
//        private Mock<IMapper> mockMapper;
//        private TypeProduitsController mockController;

//        [TestInitialize]
//        public void InitTest()
//        {
//            // Arrange

//            //Pour les tests classiques
//            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
//            context = new ProduitsDBContext(builder.Options);
//            dataRepository = new TypeProduitManager(context);
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.CreateMap<TypeProduit, TypeProduitDto>();
//            });
//            mapper = config.CreateMapper();
//            controller = new TypeProduitsController(dataRepository, mapper);

//            //Pour les tests moqs
//            mockRepository = new Mock<IDataRepository<TypeProduit>>();
//            mockMapper = new Mock<IMapper>();
//            mockController = new TypeProduitsController(mockRepository.Object, mockMapper.Object);
//        }

//        [TestMethod()]
//        public void GetTypeProduitsTest()
//        {
//            //Arrange
//            List<TypeProduit> lesTypeProds = context.TypeProduits.ToList();
//            List<TypeProduitDto> expectedTypeProduitDtos = lesTypeProds.Select(m => mapper.Map<TypeProduitDto>(m)).ToList();
//            // Act
//            var res = controller.GetTypeProduits().Result;
//            var okResult = res.Result as OkObjectResult;
//            var actualTypeProduitDtos = okResult.Value as IEnumerable<TypeProduitDto>;
//            // Assert
//            Assert.IsNotNull(actualTypeProduitDtos, "La valeur de la réponse est null.");
//            CollectionAssert.AreEqual(expectedTypeProduitDtos, actualTypeProduitDtos.ToList(), "Les listes de types ne sont pas identiques");
//        }

//        [TestMethod]
//        public void GetTypeProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
//        {
//            // Arrange
//            TypeProduit typeProd = new TypeProduit
//            {
//                IdTypeProduit = 1,
//                NomTypeProduit = "shampoing",
//            };

//            TypeProduitDto expectedTypeProdDto = new TypeProduitDto
//            {
//                IdTypeProduit = 1,
//                NomTypeProduit = "shampoing",
//            };

//            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(typeProd);
//            mockMapper.Setup(m => m.Map<TypeProduitDto>(typeProd)).Returns(expectedTypeProdDto);

//            // Act
//            var actionResult = mockController.GetTypeProduit(1).Result;
//            // Assert
//            Assert.IsNotNull(actionResult);

//            var okResult = actionResult.Result as OkObjectResult;
//            Assert.IsNotNull(okResult.Value);
//            Assert.AreEqual(expectedTypeProdDto, okResult.Value as TypeProduitDto);
//        }

//        [TestMethod]
//        public void PostTypeProduit_ModelValidated_CreationOK_AvecMoq()
//        {
//            // Arrange
//            TypeProduitDto typeProduitDto = new TypeProduitDto
//            {
//                IdTypeProduit = 666,
//                NomTypeProduit = "Ordi",
//            };

//            TypeProduit expectedTypeProduit = new TypeProduit
//            {
//                IdTypeProduit = 666,
//                NomTypeProduit = "Ordi",
//            };

//            mockMapper.Setup(m => m.Map<TypeProduit>(typeProduitDto)).Returns(expectedTypeProduit);
//            mockRepository.Setup(r => r.AddAsync(It.IsAny<TypeProduit>())).Returns(Task.FromResult(expectedTypeProduit));
//            mockMapper.Setup(m => m.Map<TypeProduitDto>(expectedTypeProduit)).Returns(typeProduitDto);

//            // Act
//            var actionResult = mockController.PostMarque(typeProduitDto).Result;
//            // Assert
//            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeProduitDto>), "Pas un ActionResult<>");
//            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");

//            var result = actionResult.Result as CreatedAtActionResult;
//            Assert.IsNotNull(result, "Le résultat CreatedAtActionResult est null.");
//            Assert.IsInstanceOfType(result.Value, typeof(TypeProduitDto), "Pas un type");

//            var createdTypeProduitDto = result.Value as TypeProduitDto;
//            Assert.AreEqual(typeProduitDto, createdTypeProduitDto, "types non identiques");
//        }

//        [TestMethod]
//        public void PutTypeProduitTestAvecMoq()
//        {
//            TypeProduit typeProdToEdit = new TypeProduit
//            {
//                IdTypeProduit = 667,
//                NomTypeProduit = "Géant",

//            };
//            TypeProduitDto typeProdEditedDto = new TypeProduitDto
//            {
//                IdTypeProduit = 667,
//                NomTypeProduit = "Intermarché",
//            };
//            TypeProduit typeProdEdited = new TypeProduit
//            {
//                IdTypeProduit = 667,
//                NomTypeProduit = "Intermarché",
//            };

//            mockRepository.Setup(x => x.GetByIdAsync(667)).ReturnsAsync(typeProdToEdit);
//            mockRepository.Setup(r => r.UpdateAsync(typeProdToEdit, It.IsAny<TypeProduit>())).Returns(Task.FromResult(typeProdToEdit));
//            mockMapper.Setup(m => m.Map<TypeProduit>(typeProdEdited)).Returns(typeProdEdited);
//            mockMapper.Setup(m => m.Map<TypeProduitDto>(It.IsAny<TypeProduit>())).Returns(typeProdEditedDto);

//            // Act
//            var actionResult = mockController.PutTypeProduit(667, typeProdEditedDto).Result;

//            // Assert
//            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<TypeProduitDto>), "Pas un ActionResult<>");

//            var okResult = actionResult.Result as OkObjectResult;
//            Assert.IsNotNull(okResult.Value);
//            Assert.AreEqual(typeProdEditedDto, okResult.Value as TypeProduitDto);
//        }

//        [TestMethod]
//        public void DeleteTypeProduitTest_AvecMoq()
//        {
//            // Arrange

//            TypeProduit typeProduit = new TypeProduit
//            {
//                IdTypeProduit = 334,
//                NomTypeProduit = "téléphone mobile",
//            };

//            mockRepository.Setup(x => x.GetByIdAsync(334).Result).Returns(typeProduit);
//            // Act
//            var actionResult = mockController.DeleteTypeProduit(334).Result;
//            // Assert
//            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
//        }
//    }
//}