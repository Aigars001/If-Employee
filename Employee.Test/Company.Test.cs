using FluentAssertions;
using If_Employee.Exceptions;
using If_Employee.Interfaces;
using If_Employee.Calculation;

namespace If_Employee.Test
{
    public class CompanyTest
    {
        private readonly ICompany _company;
        private readonly List<Employee> _employeesList;
        private readonly List<HoursWorkedPerDay> _hoursWorkedPerDayList;
        private readonly List<EmployeeMonthlyReport> _employeeMonthlyReportList;
        private readonly EmployeeSalaryCalculator _employeeSalaryCalculator;

        public CompanyTest()
        {
            _employeesList = new List<Employee>
            {
                new Employee(1, "John Doe", 9.2m),
                new Employee(2, "Jane Doe", 9.0m)
            };

            _hoursWorkedPerDayList = new List<HoursWorkedPerDay>
            {
                new HoursWorkedPerDay(1, DateTime.UtcNow.AddDays(-1), 8, 0),
                new HoursWorkedPerDay(1, DateTime.UtcNow.AddDays(-2), 8, 0),
                new HoursWorkedPerDay(1, DateTime.UtcNow.AddDays(-3), 8, 0),
                new HoursWorkedPerDay(2, DateTime.UtcNow.Date, 8, 0)
            };
            _employeeSalaryCalculator = new EmployeeSalaryCalculator(_hoursWorkedPerDayList, _employeesList);

            _employeeMonthlyReportList = new List<EmployeeMonthlyReport>
            {
                new EmployeeMonthlyReport(1, DateTime.UtcNow.Year, DateTime.UtcNow.Month, _employeeSalaryCalculator.CalculateMonthlySalary(1, DateTime.UtcNow.Month)),
                new EmployeeMonthlyReport(2, DateTime.UtcNow.Year, DateTime.UtcNow.Month, _employeeSalaryCalculator.CalculateMonthlySalary(2, DateTime.UtcNow.Month))
            };

            _company = new Company("Company Name", _employeesList,
                                    _hoursWorkedPerDayList,
                                    _employeeMonthlyReportList,
                                    _employeeSalaryCalculator);
        }

        [Fact]
        public void CompanyCreated_ValidCompany()
        {
            _company.Name.Should().Be("Company Name");
        }

        [Fact]
        public void CompanyCreated_InvalidCompanyName_ThroesInvalidNameException()
        {
            FluentActions.Invoking(() => new Company("", _employeesList, _hoursWorkedPerDayList, _employeeMonthlyReportList, _employeeSalaryCalculator))
            .Should().Throw<InvalidNameException>()
            .WithMessage("Name cannot be null or empty");

        }

        [Fact]
        public void CompanyAddEmployee_EmployeeAdded()
        {
            var newEmployee = new Employee(3, "Michael Stone", 8.2m);

            _company.AddEmployee(newEmployee, DateTime.UtcNow);

            _employeesList.Count.Should().Be(3);
        }
        [Fact]
        public void CompanyAddEmployee_InvalidEmployeeId_ThrowsInvalidIdException()
        {
            var newEmployee = new Employee(' ', "Michael Stone", 8.2m);

            Action act = () => _company.AddEmployee(newEmployee, DateTime.UtcNow);

            act.Should().Throw<InvalidIdException>().WithMessage("Id cant be null or empty");
        }

        [Fact]
        public void CompanyAddEmployee_EmployeeSalaryTooLow_ThrowsInvalidHourlySalaryException()
        {
            var newEmployee = new Employee(3, "Michael Stone", 4.2m);

            Action act = () => _company.AddEmployee(newEmployee, DateTime.UtcNow);

            act.Should().Throw<InvalidHourlySalaryException>().WithMessage("Employee salary must be higher then 5");
        }

        [Fact]
        public void CompanyAddEmployee_EmployeeNameError_ThrowsInvalidNameException()
        {
            var newEmployee = new Employee(3, "", 5.1m);

            Action act = () => _company.AddEmployee(newEmployee, DateTime.UtcNow);

            act.Should().Throw<InvalidNameException>().WithMessage("Name cannot be null or empty");
        }
        [Fact]
        public void CompanyAddEmployee_EmployeeExists_ThrowsEmployeeIdAlreadyExistsException()
        {
            var newEmployee = new Employee(1, "Michael Stone", 5.1m);

            Action act = () => _company.AddEmployee(newEmployee, DateTime.UtcNow);

            act.Should().Throw<EmployeeIdAlreadyExistsException>().WithMessage("Employee with id: 1 already exists");
        }

        [Fact]
        public void CompanyRemoveEmployee_EmployeeRemoved()
        {
            _company.RemoveEmployee(2, DateTime.UtcNow);

            _employeesList.Count.Should().Be(1);
        }

        [Fact]
        public void CompanyRemoveEmployee_InvalidEmployeeId_ThrowsInvalidIdException()
        {

            Action act = () => _company.RemoveEmployee(' ', DateTime.UtcNow);

            act.Should().Throw<InvalidIdException>().WithMessage("Id cant be null or empty");
        }

        [Fact]
        public void CompanyRemoveEmployee_EmployeeIDDoesntExist_ThrowsEmployeeDoesntExistException()
        {
            Action act = () => _company.RemoveEmployee(5, DateTime.UtcNow);

            act.Should().Throw<EmployeeDoesntExistException>().WithMessage("Employee with id: 5 doesnt exist");
        }
    
        [Fact]
        public void CompanyReportHoursWorktPerDay_ReportAdded()
        {
            _company.ReportHours(2, DateTime.UtcNow.AddDays(-1), 8, 0);

            var test = new HoursWorkedPerDay(2, DateTime.UtcNow.AddDays(-1).Date, 8, 0);

            var actual = _hoursWorkedPerDayList.Last(x => x.Id == 2);

            actual.Should().BeEquivalentTo(test);
            _hoursWorkedPerDayList.Where(x => x.Id == 2).Count().Should().Be(2);
        }

        [Fact]
        public void CompanyReportHoursWorktPerDay_InvalidEmployeeId_ThrowsInvalidIdException()
        {
            Action act = () => _company.ReportHours(' ', DateTime.UtcNow, 8, 0);

            act.Should().Throw<InvalidIdException>().WithMessage("Id cant be null or empty");
        }

        [Fact]
        public void CompanyMonthlyReport_CreateMonthlyreport()
        {
            var firstDay = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).Date;
            var lastDay = firstDay.AddMonths(1).AddDays(-1).Date;

            var result = _company.GetMonthlyReport(firstDay, lastDay);

            result[0].EmployeeId.Should().Be(1);
            result[0].Salary.Should().Be(220.8m);
            result[1].EmployeeId.Should().Be(2);
            result[1].Salary.Should().Be(72m);
        }

        [Fact]
        public void CompanyMonthlyReport_WroingDate_ThrowsInvalidDateException()
        {
            var firstDay = new DateTime(2022, 12, 13);
            var lastDay = new DateTime(2022, 12, 6);

            Action act = () => _company.GetMonthlyReport(firstDay, lastDay);

            act.Should().Throw<InvalidDateException>().WithMessage("Start date must be before end date");
        }
    }
}
