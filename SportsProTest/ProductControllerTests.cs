using System;
using Xunit;
using SportsPro.Areas.Admin.Controllers;
using SportsProTest.FakeClassesNotUsed;
using SportsPro.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace SportsProTest
{
    public class ProductControllerTests
    {
        [Fact]
        public void IndexActionMethod_ReturnsAViewResult()
        {
            //arange
            var rep = new FakeProductRepository();
            var controller = new ProductController(rep);

            //act
            var result = controller.Index();

            //assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
