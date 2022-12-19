namespace If_Employee.Exceptions
{
    public class MonthErrorException : Exception
    {
        public MonthErrorException() : base("Months must be 1 - 12") { }
    }
}
