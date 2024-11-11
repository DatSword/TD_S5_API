using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bunit;
using Moq;
using TD1BlazorApp.Pages;
using TD1BlazorApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TD1BlazorApp.Services;

namespace BUnitTests
{
    [TestClass]
    public class FetchDataTests : BunitTestContext
    {
        private Mock<IService<Produit>> produitServiceMock;

        [TestInitialize]
        public void InitTest()
        {
            produitServiceMock = new Mock<IService<Produit>>();
        }

        //[TestMethod]
        //public void FetchDataDisplaysLoadingMessageInitially()
        //{
        //    // Arrange
        //    produitServiceMock.Setup(service => service.GetItemsAsync())
        //        .ReturnsAsync((IList<Produit>)null); // Simule l'état de chargement initial
        //    Services.AddSingleton(produitServiceMock.Object);

        //    // Act
        //    var cut = RenderComponent<FetchProduits>();

        //    // Assert
        //    cut.MarkupMatches(@"
        //        <h1>Visualisation des produits</h1>
        //        <p>This component demonstrates fetching data from the server.</p>
        //        <p><em>Loading...</em></p>
        //    ");
        //}

        [TestMethod]
        public void FetchDataDisplaysDataCorrectly()
        {
            // Arrange
            var produits = new List<Produit>
            {
                new Produit { NomProduit = "Produit1", Description = "Desc1", IdMarque = 1, IdTypeProduit = 1 },
                new Produit { NomProduit = "Produit2", Description = "Desc2", IdMarque = 2, IdTypeProduit = 2 }
            };

            produitServiceMock.Setup(service => service.GetItemsAsync())
                .ReturnsAsync(produits);
            Services.AddSingleton(produitServiceMock.Object);

            // Act
            var cut = RenderComponent<FetchProduits>();

            // Assert
            cut.Find("h1").MarkupMatches("<h1>Visualisation des produits</h1>");

            // Vérifier le tableau
            var rows = cut.FindAll("table tbody tr");
            Assert.AreEqual(2, rows.Count);

            // Vérifier les données de la première ligne
            rows[0].MarkupMatches(@"
                <tr>
                    <td>Produit1</td>
                    <td>Desc1</td>
                    <td>1</td>
                    <td>1</td>
                </tr>
            ");

            // Vérifier les données de la deuxième ligne
            rows[1].MarkupMatches(@"
                <tr>
                    <td>Produit2</td>
                    <td>Desc2</td>
                    <td>2</td>
                    <td>2</td>
                </tr>
            ");
        }
    }
}