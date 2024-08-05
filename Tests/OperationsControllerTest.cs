using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Mvc;

using Moq.EntityFrameworkCore;
namespace Tests
{
    public class OperationsControllerTest
    {
        [Fact]
        public void GetOperatinList()
        {
            // Arrange
            var fuelsContextMock = new Mock<FuelsContext>();
            var operations = TestDataHelper.GetFakeOperationsList();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);
            FilterOperationViewModel operation = new(); 

            //Act
            OperationsController OperationsController = new(fuelsContextMock.Object);
            var result = OperationsController.Index(operation);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<OperationsViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(TestDataHelper.GetFakeOperationsList().Count, model.Operations.Count());
        }

        [Fact]
        public async void GetOperation()
        {
            // Arrange
            var fuelsContextMock = new Mock<FuelsContext>();
            var operations = TestDataHelper.GetFakeOperationsList();
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuels = TestDataHelper.GetFakeFuelsList();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            fuelsContextMock.Setup(x => x.Fuels).ReturnsDbSet(fuels);
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);

            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            var notFoundResult = await controller.Details(400);
            var foundResult = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }




        [Fact]
        public async Task Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var operations = TestDataHelper.GetFakeOperationsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);

            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            controller.ModelState.AddModelError("error", "some error");
            var result = await controller.Create(operation:null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var operations = TestDataHelper.GetFakeOperationsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);
            Operation operation = new()
            {
                OperationID = 200,
                FuelID = 3,
                TankID = 3,
                Date = DateTime.Now,
                Inc_Exp = -3.13F,
                Fuel = TestDataHelper.GetFakeFuelsList().SingleOrDefault(m => m.FuelID == 3),
                Tank = TestDataHelper.GetFakeTanksList().SingleOrDefault(m => m.TankID == 3)
            };

            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            var result = await controller.Create(operation);

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
            var operations = TestDataHelper.GetFakeOperationsList();
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuels = TestDataHelper.GetFakeFuelsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);

            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            var notFoundResult = await controller.Edit(400);
            var foundResult = await controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            var operations = TestDataHelper.GetFakeOperationsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);
            Operation operation = new()
            {
                OperationID = 200,
                FuelID = 3,
                TankID = 3,
                Date = DateTime.Now,
                Inc_Exp = -3.13F,
                Fuel = TestDataHelper.GetFakeFuelsList().SingleOrDefault(m => m.FuelID == 3),
                Tank = TestDataHelper.GetFakeTanksList().SingleOrDefault(m => m.TankID == 3)
            };


            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            controller.ModelState.AddModelError("error", "some error");
            var result = await controller.Edit(1, operation);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Edit_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var operations = TestDataHelper.GetFakeOperationsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);
            Operation operation = new()
            {
                OperationID = 1,
                FuelID = 3,
                TankID = 3,
                Date = DateTime.Now,
                Inc_Exp = -3.13F,
                Fuel = TestDataHelper.GetFakeFuelsList().SingleOrDefault(m => m.FuelID == 3),
                Tank = TestDataHelper.GetFakeTanksList().SingleOrDefault(m => m.TankID == 3)
            };
            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            var result = await controller.Edit(1, operation);

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
            var operations = TestDataHelper.GetFakeOperationsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);

            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            var notFoundResult = await controller.Delete(40);
            var foundResult = await controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Delete_ReturnsARedirectAndDelete()
        {
            // Arrange
            var operations = TestDataHelper.GetFakeOperationsList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Operations).ReturnsDbSet(operations);


            // Act
            var controller = new OperationsController(fuelsContextMock.Object);
            var result = await controller.DeleteConfirmed(3);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            fuelsContextMock.Verify();

        }

    }

}