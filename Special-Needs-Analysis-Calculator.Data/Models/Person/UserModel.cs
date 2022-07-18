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
        public List<Dependent>? Dependents { get; set; }       // not everyone has dependents 
        public ContactInfoModel ContactInfo { get; set; }  
    }
}
