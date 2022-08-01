using Special_Needs_Analysis_Calculator.Data.Models.People;

namespace Special_Needs_Analysis_Calculator.Domain
{
    public interface ISpecialNeedsCalculator
    {
        public int GetRemainingDependency();
        public double GetCostMonthly();
        public double OverallMonetaryCost();
        public double ExtraMonetaryCost();
        public bool IsUnder65();
        public bool SpecialNeedsTrustEligible();
        public bool SupplementalSecurityIncomeEligible();
        public bool SocialSecurityDisabilityInsuranceEligible();
        public double NetSocialSecurityDisabilityInsurance();
        public double NetSupplementalSecurityIncome();
        public double MaxABLEContribution();
        public double RecommendedABLEContribution();
        public double ABLELifetimeValue();
    }

    public class SpecialNeedsCalculator : ISpecialNeedsCalculator
    {
        public BeneficiaryModel BM { get; set; }

        // Fields which we only need to calculate once
        public double RemainingDependency { get; set; }
        public double CostMonthly { get; set; }

        // constructor
        public SpecialNeedsCalculator(BeneficiaryModel beneficiaryModel)
        {
            BM = beneficiaryModel;
            RemainingDependency = GetRemainingDependency();
            CostMonthly = GetCostMonthly();
        }

        /// <summary>
        /// Finds the remaining amount of years the benificary is
        /// expected to be dependent on the user
        /// </summary>
        /// <returns>The amount of years they will be dependent</returns>
        public int GetRemainingDependency()
        {
            if (BM.ConditionStatus.IsConditionPermanent)
                return BM.ExpectedLifespan - BM.Age;
            else if (BM.IsStudent)
                return Math.Max(24 - BM.Age, 0);
            return Math.Max(19 - BM.Age, 0);
        }

        /// <summary>
        /// Finds the monthly finacial cost of a dependent
        /// </summary>
        /// <returns>the monthly cost</returns>
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

        /// <summary>
        /// The overall cost of a beneficiary throughout their expected lifespan
        /// </summary>
        /// <returns>cost of beneficiary</returns>
        public double OverallMonetaryCost()
        {
            double costTotal = CostMonthly * 12 * RemainingDependency;
            return costTotal;
        }

        /// <summary>
        /// The additional cost of a beneficiary thorughout their life
        /// compared to a beneficiary without the additional cost
        /// </summary>
        /// <returns></returns>
        public double ExtraMonetaryCost()
        {
            return CostMonthly - (412 + 243 + 819 + 161 + 480); // temp averages might change later
        }

        /// <summary>
        /// Determines if someone is under 65
        /// </summary>
        /// <returns>true / false</returns>
        public bool IsUnder65()
        {
            return BM.Age < 65;
        }

        /// <summary>
        /// Determines if someone is eligable for the Special
        /// Needs Trust
        /// </summary>
        /// <returns>true/false</returns>
        public bool SpecialNeedsTrustEligible()
        {
            return IsUnder65();
        }
        
        /// <summary>
        /// Determines if someone is eligable for Supplimental Security Income
        /// </summary>
        /// <returns>true/false</returns>
        public bool SupplementalSecurityIncomeEligible()
        {
            // Individual income, not household
            double monthlyIncome = (double)BM.YearlyIncome / 12;

            return (IsUnder65() || BM.ConditionStatus.IsLegallyBlind || BM.ConditionStatus.IsLegallyDisabled) && monthlyIncome < 2000;
        }

        /// <summary>
        /// Calculates total income from Suppimental Security
        /// </summary>
        /// <returns>income total</returns>
        public double NetSupplementalSecurityIncome()
        {
            return BM.SupplementalSecurityIncomeMonthly * 12 * RemainingDependency;
        }

        /// <summary>
        /// Determines if someone is eligable for Social Security Disability Insurance
        /// </summary>
        /// <returns>true/false</returns>
        public bool SocialSecurityDisabilityInsuranceEligible()
        {
            if (!BM.IsEmployed) return false;
            else if (BM.ConditionStatus == null || !BM.ConditionStatus.IsLegallyDisabled) return false;
            else if (!BM.ConditionStatus.IsLegallyBlind && BM.YearlyIncome >= 1350) return false;  // not blind income cutoff
            else if (BM.ConditionStatus.IsLegallyBlind && BM.YearlyIncome >= 2260) return false;   // blind income cutoff
            else return true;
        }

        /// <summary>
        /// Calculates the total Social Security Disability Insurance Income
        /// </summary>
        /// <returns>Insurance Income</returns>
        public double NetSocialSecurityDisabilityInsurance()
        {
            return BM.SocialSecurityDisabilityInsuranceMonthly * 12 * RemainingDependency;  
        }

        /// <summary>
        /// The maximum amount of money someone can contribute to their ABLE account
        /// without missing out on other money
        /// </summary>
        /// <returns>max amount contribution</returns>
        public double MaxABLEContribution()
        {
            var Numerator = BM.ABLEMaxHoldings * BM.ABLEFundRate * Math.Pow((1 + BM.ABLEFundRate), RemainingDependency);
            var Denomenator = Math.Pow((1 + BM.ABLEFundRate), RemainingDependency) -1;
            return Numerator / Denomenator;
        }

        /// <summary>
        /// The amount of money someone should contribute to their ABLE account
        /// to earn the most amount of money back
        /// </summary>
        /// <returns>reccommended contribution amount</returns>
        public double RecommendedABLEContribution()
        {
            var Numerator = 100000 * BM.ABLEFundRate * (Math.Pow((1 + BM.ABLEFundRate), RemainingDependency));
            var Denomenator = (Math.Pow((1+BM.ABLEFundRate), RemainingDependency)) - 1;
            return Numerator / Denomenator;
        }

        /// <summary>
        /// The total amount of money the ABlE account will hold
        /// at the end of their lifespan
        /// </summary>
        /// <returns>amount of money in ABLE</returns>
        public double ABLELifetimeValue()
        {
            double ableValue = BM.AnnualABLEContributions *
                (Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1) /
                BM.ABLEFundRate;

            return ableValue;
        }

        /// <summary>
        /// Assigns method return balues to object feilds
        /// in order to be returned in a simpler way
        /// </summary>
        /// <returns>Object that holds all of the results</returns>
        public BeneficiaryCalculation Results()
        {
            return new BeneficiaryCalculation
            {
                RemainingDependency = GetRemainingDependency(),
                CostMonthly = GetCostMonthly(),
                OverallMonetaryCost = OverallMonetaryCost(),
                ExtraMonthlyCostSpecialNeedsDependent = ExtraMonetaryCost(),
                IsUnder65 = IsUnder65(),
                SpecialNeedsTrustEligible = SpecialNeedsTrustEligible(),
                SupplementalSecurityIncomeEligible = SupplementalSecurityIncomeEligible(),
                NetSupplementalSecurityIncome = NetSupplementalSecurityIncome(),
                SocialSecurityDisabilityInsuranceEligible = SocialSecurityDisabilityInsuranceEligible(),
                NetSocialSecurityDisabilityInsurance = NetSocialSecurityDisabilityInsurance(),
                MaxABLEContribution = MaxABLEContribution(),
                RecommendedABLEContribution = RecommendedABLEContribution(),
                ABLELifetimeValue = ABLELifetimeValue()
            };
        }
    }
}
