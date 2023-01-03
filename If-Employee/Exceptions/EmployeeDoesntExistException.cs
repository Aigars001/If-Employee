namespace If_Employee.Exceptions
{
    public class EmployeeDoesntExistException : Exception
    {
        public EmployeeDoesntExistException(int id) : base($"Employee with id: {id} doesnt exist") { }
    }
}
