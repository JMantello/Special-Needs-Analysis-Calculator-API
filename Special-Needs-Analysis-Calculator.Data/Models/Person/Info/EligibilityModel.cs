using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person.Info
{
    public class EligibilityModel
    {
        public bool AllowedSocialSecurityDisabilityInsurance { get; set; }       // long name not sure what to change to
        public bool AllowedSupplimentalSecurityIncome { get; set; }
        public bool AllowedMedicaid { get; set; }

        public EligibilityModel (bool allowedSocialSecurityDisabilityInsurance, bool allowedSupplimentalSecurityIncome, bool allowedMedicaid)
        {
            this.AllowedSocialSecurityDisabilityInsurance = allowedSocialSecurityDisabilityInsurance;
            this.AllowedSupplimentalSecurityIncome = allowedSupplimentalSecurityIncome;
            this.AllowedMedicaid = allowedMedicaid;
        }
    }
}
