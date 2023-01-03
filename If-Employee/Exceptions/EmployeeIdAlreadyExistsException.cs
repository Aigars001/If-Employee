namespace If_Employee.Exceptions
{
    public class EmployeeIdAlreadyExistsException : Exception
    {
        public EmployeeIdAlreadyExistsException(int id) : base($"Employee with id: {id} already exists") { }
    }
}
