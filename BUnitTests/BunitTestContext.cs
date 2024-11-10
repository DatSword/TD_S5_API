using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bunit;

namespace BUnitTests;
public abstract class BunitTestContext : TestContextWrapper
{
    [TestInitialize]
    public void Setup() => TestContext = new Bunit.TestContext();

    [TestCleanup]
    public void TearDown() => TestContext?.Dispose();
}