using FuelStation.Models;
using System.Collections.Generic;

namespace FuelStation.ViewModels
{
    public class OperationsViewModel
    {
        public IEnumerable<Operation> Operations { get;set;}        
        //�������� ��� ����������
        public FilterOperationViewModel FilterOperationViewModel { get; set; }
        //�������� ��� ��������� �� ���������
        public PageViewModel PageViewModel { get; set; }
        // ������� ����������
        public SortViewModel SortViewModel { get; set; }

    }
}
