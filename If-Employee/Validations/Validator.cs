using If_Employee.Exceptions;
using System.Collections.Generic;

namespace If_Employee.Validations
{
    public class Validator
    {
        public static void ValidateId(int id)
        {
            if (char.IsWhiteSpace((char)id))
            {
                throw new InvalidIdException();
            }
        }

        public static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new InvalidNameException();
            }
        }

        public static void ValidateHourlySalary(decimal hourlySalary)
        {
            if (hourlySalary <= 5)
            {
                throw new InvalidHourlySalaryException();
            }
        }
    }
}
