using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models
{
    public class PersonModel
    {
        // basic info
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string StateOfResidence { get; set; }

        // advanced info
        public ConditionStatusModel ConditionStatus { get; set; }
        public EligibilityModel Eligibility { get; set; }
        public ExpensesModel Expenses { get; set; }
    }
}
