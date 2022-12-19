using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace If_Employee.Interfaces
{
    public interface IEmployeeSalaryCalculator
    {
        decimal CalculateDailySalary(int id);
        decimal CalculateMonthlySalary(int id, int month);
    }
}
