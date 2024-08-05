using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq.EntityFrameworkCore;
namespace Tests
{
    public class FuelsControllerTest
    {
        [Fact]
        public void GetFuelList()
        {
            // Arrange
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(TestDataHelper.GetFakeFuelsList());

            //Act
            FuelsController fuelsController = new(fuelsContextMock.Object);
            var result = fuelsController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<FuelsViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Fuels.Count());
        }

        [Fact]
        public async void GetFuel()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);
            var controller = new FuelsController(fuelsContextMock.Object);

            // Act
            var notFoundResult = await controller.Details(4);
            var foundResult = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }




        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);

            var controller = new FuelsController(fuelsContextMock.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Create(fuel: null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);

            var controller = new FuelsController(fuelsContextMock.Object);


            // Act
            Fuel fuel = new()
            {
                FuelID = 4,
                FuelType = "Heavy oil",
                FuelDensity = 3.7141F

            };
            var result = await controller.Create(fuel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            fuelsContextMock.Verify();

        }

        [Fact]
        public async Task Edit_ReturnsNotFound()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);
            var controller = new FuelsController(fuelsContextMock.Object);

            // Act
            var notFoundResult = await controller.Edit(4);
            var foundResult = await controller.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);

            var controller = new FuelsController(fuelsContextMock.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            Fuel fuel = new()
            {
                FuelID = 4,
                FuelType = "",
                FuelDensity = 3.7141F

            };
            var result = await controller.Edit(1, fuel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);
            var controller = new FuelsController(fuelsContextMock.Object);

            // Act
            Fuel fuel = new()
            {
                FuelID = 3,
                FuelType = "Heavy oil",
                FuelDensity = 3.7141F

            };
            var result = await controller.Edit(3, fuel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            fuelsContextMock.Verify();

        }



        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);
            var controller = new FuelsController(fuelsContextMock.Object);

            // Act
            var notFoundResult = await controller.Delete(4);
            var foundResult = await controller.Delete(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);

            var controller = new FuelsController(fuelsContextMock.Object);


            // Act
            var result = await controller.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            fuelsContextMock.Verify();

        }








    }

}