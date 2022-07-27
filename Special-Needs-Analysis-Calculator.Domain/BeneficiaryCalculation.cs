using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Domain
{
    public class BeneficiaryCalculation
    {
        public double RemainingDependency { get; set; }
        public double CostMonthly { get; set; }
        public double OverallMonetaryCost { get; set; }
        public bool IsUnder65 { get; set; }
        public bool SpecialNeedsTrustEligible { get; set; }
        public bool SupplementalSecurityIncomeEligible { get; set; }
        public bool SocialSecurityDisabilityInsuranceEligible { get; set; }
        public double NetSocialSecurityDisabilityInsurance { get; set; }
        public double MaxABLEContribution { get; set; }
        public double RecomendedABLEContribution { get; set; }
        public double ABLELifetimeValue { get; set; }
        
    }
}
