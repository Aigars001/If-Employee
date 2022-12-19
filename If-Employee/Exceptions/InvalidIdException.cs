namespace If_Employee.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base("Id cant be null or empty") { }
    }
}
