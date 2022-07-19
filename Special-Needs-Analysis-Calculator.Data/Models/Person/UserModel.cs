using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.People
{
    public class UserModel : PersonModel
    {
        public List<DependentModel>? Dependents { get; set; }
        public bool IsActive { get; set; }

        public UserModel()
        {
            IsActive = true;
        }



    }
}
