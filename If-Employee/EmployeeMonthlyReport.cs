using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace If_Employee
{
    public class EmployeeMonthlyReport
    {
        public int EmployeeId { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public decimal Salary { get; set; }

        public EmployeeMonthlyReport(int employeeId, int year, int month, decimal salary)
        {
            EmployeeId = employeeId;
            Year = year;
            Month = month;
            Salary = salary;
        }
    }
}
