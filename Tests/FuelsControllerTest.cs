using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Moq;
using Moq.EntityFrameworkCore;
namespace Tests
{
    public class FuelsControllerTest
    {
        [Fact]
        public async Task GetFuelList()
        {
            // Arrange
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x=>x.Fuels).ReturnsDbSet(TestDataHelper.GetFakeFuelsList());
         
            //Act
            FuelsController fuelsController = new(fuelsContextMock.Object);
            var result = fuelsController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<FuelsViewModel> (
                viewResult.ViewData.Model);
            Assert.Equal(2, model.Fuels.Count());


        }
    }
}