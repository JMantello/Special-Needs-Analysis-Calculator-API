using Special_Needs_Analysis_Calculator.Data.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Domain
{
    public interface ISpecialNeedsCalculator
    {
        public int GetRemainingDependency();
        public double GetCostMonthly();
        public double OverallMonetaryCost();
        public bool IsUnder65();
        public bool SpecialNeedsTrustEligible();
        public bool SupplementalSecurityIncomeEligible();
        public bool SocialSecurityDisabilityInsuranceEligible();
        public double NetSocialSecurityDisabilityInsurance();
        public double MaxABLEContribution();
        public double RecomendedABLEContribution();
        public double ValueOfAnnualContribution();
    }

    public class SpecialNeedsCalculator : ISpecialNeedsCalculator
    {
        public BeneficiaryModel BM { get; set; }

        // Fields which we only need to calculate once
        public double RemainingDependency { get; set; }
        public double CostMonthly { get; set; }

        public SpecialNeedsCalculator(BeneficiaryModel beneficiaryModel)
        {
            BM = beneficiaryModel;
            RemainingDependency = GetRemainingDependency();
            CostMonthly = GetCostMonthly();
        }

        public int GetRemainingDependency()
        {
            if (BM.ConditionStatus.IsConditionPermanent)
                return BM.ExpectedLifespan - BM.Age;
            else if (BM.IsStudent)
                return Math.Max(24 - BM.Age, 0);
            return Math.Max(19 - BM.Age, 0);
        }

        public double GetCostMonthly()
        {
            double costMonthly =
                BM.Expenses.CostOfHousing +
                BM.Expenses.CostOfFood +
                BM.Expenses.CostOfUtilities +
                BM.Expenses.CostOfTransportation +
                BM.Expenses.CostOfMedicalCoPay +
                BM.Expenses.CostOfEntertainment +
                BM.Expenses.CostOfConditionCare +
                BM.Expenses.CostOther;

            return costMonthly;
        }

        public double OverallMonetaryCost()
        {
            double costTotal = CostMonthly * 12 * RemainingDependency;
            return costTotal;
        }

        public bool IsUnder65()
        {
            return BM.Age < 65;
        }

        public bool SpecialNeedsTrustEligible()
        {
            return IsUnder65() && true; // Does qualify?
        }

        public bool SupplementalSecurityIncomeEligible()
        {
            double monthlyHouseholdIncome = (double)BM.YearlyHouseHoldIncome / 12;

            return (IsUnder65() || BM.ConditionStatus.IsLegallyBlind || BM.ConditionStatus.IsLegallyDisabled) && monthlyHouseholdIncome < 2000;
        }

        public bool SocialSecurityDisabilityInsuranceEligible()
        {
            throw new NotImplementedException();
        }

        public double NetSocialSecurityDisabilityInsurance()
        {
            return BM.SocialSecurityDisabilityInsuranceMonthly * 12 * RemainingDependency;
        }

        public double MaxABLEContribution()
        {
            return BM.AnnualABLEContributions * BM.ABLEFundRate * Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) / (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1);
        }

        public double RecomendedABLEContribution()
        {
            double recommendedContribution = 100000 * BM.ABLEFundRate *
                Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) /
                (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1);

            return recommendedContribution;
        }

        public double ValueOfAnnualContribution()
        {
            double able = BM.AnnualABLEContributions *
                (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1) /
                BM.ABLEFundRate;

            return able;
        }
    }
}
