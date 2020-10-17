using FuelStation.ViewModels;

namespace FuelStation.Services
{
    public interface IOperationService
    {
        HomeViewModel GetHomeViewModel(int numberRows=10);
        

    }
}
