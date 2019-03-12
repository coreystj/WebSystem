using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebSystem.Example.ASP.NET.SampleProject;
using WebSystem.Example.ASP.NET.SampleProject.Controllers;

namespace WebSystem.Example.ASP.NET.SampleProject.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home Page", result.ViewBag.Title);
        }
    }
}
