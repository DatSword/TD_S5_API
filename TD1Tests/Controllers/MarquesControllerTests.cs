﻿using Microsoft.AspNetCore.Mvc;
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
        private IDataRepository<Marque> dataRepository;
        private IMapper mapper;
        private MarquesController controller;

        private Mock<IDataRepository<Marque>> mockRepository;
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
            mockRepository = new Mock<IDataRepository<Marque>>();
            mockMapper = new Mock<IMapper>();
            mockController = new MarquesController(mockRepository.Object, mockMapper.Object);
        }

        [TestMethod()]
        public void GetMarquesTest()
        {
            //Arrange
            List<Marque> lesMarques = context.Marques.ToList();
            List<MarqueDto> expectedMarqueDtos = lesMarques.Select(m => mapper.Map<MarqueDto>(m)).ToList();
            // Act
            var res = controller.GetMarques().Result;
            var okResult = res.Result as OkObjectResult;
            var actualMarqueDtos = okResult.Value as IEnumerable<MarqueDto>;
            // Assert
            Assert.IsNotNull(actualMarqueDtos, "La valeur de la réponse est null.");
            CollectionAssert.AreEqual(expectedMarqueDtos, actualMarqueDtos.ToList(), "Les listes de marques ne sont pas identiques");
        }

        [TestMethod]
        public void GetMarqueById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Marque marque = new Marque
            {
                IdMarque = 1,
                NomMarque = "Loréal",
            };

            MarqueDto expectedMarqueDto = new MarqueDto
            {
                IdMarque = 1,
                NomMarque = "Loréal",
            };

            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(marque);
            mockMapper.Setup(m => m.Map<MarqueDto>(marque)).Returns(expectedMarqueDto);

            // Act
            var actionResult = mockController.GetMarque(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(expectedMarqueDto, okResult.Value as MarqueDto);
        }

        [TestMethod]
        public void PostMarque_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            MarqueDto marqueDto = new MarqueDto
            {
                IdMarque = 666,
                NomMarque = "VCOUT_Industries",
            };

            Marque expectedMarque = new Marque
            {
                IdMarque = 666,
                NomMarque = "VCOUT_Industries",
            };

            mockMapper.Setup(m => m.Map<Marque>(marqueDto)).Returns(expectedMarque);
            mockRepository.Setup(r => r.AddAsync(It.IsAny<Marque>())).Returns(Task.FromResult(expectedMarque));
            mockMapper.Setup(m => m.Map<MarqueDto>(expectedMarque)).Returns(marqueDto);

            // Act
            var actionResult = mockController.PostMarque(marqueDto).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<MarqueDto>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result, "Le résultat CreatedAtActionResult est null.");
            Assert.IsInstanceOfType(result.Value, typeof(MarqueDto), "Pas une marque");

            var createdMarqueDto = result.Value as MarqueDto;
            Assert.AreEqual(marqueDto, createdMarqueDto, "marques non identiques");
        }

        [TestMethod]
        public void PutMarqueTestAvecMoq()
        {
            Marque marqueToEdit = new Marque
            {
                IdMarque = 667,
                NomMarque = "Géant",

            };
            MarqueDto marqueEdited = new MarqueDto
            {
                IdMarque = 667,
                NomMarque = "Intermarché",
            };

            mockRepository.Setup(x => x.GetByIdAsync(667)).ReturnsAsync(marqueToEdit);
            mockRepository.Setup(r => r.UpdateAsync(marqueToEdit, It.IsAny<Marque>())).Returns(Task.FromResult(marqueEdited));

            // Act
            var actionResult = mockController.PutMarque(667, marqueEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<MarqueDto>), "Pas un ActionResult<>");

            var okResult = actionResult.Result as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            Assert.AreEqual(marqueEdited, okResult.Value as MarqueDto);
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

            mockRepository.Setup(x => x.GetByIdAsync(334).Result).Returns(marque);
            // Act
            var actionResult = mockController.DeleteMarque(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

    }
}