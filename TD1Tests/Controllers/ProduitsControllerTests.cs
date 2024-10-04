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
    public class ProduitControllerTests
    {
        private ProduitsController controller;
        private ProduitsDBContext context;
        private IDataRepository<Produit> dataRepository;

        [TestInitialize]
        public void InitTest()
        {
            // Arrange
            var builder = new DbContextOptionsBuilder<ProduitsDBContext>().UseNpgsql("Server=localhost;port=5432;Database=ProduitsDB; uid=postgres;password=postgres;");
            context = new ProduitsDBContext(builder.Options);
            dataRepository = new ProduitManager(context);
            controller = new ProduitsController(dataRepository);
        }

        [TestMethod()]
        public void GetTypeProduitsTest()
        {
            //Arrange
            List<Produit> lesProds = context.Produits.ToList();
            // Act
            var res = controller.GetProduits().Result;
            // Assert
            Assert.IsNotNull(res);
            CollectionAssert.AreEqual(lesProds, res.Value.ToList(), "Les listes de prods ne sont pas identiques");
        }

        [TestMethod]
        public void GetProduitById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Produit prod = new Produit
            {
                IdProduit = 1,
                NomProduit = "shampoing",
                Description ="bla",
                NomPhoto="cool",
                UriPhoto="https://coool.com",
                IdTypeProduit=1,
                IdMarque=1,
                StockReel=1,
                StockMin = 1,
                StockMax = 1,
            };
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(1).Result).Returns(prod);
            var prodController = new ProduitsController(mockRepository.Object);
            // Act
            var actionResult = prodController.GetProduit(1).Result;
            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(prod, actionResult.Value as Produit);
        }

        [TestMethod]
        public void PostProduit_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Produit>>();
            var prodController = new ProduitsController(mockRepository.Object);
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
            var actionResult = prodController.PostProduit(prod).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ActionResult<Produit>), "Pas un ActionResult<>");
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult), "Pas un CreatedAtActionResult");
            var result = actionResult.Result as CreatedAtActionResult;
            Assert.IsInstanceOfType(result.Value, typeof(Produit), "Pas un prod");
            prod.IdProduit = ((Produit)result.Value).IdProduit;
            Assert.AreEqual(prod, (Produit)result.Value, "Prods pas identiques");
        }

       /* [TestMethod]
        public void PutProduitTestAvecMoq()
        {
            Produit prodToEdit = new Produit
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
            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(667).Result).Returns(prodToEdit);
            var prodController = new ProduitsController(mockRepository.Object);

            // Act
            var actionResult = prodController.PutProduit(prodToEdit.IdProduit, prodEdited).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult");
        }*/

        [TestMethod]
        public void DeleteMarqueTest_AvecMoq()
        {
            // Arrange

            Produit prod = new Produit
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

            var mockRepository = new Mock<IDataRepository<Produit>>();
            mockRepository.Setup(x => x.GetByIdAsync(334).Result).Returns(prod);
            var prodController = new ProduitsController(mockRepository.Object);
            // Act
            var actionResult = prodController.DeleteProduit(334).Result;
            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult), "Pas un NoContentResult"); // Test du type de retour
        }

    }
}