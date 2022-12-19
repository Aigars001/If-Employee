using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using If_Employee.Exceptions;
using If_Employee.Interfaces;
using If_Employee.Validations;

namespace If_Employee
{
    public class Company : ICompany
    {
        public string Name { get; }
        private readonly List<Employee> _employeesList;
        private readonly List<HoursWorkedPerDay> _hoursWorkedPerDayList;
        private readonly List<EmployeeMonthlyReport> _employeesMonthlyReportList;
        private readonly IEmployeeSalaryCalculator _employeeSalaryCalculator;
        public Employee[] Employees { get; set; }

        public Company(string name, List<Employee> employeesList,
                        List<HoursWorkedPerDay> hoursWorkedPerDayList,
                        List<EmployeeMonthlyReport> employeesMonthlyReportList,
                        IEmployeeSalaryCalculator employeeSalaryCalculator)
        {
            Validator.ValidateName(name);

            Name = name;
            _employeesList = employeesList;
            _hoursWorkedPerDayList = hoursWorkedPerDayList;
            Employees = _employeesList.ToArray<Employee>();
            _employeesMonthlyReportList = employeesMonthlyReportList;
            _employeeSalaryCalculator = employeeSalaryCalculator;
        }

        public void AddEmployee(Employee employee, DateTime contractStartDate)
        {
            Validator.ValidateName(employee.FullName);
            Validator.ValidateHourlySalary(employee.HourlySalary);
            Validator.ValidateId(employee.Id);

            if (_employeesList.Contains(_employeesList.FirstOrDefault(x => x.Id == employee.Id)))
            {
                throw new EmployeeIdAlreadyExistsException(employee.Id);
            }

            _employeesList.Add(employee);
        }

        public EmployeeMonthlyReport[] GetMonthlyReport(DateTime periodStartDate, DateTime periodEndDate)
        {
            return _employeesMonthlyReportList.ToArray<EmployeeMonthlyReport>();
        }

        public void RemoveEmployee(int employeeId, DateTime contractEndDate)
        {
            Validator.ValidateId(employeeId);

            Employee employee = _employeesList.FirstOrDefault(x => x.Id == employeeId);
            
            if(employee == null)
            {
                throw new EmployeeDoesntExistException(employeeId);
            }
            
            _employeesList.Remove(employee);
        }

        public void ReportHours(int employeeId, DateTime dateAndTime, int hours, int minutes)
        {
            Validator.ValidateId(employeeId);

            Employee employee = _employeesList.FirstOrDefault(x => x.Id == employeeId);
            
            if (employee == null)
            {
                throw new EmployeeDoesntExistException(employeeId);
            }

            var day = dateAndTime.Date; 

            _hoursWorkedPerDayList.Add(new HoursWorkedPerDay(employeeId, day, hours, minutes));
        }
    }
}
