using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bunit;
using TD1BlazorApp.Pages;

namespace BUnitTests.Pages;

[TestClass]
public class HomeTests : BunitTestContext
{
    [TestMethod]
    public void HelloWorldComponentRendersCorrectly()
    {
        // Act
        var cut = RenderComponent<Home>();

        // Assert
        cut.MarkupMatches(@"
                <h1>Hello, world!</h1>
                Welcome to your new app.
            ");
    }
}