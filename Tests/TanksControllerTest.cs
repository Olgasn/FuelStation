using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq.EntityFrameworkCore;
namespace Tests
{
    public class TanksControllerTest
    {
        [Fact]
        public void GetTankList()
        {
            // Arrange
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(TestDataHelper.GetFakeTanksList());

            //Act
            TanksController TanksController = new(fuelsContextMock.Object);
            var result = TanksController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<TanksViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Tanks.Count());
        }

        [Fact]
        public async void GetTank()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            var controller = new TanksController(fuelsContextMock.Object);

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
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);

            var controller = new TanksController(fuelsContextMock.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Create(tank:null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);

            var controller = new TanksController(fuelsContextMock.Object);


            // Act
            Tank tank = new()
            {
                TankID = 4,
                TankType = "Very big tank",
                TankMaterial = "Steel",
                TankVolume = 55600.4F,
                TankWeight = 20023.14F

            };
            var result = await controller.Create(tank);

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
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            var controller = new TanksController(fuelsContextMock.Object);

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
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);

            var controller = new TanksController(fuelsContextMock.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            Tank tank = new()
            {
                TankID = 4,
                TankType = "Very big tank",
                TankMaterial = "Steel",
                TankVolume = 55600.4F,
                TankWeight = 20023.14F

            };
            var result = await controller.Edit(1, tank);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            var controller = new TanksController(fuelsContextMock.Object);

            // Act
            Tank tank = new()
            {
                TankID = 3,
                TankType = "Very big tank",
                TankMaterial = "Steel",
                TankVolume = 55600.4F,
                TankWeight = 20023.14F

            };
            var result = await controller.Edit(3, tank);

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
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            var controller = new TanksController(fuelsContextMock.Object);

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
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);

            var controller = new TanksController(fuelsContextMock.Object);


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