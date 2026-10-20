using FuelStation.Infrastructure;
using FuelStation.ViewModels;

namespace Tests
{
    public class InfrastructureTest
    {
        [Fact]
        public void Session_SetAndGetDictionary_RoundTrips()
        {
            var session = new FakeSession();
            var dict = new Dictionary<string, string> { ["FuelType"] = "Oil", ["TankType"] = "Big" };

            session.Set("Operation", dict);
            var result = session.Get("Operation");

            Assert.Equal(dict, result);
        }

        [Fact]
        public void Session_GetMissingKey_ReturnsNull()
        {
            var session = new FakeSession();

            Assert.Null(session.Get("missing"));
            Assert.Null(session.Get<FilterOperationViewModel>("missing"));
        }

        [Fact]
        public void Session_SetAndGetGeneric_RoundTrips()
        {
            var session = new FakeSession();

            session.Set("model", new FilterOperationViewModel { FuelType = "Oil", TankType = "Big" });
            var result = session.Get<FilterOperationViewModel>("model");

            Assert.NotNull(result);
            Assert.Equal("Oil", result.FuelType);
            Assert.Equal("Big", result.TankType);
        }

        [Fact]
        public void DictionaryToObject_MapsPropertiesCaseInsensitively_AndConvertsTypes()
        {
            var dict = new Dictionary<string, string>
            {
                ["fueltype"] = "Petrol",
                ["TankType"] = "Small",
                ["Inc_Exp"] = "42",
                ["Unknown"] = "ignored"
            };

            var result = Transformations.DictionaryToObject<FilterOperationViewModel>(dict);

            Assert.Equal("Petrol", result.FuelType);
            Assert.Equal("Small", result.TankType);
            Assert.Equal(42, result.Inc_Exp);
        }

        [Fact]
        public void DictionaryToObject_EmptyDictionary_ReturnsDefaultObject()
        {
            var result = Transformations.DictionaryToObject<FilterOperationViewModel>(new Dictionary<string, string>());

            Assert.NotNull(result);
        }
    }
}
