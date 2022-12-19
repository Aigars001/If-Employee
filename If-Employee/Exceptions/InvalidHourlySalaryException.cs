namespace If_Employee.Exceptions
{
    public class InvalidHourlySalaryException : Exception
    {
        public InvalidHourlySalaryException() : base("Employee salary must be higher then 5") {}
    }
}
