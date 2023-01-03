using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace If_Employee
{
    public class HoursWorkedPerDay
    {
        public int Id { get; }
        public DateTime Date { get; }
        public int Hours { get; }
        public int Minutes { get; }

        public HoursWorkedPerDay(int id, DateTime date, int hours, int minutes)
        {
            Id = id;
            Date = date;
            Hours = hours;
            Minutes = minutes;
        }
    }
}
