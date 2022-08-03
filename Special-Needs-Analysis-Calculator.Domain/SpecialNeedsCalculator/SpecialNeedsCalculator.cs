using Special_Needs_Analysis_Calculator.Data.Models;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.Person;

namespace Special_Needs_Analysis_Calculator.Domain.SpecialNeedsCalculator
{
    public class SpecialNeedsCalculator : TemplateSpecialNeedsCalculator
    {
        public UserModel User { get; set; }
        public BeneficiaryModel BM { get; set; }

        // Fields which we only need to calculate once
        public double RemainingDependency { get; set; }
        public double CostMonthly { get; set; }

        // constructor
        public SpecialNeedsCalculator(UserModel user, BeneficiaryModel beneficiaryModel)
        {
            User = user;
            BM = beneficiaryModel;
            RemainingDependency = GetRemainingDependency();
            CostMonthly = GetCostMonthly();
        }

        /// <summary>
        /// Finds the remaining amount of years the benificary is
        /// expected to be dependent on the user
        /// </summary>
        /// <returns>The amount of years they will be dependent</returns>
        public override int GetRemainingDependency()
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
        public override double GetCostMonthly()
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
        public override double OverallMonetaryCost()
        {
            double costTotal = CostMonthly * 12 * RemainingDependency;
            return costTotal;
        }

        /// <summary>
        /// The additional cost of a beneficiary thorughout their life
        /// compared to a beneficiary without the additional cost
        /// </summary>
        /// <returns></returns>
        public override double ExtraMonetaryCost()
        {
            return CostMonthly - (412 + 243 + 819 + 161 + 480); // temp averages might change later
        }

        /// <summary>
        /// Determines if someone is under 65
        /// </summary>
        /// <returns>true / false</returns>
        public override bool IsUnder65()
        {
            return BM.Age < 65;
        }

        /// <summary>
        /// Determines if someone is eligable for the Special
        /// Needs Trust
        /// </summary>
        /// <returns>true/false</returns>
        public override bool SpecialNeedsTrustEligible()
        {
            return IsUnder65();
        }

        /// <summary>
        /// Determines if someone is eligable for Supplimental Security Income
        /// </summary>
        /// <returns>true/false</returns>
        public override bool SupplementalSecurityIncomeEligible()
        {
            // Individual income, not household
            double monthlyIncome = (double)BM.YearlyIncome / 12;

            return (IsUnder65() || BM.ConditionStatus.IsLegallyBlind || BM.ConditionStatus.IsLegallyDisabled) && monthlyIncome < 2000;
        }

        /// <summary>
        /// Calculates total income from Suppimental Security
        /// </summary>
        /// <returns>income total</returns>
        public override double NetSupplementalSecurityIncome()
        {
            return BM.SupplementalSecurityIncomeMonthly * 12 * RemainingDependency;
        }

        /// <summary>
        /// Determines if someone is eligable for Social Security Disability Insurance
        /// </summary>
        /// <returns>true/false</returns>
        public override bool SocialSecurityDisabilityInsuranceEligible()
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
        public override double NetSocialSecurityDisabilityInsurance()
        {
            return BM.SocialSecurityDisabilityInsuranceMonthly * 12 * RemainingDependency;
        }

        /// <summary>
        /// The maximum amount of money someone can contribute to their ABLE account
        /// without missing out on other money
        /// </summary>
        /// <returns>max amount contribution</returns>
        public override double MaxABLEContribution()
        {
            var Numerator = BM.ABLEMaxHoldings * BM.ABLEFundRate * Math.Pow(1 + BM.ABLEFundRate, RemainingDependency);
            var Denomenator = Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1;
            return Numerator / Denomenator;
        }

        /// <summary>
        /// The amount of money someone should contribute to their ABLE account
        /// to earn the most amount of money back
        /// </summary>
        /// <returns>reccommended contribution amount</returns>
        public override double RecommendedABLEContribution()
        {
            var Numerator = 100000 * BM.ABLEFundRate * Math.Pow(1 + BM.ABLEFundRate, RemainingDependency);
            var Denomenator = Math.Pow(1 + BM.ABLEFundRate, RemainingDependency) - 1;
            return Numerator / Denomenator;
        }

        /// <summary>
        /// The total amount of money the ABlE account will hold
        /// at the end of their lifespan
        /// </summary>
        /// <returns>amount of money in ABLE</returns>
        public override double ABLELifetimeValue()
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

        public override List<double> AbleAccountValues()
        {
            double annualContribution = BM.AnnualABLEContributions;
            double growthRate = BM.ABLEFundRate;

            List<double> values = new List<double>();

            double total = 0;

            for(int i = 1; i < RemainingDependency; i++)
            {
                total = (total + annualContribution) * (1 + growthRate);
                values.Add(Math.Round(total, 2, MidpointRounding.AwayFromZero));
            }

            return values;
        }

        public override List<double> SavingsAccountValues()
        {
            double annualContribution = BM.AnnualABLEContributions;
            double growthRate = 1.01;

            List<double> values = new List<double>();
            double total = 0;

            for (int i = 1; i < RemainingDependency; i++)
            {
                total = (total + annualContribution) * (growthRate);
                values.Add(Math.Round(total, 2, MidpointRounding.AwayFromZero));
            }

            return values;
        }

        public override List<double> PostTaxCapitalValues()
        {
            List<double> preTaxValues = AbleAccountValues();
            List<double> postTaxValues = new List<double>();
            // Each value at the end of the year
            foreach (double value in preTaxValues)
            {
                // Determine tax rate
                double taxSum = 0;

                if (User.TaxFilingSatus == TaxFilingSatus.Single)
                {
                    if (value >= 40401 && value <= 445850)
                    {
                        taxSum += value * .15;
                    }
                    else if (value >= 445851)
                    {
                        // determine previous range tax
                        var previousRangeTax = (445850 - 40401) * .15;

                        // new range tax
                        taxSum += previousRangeTax + ((value - 445850) * .20);
                    }
                }

                if (User.TaxFilingSatus == TaxFilingSatus.MarriedJointly)
                {
                    if (value >= 80801 && value <= 501600)
                    {
                        taxSum += value * .15;
                    }
                    else if (value >= 501601)
                    {
                        // determine previous range tax
                        var previousRangeTax = (501600 - 80801) * .15;

                        // new range tax
                        taxSum += previousRangeTax + ((value - 501601) * .20);
                    }
                }

                if (User.TaxFilingSatus == TaxFilingSatus.MarriedSeperately)
                {
                    if (value >= 40401 && value <= 250800)
                    {
                        taxSum += value * .15;
                    }
                    else if (value >= 250801)
                    {
                        // determine previous range tax
                        var previousRangeTax = (250800 - 40401) * .15;

                        // new range tax
                        taxSum += previousRangeTax + ((value - 250801) * .20);
                    }
                }

                if (User.TaxFilingSatus == TaxFilingSatus.HeadOfHousehold)
                {
                    if (value >= 54101 && value <= 473750)
                    {
                        taxSum += value * .15;
                    }
                    else if (value >= 473751)
                    {
                        // determine previous range tax
                        var previousRangeTax = (473750 - 54101) * .15;

                        // new range tax
                        taxSum += previousRangeTax + ((value - 473751) * .20);
                    }
                }
                postTaxValues.Add(value-taxSum);
            }
            return postTaxValues;
        }
    }
}
