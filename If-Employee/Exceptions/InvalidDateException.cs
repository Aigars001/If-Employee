using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace If_Employee.Exceptions
{
    public class InvalidDateException : Exception
    {
        public InvalidDateException() : base("Start date must be before end date") { }
    }
}
