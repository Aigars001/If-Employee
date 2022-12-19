using FluentAssertions;
using If_Employee.Calculation;
using If_Employee.Exceptions;

namespace If_Employee.Test
{
    public class CalculationTest
    {
        private readonly List<HoursWorkedPerDay> _hoursWorkedPerDayList;
        private readonly List<Employee> _employeesList;
        private readonly EmployeeSalaryCalculator _employeeSalaryCalculator;
        private readonly EmployeeMonthlyReport _employeeMonthlyReport;
        private readonly EmployeeMonthlyReport _employeeMonthlyReport1;
        private readonly List<EmployeeMonthlyReport> _employeeMonthlyReportList;

        public CalculationTest()
        {
            _employeesList = new List<Employee>
            {
                new Employee(1, "John Doe", 6)
            };

            _hoursWorkedPerDayList = new List<HoursWorkedPerDay>
            {
                new HoursWorkedPerDay(1,DateTime.UtcNow, 8, 0),
                new HoursWorkedPerDay(1,DateTime.UtcNow.AddDays(1), 8, 0),
                new HoursWorkedPerDay(1,DateTime.UtcNow.AddDays(2), 8, 0),
                new HoursWorkedPerDay(1,DateTime.UtcNow.AddMonths(-1), 8, 0),
            };

            _employeeSalaryCalculator = new EmployeeSalaryCalculator(_hoursWorkedPerDayList, _employeesList);
            _employeeMonthlyReport = new EmployeeMonthlyReport(1, DateTime.UtcNow.Year, DateTime.UtcNow.Month, _employeeSalaryCalculator.CalculateMonthlySalary(1, DateTime.UtcNow.Month));
            _employeeMonthlyReport1 = new EmployeeMonthlyReport(1, DateTime.UtcNow.Year, DateTime.UtcNow.AddMonths(-1).Month, _employeeSalaryCalculator.CalculateMonthlySalary(1, DateTime.UtcNow.AddMonths(-1).Month));
            _employeeMonthlyReportList = new List<EmployeeMonthlyReport>
            {
                _employeeMonthlyReport,
                _employeeMonthlyReport1
            };
        }

        [Fact]
        public void CalculateDailySalary_SalaryCalculated()
        {
            _employeeSalaryCalculator.CalculateDailySalary(1).Should().Be(48);       
        }

        [Fact]
        public void EmployeesMonthlyReport_CalculateMonthlySalary()
        {
            _employeeSalaryCalculator.CalculateMonthlySalary(1, DateTime.UtcNow.Month);
            _employeeMonthlyReport.EmployeeId.Should().Be(1);
            _employeeMonthlyReport.Year.Should().Be(DateTime.UtcNow.Year);
            _employeeMonthlyReport.Month.Should().Be(DateTime.UtcNow.Month);
            _employeeMonthlyReport.Salary.Should().Be(144);    
        }

        [Fact]
        public void EmployeesMonthlyReport_CalculateMonthlySalary_CreatedTwoReportsCorrectly()
        {
            _employeeSalaryCalculator.CalculateMonthlySalary(1, DateTime.UtcNow.Month);
            _employeeMonthlyReport1.EmployeeId.Should().Be(_employeeMonthlyReport.EmployeeId);
            _employeeMonthlyReport1.Salary.Should().Be(48);
            _employeeMonthlyReport1.Month.Should().NotBe(_employeeMonthlyReport.Month);
            _employeeMonthlyReportList.Count.Should().Be(2);
        }

        [Fact]
        public void EmployeesMonthlyReport_CalculateMonthlySalary_WrongEmployeeId_ThrowsEmployeeDoesntExistException()
        {
            Action act = () => _employeeSalaryCalculator.CalculateMonthlySalary(4, DateTime.UtcNow.Month);

            act.Should().Throw<EmployeeDoesntExistException>().WithMessage("Employee with id: 4 doesnt exist");
        }

        [Fact]
        public void EmployeesMonthlyReport_CalculateMonthlySalary_WrongEmployeeId_ThrowsMonthErrorException()
        {
            Action act = () => _employeeSalaryCalculator.CalculateMonthlySalary(' ', DateTime.UtcNow.Month);

            act.Should().Throw<InvalidIdException>().WithMessage("Id cant be null or empty");
        }

        [Fact]
        public void EmployeesMonthlyReport_CalculateMonthlySalary_MonthError_ThrowsMonthErrorException()
        {
            Action act = () => _employeeSalaryCalculator.CalculateMonthlySalary(1, 13);

            act.Should().Throw<MonthErrorException>().WithMessage("Months must be 1 - 12");
        }
    }
}
