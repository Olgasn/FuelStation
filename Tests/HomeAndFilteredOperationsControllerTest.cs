using FuelStation.DataLayer.Data;
using FuelStation.Infrastructure;
using FuelStation.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq.EntityFrameworkCore;

namespace Tests
{
    public class HomeAndFilteredOperationsControllerTest
    {
        private static Mock<FuelsContext> CreateContext()
        {
            var mock = new Mock<FuelsContext>();
            mock.Setup(x => x.Fuels).ReturnsDbSet(TestDataHelper.GetFakeFuelsList());
            mock.Setup(x => x.Tanks).ReturnsDbSet(TestDataHelper.GetFakeTanksList());
            mock.Setup(x => x.Operations).ReturnsDbSet(TestDataHelper.GetFakeOperationsList());
            return mock;
        }

        private static FilteredOperationsController CreateFilteredController(FuelsContext context, FakeSession session)
        {
            return new FilteredOperationsController(context)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext { Session = session }
                }
            };
        }

        [Fact]
        public void Home_Index_ReturnsDataSortedByDateDescending()
        {
            var controller = new HomeController(CreateContext().Object);

            var result = controller.Index();

            var model = Assert.IsType<HomeViewModel>(Assert.IsType<ViewResult>(result).Model);
            Assert.Equal(3, model.Fuels.Count());
            Assert.Equal(3, model.Tanks.Count());
            Assert.Equal(9, model.Operations.Count());
            Assert.Equal(model.Operations.OrderByDescending(o => o.Date), model.Operations);
        }

        [Fact]
        public void Filtered_Index_Get_WithoutSession_ReturnsAllOperations()
        {
            var controller = CreateFilteredController(CreateContext().Object, new FakeSession());

            var result = controller.Index(SortState.No);

            var model = Assert.IsType<OperationsViewModel>(Assert.IsType<ViewResult>(result).Model);
            Assert.Equal(9, model.Operations.Count());
        }

        [Fact]
        public void Filtered_Index_Get_SortsByFuelTypeDescending()
        {
            var controller = CreateFilteredController(CreateContext().Object, new FakeSession());

            var result = controller.Index(SortState.FuelTypeDesc);

            var model = Assert.IsType<OperationsViewModel>(Assert.IsType<ViewResult>(result).Model);
            var types = model.Operations.Select(o => o.Fuel.FuelType).ToList();
            Assert.Equal(types.OrderByDescending(t => t), types);
            Assert.Equal(SortState.FuelTypeDesc, model.SortViewModel.CurrentState);
        }

        [Fact]
        public void Filtered_Index_Get_UsesSortStateFromSession_WhenNoSortRequested()
        {
            var session = new FakeSession();
            session.Set("SortState", new Dictionary<string, string> { ["sortOrder"] = nameof(SortState.TankTypeAsc) });
            var controller = CreateFilteredController(CreateContext().Object, session);

            var result = controller.Index(SortState.No);

            var model = Assert.IsType<OperationsViewModel>(Assert.IsType<ViewResult>(result).Model);
            Assert.Equal(SortState.TankTypeAsc, model.SortViewModel.CurrentState);
        }

        [Fact]
        public void Filtered_Index_Get_UsesFilterFromSession()
        {
            var session = new FakeSession();
            session.Set("Operation", new Dictionary<string, string> { ["FuelType"] = "Kerosene", ["TankType"] = "" });
            var controller = CreateFilteredController(CreateContext().Object, session);

            var result = controller.Index(SortState.No);

            var model = Assert.IsType<OperationsViewModel>(Assert.IsType<ViewResult>(result).Model);
            Assert.All(model.Operations, o => Assert.Contains("Kerosene", o.Fuel.FuelType));
        }

        [Fact]
        public void Filtered_Index_Post_FiltersByFuelAndTank()
        {
            var session = new FakeSession();
            session.Set("SortState", new Dictionary<string, string> { ["sortOrder"] = nameof(SortState.No) });
            var controller = CreateFilteredController(CreateContext().Object, session);
            var filter = new FilterOperationViewModel { FuelType = "Kerosene", TankType = "Small" };

            var result = controller.Index(filter);

            var model = Assert.IsType<OperationsViewModel>(Assert.IsType<ViewResult>(result).Model);
            Assert.All(model.Operations, o =>
            {
                Assert.Contains("Kerosene", o.Fuel.FuelType);
                Assert.Contains("Small", o.Tank.TankType);
            });
            Assert.Same(filter, model.FilterOperationViewModel);
        }

        [Fact]
        public void Filtered_Index_Post_NoMatches_ReturnsEmptyList()
        {
            var session = new FakeSession();
            session.Set("SortState", new Dictionary<string, string> { ["sortOrder"] = nameof(SortState.No) });
            var controller = CreateFilteredController(CreateContext().Object, session);

            var result = controller.Index(new FilterOperationViewModel { FuelType = "NoSuchFuel", TankType = "" });

            var model = Assert.IsType<OperationsViewModel>(Assert.IsType<ViewResult>(result).Model);
            Assert.Empty(model.Operations);
        }
    }
}
