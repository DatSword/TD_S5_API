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
    public class TypeProduitsControllerTests
    {
        private TypeProduitsController controller;
        private ProduitsDBContext context;
        private IDataRepository<TypeProduit> dataRepository;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new TypeProduitManager(context);
            controller = new TypeProduitsController(dataRepository);
        }

        [TestMethod()]
        public void GetTypeProduitsTest()
        {
            //Arrange
            List<TypeProduit> lesTypes = context.TypeProduits.ToList();
            // Act
            var res = controller.GetTypeProduits().Result;
            // Assert
            Assert.IsNotNull(res);
            CollectionAssert.AreEqual(lesTypes, res.Value.ToList(), "Les listes de types ne sont pas identiques");
        }

        [TestMethod]
        public void GetTypeProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            TypeProduit type = new TypeProduit
            {
                IdTypeProduit = 1,
                NomTypeProduit = "shampoing",
            };
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(type);
            var typeController = new TypeProduitsController(mockRepository.Object);
            // Act
            var actionResult = typeController.GetTypeProduit(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(type, actionResult.Value as TypeProduit);
        }

        [TestMethod]
        public void PostTypeProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            var typeController = new TypeProduitsController(mockRepository.Object);
            TypeProduit type = new TypeProduit
            {
                IdTypeProduit = 666,
                NomTypeProduit = "Ordi",
            };
            // Act
            var actionResult = typeController.PostTypeProduit(type).Result;
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
            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(667).Result).Returns(typeToEdit);
            var typeController = new TypeProduitsController(mockRepository.Object);

            // Act
            var actionResult = typeController.PutTypeProduit(667, typeEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }

        [TestMethod]
        public void DeleteMarqueTest_AvecMoq()
        {
            // Arrange

            TypeProduit marque = new TypeProduit
            {
                IdTypeProduit = 334,
                NomTypeProduit = "téléphone mobile",
            };

            var mockRepository = new Mock<IDataRepository<TypeProduit>>();
            mockRepository.Setup(x => x.GetByIdAsync(334).Result).Returns(marque);
            var typeController = new TypeProduitsController(mockRepository.Object);
            // Act
            var actionResult = typeController.DeleteTypeProduit(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

    }
}