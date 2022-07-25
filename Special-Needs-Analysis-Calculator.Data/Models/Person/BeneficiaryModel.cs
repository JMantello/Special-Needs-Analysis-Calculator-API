using Special_Needs_Analysis_Calculator.Data.Models.Person;
using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.People
{
    public class BeneficiaryModel : PersonModel
    {
        public bool IsStudent { get; set; }
        public string StateOfResidence { get; set; }
        public bool IsEmployed { get; set; }
        public int EmploymentYears { get; set; }
        public ExpensesModel? Expenses { get; set; }
    }
}
