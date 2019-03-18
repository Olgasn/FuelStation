namespace FuelStation.ViewModels
{
    public enum SortState
    {
        No, // не сортировать
        FuelTypeAsc,    // по топливу по возрастанию
        FuelTypeDesc,   // по топливу по убыванию
        TankTypeAsc, // по емкости возрастанию
        TankTypeDesc    // по емкости по убыванию
    }
    public class SortViewModel
    {
        public SortState FuelTypeSort { get; set; } // значение для сортировки по топливу
        public SortState TankTypeSort { get; set; }    // значение для сортировки по емкости
        public SortState CurrentState { get; set; }     // текущее значение сортировки

        public SortViewModel(SortState sortOrder)
        {
            FuelTypeSort = sortOrder == SortState.FuelTypeAsc ? SortState.FuelTypeDesc : SortState.FuelTypeAsc;
            TankTypeSort = sortOrder == SortState.TankTypeAsc ? SortState.TankTypeDesc : SortState.TankTypeAsc;
            CurrentState = sortOrder;
        }



    }
}
