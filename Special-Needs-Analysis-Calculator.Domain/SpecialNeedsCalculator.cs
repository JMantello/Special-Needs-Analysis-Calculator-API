using Special_Needs_Analysis_Calculator.Data.Models.People;

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
        public double ABLELifetimeValue();
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
                BM.Expenses.Housing +
                BM.Expenses.Food +
                BM.Expenses.Utilities +
                BM.Expenses.Transportation +
                BM.Expenses.MedicalCoPay +
                BM.Expenses.Entertainment +
                BM.Expenses.ConditionCare +
                BM.Expenses.Other;

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
            return IsUnder65();
        }
        // Need Net SSI
        public bool SupplementalSecurityIncomeEligible()
        {
            // Individual income, not household
            double monthlyIncome = (double)BM.YearlyIncome / 12;

            return (IsUnder65() || BM.ConditionStatus.IsLegallyBlind || BM.ConditionStatus.IsLegallyDisabled) && monthlyIncome < 2000;
        }

        public bool SocialSecurityDisabilityInsuranceEligible()
        {
            return IsUnder65();
        }


        // Returning net SSI? Need net SSDI
        public double NetSocialSecurityDisabilityInsurance()
        {
            return BM.SocialSecurityDisabilityInsuranceMonthly * 12 * RemainingDependency;
        }

        // Broken
        public double MaxABLEContribution()
        {
            //Max Holdings?
            return BM.AnnualABLEContributions * BM.ABLEFundRate * Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) / (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1);
        }

        public double RecomendedABLEContribution()
        {
            double recommendedContribution = 100000 * BM.ABLEFundRate *
                Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) /
                (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1);

            return recommendedContribution;
        }

        public double ABLELifetimeValue()
        {
            double ableValue = BM.AnnualABLEContributions *
                (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1) /
                BM.ABLEFundRate;

            return ableValue;
        }

        public BeneficiaryCalculation Results()
        {
            return new BeneficiaryCalculation
            {
                RemainingDependency = GetRemainingDependency(),
                CostMonthly = GetCostMonthly(),
                OverallMonetaryCost = OverallMonetaryCost(),
                IsUnder65 = IsUnder65(),
                SpecialNeedsTrustEligible = SpecialNeedsTrustEligible(),
                SupplementalSecurityIncomeEligible = SupplementalSecurityIncomeEligible(),
                SocialSecurityDisabilityInsuranceEligible = SocialSecurityDisabilityInsuranceEligible(),
                NetSocialSecurityDisabilityInsurance = NetSocialSecurityDisabilityInsurance(),
                MaxABLEContribution = MaxABLEContribution(),
                RecomendedABLEContribution = RecomendedABLEContribution(),
                ABLELifetimeValue = ABLELifetimeValue()
            };
        }
    }
}
