using Special_Needs_Analysis_Calculator.Data.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.InputModels
{
    public interface IAddBeneficiaryModel
    {
        public string CheckInput(BeneficiaryModel beneficiary);
    }

    public class AddBeneficiaryModel : IAddBeneficiaryModel
    {
        public BeneficiaryModel BeneficiaryModel { get; set; }
        public string SessionToken { get; set; }

        public string CheckInput(BeneficiaryModel beneficiary)
        {
            // age check
            if(beneficiary.Age > beneficiary.ExpectedLifespan)
            {
                return "Age cannot be greater than Expected Lifespan";
            }
            return "";
        }
    }
}
