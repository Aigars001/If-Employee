using If_Employee.Validations;
using If_Employee.Exceptions;
using If_Employee.Interfaces;

namespace If_Employee.Calculation
{
    public class EmployeeSalaryCalculator : IEmployeeSalaryCalculator
    {
        private readonly List<HoursWorkedPerDay> _hoursWorkedPerDayList;
        private readonly List<Employee> _employeesList;

        public EmployeeSalaryCalculator(List<HoursWorkedPerDay> hoursWorkedPerDayList,
                                        List<Employee> employeesList)
        {
            _hoursWorkedPerDayList = hoursWorkedPerDayList;
            _employeesList = employeesList;
        }

        public decimal CalculateDailySalary(int id)
        {
            Validator.ValidateId(id);
            var employee = _hoursWorkedPerDayList.FirstOrDefault(x => x.Id == id);
            var hourlyRate = _employeesList.FirstOrDefault(x => x.Id == id).HourlySalary;

            var hoursWorked = employee.Hours + (employee.Minutes / 60);

            var dailySalary = hoursWorked * hourlyRate;

            return dailySalary;
        }

        public decimal CalculateMonthlySalary(int id, int month)
        {
            Validator.ValidateId(id);
            var employee = _hoursWorkedPerDayList.FirstOrDefault(x => x.Id == id);         
            
            if (employee == null)
            {
                throw new EmployeeDoesntExistException(id);
            }
            
            var employeeHoursWorked = _hoursWorkedPerDayList.FindAll(x => x.Id == id);


            if(month < 1 || month > 12)
            {
                throw new MonthErrorException();
            }

            return employeeHoursWorked.Where(x => x.Date.Month == month).Sum(s => CalculateDailySalary(id));
        }
    }
}
