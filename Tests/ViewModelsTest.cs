using FuelStation.ViewModels;

namespace Tests
{
    public class ViewModelsTest
    {
        [Theory]
        [InlineData(0, 1, 20, 0)]
        [InlineData(20, 1, 20, 1)]
        [InlineData(21, 1, 20, 2)]
        [InlineData(100, 3, 15, 7)]
        public void PageViewModel_CalculatesTotalPages(int count, int page, int size, int expectedTotal)
        {
            var model = new PageViewModel(count, page, size);

            Assert.Equal(expectedTotal, model.TotalPages);
            Assert.Equal(page, model.PageNumber);
        }

        [Fact]
        public void PageViewModel_FirstPage_HasNoPrevious_ButHasNext()
        {
            var model = new PageViewModel(50, 1, 20);

            Assert.False(model.HasPreviousPage);
            Assert.True(model.HasNextPage);
        }

        [Fact]
        public void PageViewModel_LastPage_HasPrevious_ButNoNext()
        {
            var model = new PageViewModel(50, 3, 20);

            Assert.True(model.HasPreviousPage);
            Assert.False(model.HasNextPage);
        }

        [Theory]
        [InlineData(SortState.No, SortState.FuelTypeAsc, SortState.TankTypeAsc, SortState.FuelDensityAsc)]
        [InlineData(SortState.FuelTypeAsc, SortState.FuelTypeDesc, SortState.TankTypeAsc, SortState.FuelDensityAsc)]
        [InlineData(SortState.TankTypeAsc, SortState.FuelTypeAsc, SortState.TankTypeDesc, SortState.FuelDensityAsc)]
        [InlineData(SortState.FuelDensityAsc, SortState.FuelTypeAsc, SortState.TankTypeAsc, SortState.FuelDensityDesc)]
        public void SortViewModel_TogglesOnlyCurrentColumn(SortState current, SortState fuel, SortState tank, SortState density)
        {
            var model = new SortViewModel(current);

            Assert.Equal(current, model.CurrentState);
            Assert.Equal(fuel, model.FuelTypeSort);
            Assert.Equal(tank, model.TankTypeSort);
            Assert.Equal(density, model.FuelDensitySort);
        }
    }
}
