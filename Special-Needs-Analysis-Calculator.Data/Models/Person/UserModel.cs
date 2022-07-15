using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.People
{
    public class UserModel : PersonModel
    {
        public List<string> Dependents { get; set; }
        public ContactInfoModel ContactInfo { get; set; }
        public string Email { get; set; }
    }
}
