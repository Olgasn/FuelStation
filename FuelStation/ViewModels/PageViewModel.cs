using System;

namespace FuelStation.ViewModels
{
    //Класс для хранения информации о страницах разбиения
    public class PageViewModel(int count, int pageNumber, int pageSize)
    {
        public int PageNumber { get; private set; } = pageNumber;
        public int TotalPages { get; private set; } = (int)Math.Ceiling(count / (double)pageSize);

        public bool HasPreviousPage
        {
            get
            {
                return (PageNumber > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageNumber < TotalPages);
            }
        }
    }
}
