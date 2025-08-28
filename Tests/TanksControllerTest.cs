using FuelStation.DataLayer.Data;
using FuelStation.DataLayer.Models;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq.EntityFrameworkCore;


namespace Tests
{
    public class TanksControllerTest
    {
        private readonly Mock<IWebHostEnvironment> _mockWebHostEnvironment;

        public TanksControllerTest()
        {
            _mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockWebHostEnvironment.Setup(m => m.WebRootPath).Returns("wwwroot");
        }

        [Fact]
        public void GetTankList()
        {
            // Arrange
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(TestDataHelper.GetFakeTanksList());

            //Act
            TanksController tanksController = new(fuelsContextMock.Object, _mockWebHostEnvironment.Object);
            var result = tanksController.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult);
            var model = Assert.IsAssignableFrom<TanksViewModel>(
                viewResult.ViewData.Model);
            Assert.Equal(3, model.Tanks.Count());
        }

        [Fact]
        public async Task GetTank()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            var controller = new TanksController(fuelsContextMock.Object, _mockWebHostEnvironment.Object);

            // Act
            var notFoundResult = await controller.Details(4);
            var foundResult = await controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Create_ReturnsView_GivenInvalidModel()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);

            var controller = new TanksController(fuelsContextMock.Object, _mockWebHostEnvironment.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Create(tank: null);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsARedirectAndCreate_WhenModelStateIsValid()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);
            fuelsContextMock.Setup(x => x.Add(It.IsAny<Tank>())).Verifiable();
            fuelsContextMock.Setup(x => x.SaveChangesAsync(default)).ReturnsAsync(1);

            var controller = new TanksController(fuelsContextMock.Object, _mockWebHostEnvironment.Object);

            // Создаем и настраиваем HttpContext с формой
            var httpContext = new DefaultHttpContext();
            httpContext.Request.ContentType = "multipart/form-data";

            // Создаем пустую форму (без файлов)
            var formCollection = new FormCollection(new Dictionary<string, StringValues>(),
                new FormFileCollection());
            httpContext.Request.Form = formCollection;

            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };

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
            var controller = new TanksController(fuelsContextMock.Object, _mockWebHostEnvironment.Object);

            // Act
            var notFoundResult = await controller.Edit(4);
            var foundResult = await controller.Edit(3);

            // Assert
            Assert.IsType<NotFoundResult>(notFoundResult);
            Assert.IsType<ViewResult>(foundResult);
        }

        [Fact]
        public async Task Edit_ReturnsView_WhenModelStateIsInvalid()
        {
            // Arrange
            var tanks = TestDataHelper.GetFakeTanksList();
            var fuelsContextMock = new Mock<FuelsContext>();
            fuelsContextMock.Setup(x => x.Tanks).ReturnsDbSet(tanks);

            var controller = new TanksController(fuelsContextMock.Object, _mockWebHostEnvironment.Object);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            Tank tank = new()
            {
                TankID = 3,  // Этот ID совпадает с ID в маршруте
                TankType = "Very big tank",
                TankMaterial = "Steel",
                TankVolume = 55600.4F,
                TankWeight = 20023.14F
            };
            var result = await controller.Edit(3, tank);  // ID в маршруте = 3, ID модели = 3

            // Assert
            Assert.IsType<ViewResult>(result);
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
                TankWeight = 20023.14F,
                TankPicture= "d368fc74 - a5a6 - 49e0 - 8117 - 13a02c431f24_1756385278.png"
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
            var controller = new TanksController(fuelsContextMock.Object, _mockWebHostEnvironment.Object);

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