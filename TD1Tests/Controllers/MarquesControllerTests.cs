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
using TD1.Controllers;
using Moq;

namespace TD1.Controllers.Tests
{
    [TestClass()]
    public class MarquesControllerTests
    {
        private MarquesController controller;
        private ProduitsDBContext context;
        private IDataRepository<Marque> dataRepository;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new MarqueManager(context);
            controller = new MarquesController(dataRepository);
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
            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(marque);
            var marqueController = new MarquesController(mockRepository.Object);
            // Act
            var actionResult = marqueController.GetMarque(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(marque, actionResult.Value as Marque);
        }

        [TestMethod]
        public void PostMarque_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Marque>>();
            var marqueController = new MarquesController(mockRepository.Object);
            Marque marque = new Marque
            {
                IdMarque = 666,
                NomMarque = "VCOUT_Industries",
            };
            // Act
            var actionResult = marqueController.PostMarque(marque).Result;
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
            var mockRepository = new Mock<IDataRepository<Marque>>();
            mockRepository.Setup(x => x.GetByIdAsync(667).Result).Returns(marqueToEdit);
            var userController = new MarquesController(mockRepository.Object);

            // Act
            var actionResult = userController.PutMarque(667, userEdited).Result;

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
            var marquesController = new MarquesController(mockRepository.Object);
            // Act
            var actionResult = marquesController.DeleteMarque(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

        }
    }