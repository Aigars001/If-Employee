using FluentAssertions;

namespace If_Employee.Test
{
    public class EmployeeTest
    {
        private Employee _employee;

        public EmployeeTest()
        {
            _employee = new Employee(1, "John Doe", 9.2m);
        }

        [Fact]
        public void Employee_Create_Id_FullName_AndHRateSetCorrectly()
        {
            _employee.Id.Should().Be(1);
            _employee.FullName.Should().Be("John Doe");
            _employee.HourlySalary.Should().Be(9.2m);
        }
    }
}