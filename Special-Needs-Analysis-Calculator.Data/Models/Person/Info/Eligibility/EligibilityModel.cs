using Special_Needs_Analysis_Calculator.Data.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person.Info.Eligibility
{
    public class EligibilityModel : IEligibilityModel
    {
        public bool AllowedSocialSecurityDisabilityInsurance { get; set; }
        public bool AllowedSupplimentalSecurityIncome { get; set; }
        public bool AllowedMedicaid { get; set; }

        public EligibilityModel(bool allowedSocialSecurityDisabilityInsurance, bool allowedSupplimentalSecurityIncome, bool allowedMedicaid)
        {
            AllowedSocialSecurityDisabilityInsurance = allowedSocialSecurityDisabilityInsurance;
            AllowedSupplimentalSecurityIncome = allowedSupplimentalSecurityIncome;
            AllowedMedicaid = allowedMedicaid;
        }

        public bool IsAllowedSocialSecurityDisabilityInsurance(BeneficiaryModel beneficiary, ConditionStatusModel status)
        {
            return false;
        }

        public bool IsAllowedSupplimentalSecurityIncome(ConditionStatusModel status)
        {
            return false;
        }

        public bool IsAllowedMedicaid(BeneficiaryModel beneficiary)
        {
            return false;
        }
    }
}
