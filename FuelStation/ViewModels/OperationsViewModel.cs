using Microsoft.AspNetCore.Mvc.Rendering;
using FuelStation.Models;
using System.Collections.Generic;

namespace FuelStation.ViewModels
{
    public class OperationsViewModel
    {
        public IEnumerable<Operation> Operations { get;set;}
        //�������� ��� ��������� �� ���������
        public PageViewModel PageViewModel { get; set; }
        //������ �������� �����
        public SelectList ListYears {get;set;}
        
    }
}