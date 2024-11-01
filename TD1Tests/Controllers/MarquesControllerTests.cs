using Microsoft.VisualStudio.TestTools.UnitTesting;
using TD1.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private MarquesController controller;
        private ProduitsDBContext context;
        private IDataRepository<Marque> dataRepository;
        private readonly IMapper mapper;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new MarqueManager(context);
            controller = new MarquesController(dataRepository, mapper);
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
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(marque);
            mockMapper.Setup(m => m.Map<MarqueDto>(marque)).Returns(expectedMarqueDto);

            var marquesController = new MarquesController(mockRepository.Object, mockMapper.Object);
            // Act
            var actionResult = marquesController.GetMarque(1).Result;
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
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var mockMapper = new Mock<IMapper>();
            var marquesController = new MarquesController(mockRepository.Object, mockMapper.Object);
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
            var actionResult = marquesController.PostMarque(marqueDto).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<MarqueDto>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(result, "Le résultat CreatedAtActionResult est null.");
            Assert.IsInstanceOfType(result.Value, typeof(MarqueDto), "Pas une marque");

            var createdMarqueDto = result.Value as MarqueDto;
            Assert.AreEqual(marqueDto.IdMarque, createdMarqueDto.IdMarque, "ID de la marque incorrect");
            Assert.AreEqual(marqueDto.NomMarque, createdMarqueDto.NomMarque, "Nom de la marque incorrect");
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
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var mockMapper = new Mock<IMapper>();
            mockRepository.Setup(x => x.GetByIdAsync(667)).ReturnsAsync(marqueToEdit);
            
            var marquesController = new MarquesController(mockRepository.Object, mockMapper.Object);

            // Act
            var actionResult = marquesController.PutMarque(667, marqueEdited).Result;

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

            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(334).Result).Returns(marque);
            var mockMapper = new Mock<IMapper>();
            var marquesController = new MarquesController(mockRepository.Object, mockMapper.Object);
            // Act
            var actionResult = marquesController.DeleteMarque(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

    }
}