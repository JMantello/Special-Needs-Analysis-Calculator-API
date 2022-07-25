using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person
{
    public class PersonModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int YearlyHouseHoldIncome { get; set; }
        public int NumPeopleInHousHold { get; set; }
        public int Age { get; set; }
    }
}
