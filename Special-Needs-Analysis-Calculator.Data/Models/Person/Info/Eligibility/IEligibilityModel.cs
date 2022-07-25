using Special_Needs_Analysis_Calculator.Data.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person.Info.Eligibility
{
    public interface IEligibilityModel
    {
        public bool IsAllowedSocialSecurityDisabilityInsurance(BeneficiaryModel beneficiary, ConditionStatusModel status);
        public bool IsAllowedSupplimentalSecurityIncome(ConditionStatusModel status);
        public bool IsAllowedMedicaid(BeneficiaryModel beneficiary);
        public bool IsAllowedAbleAccount();
        public bool IsAllowedSpecialNeedsTrust();
    }
}
