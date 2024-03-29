using FuelStation.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FuelStation.ViewModels
{
    public class OperationsViewModel
    {
        public IEnumerable<Operation> Operations { get; set; }

        //�������� ��� ����������
        public OperationViewModel OperationViewModel { get; set; }
        //�������� ��� ��������� �� ���������
        public PageViewModel PageViewModel { get; set; }
        //������ �������� �����
        public SelectList ListYears { get; set; }

    }
}
