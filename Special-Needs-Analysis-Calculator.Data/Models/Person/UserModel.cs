using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.People
{
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string? SecondaryPhoneNumber { get; set; }
        public bool IsAccountActive { get; set; }
        public bool IsOwnBeneficiary { get; set; }
        public List<BeneficiaryModel>? Beneficiaries { get; set; }

        public UserModel()
        {
            IsAccountActive = true;
            Beneficiaries = new List<BeneficiaryModel>();
        }
    }
}
