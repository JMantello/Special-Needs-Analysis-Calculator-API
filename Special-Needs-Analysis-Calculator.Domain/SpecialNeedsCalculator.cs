using Special_Needs_Analysis_Calculator.Dat.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.People;
using Special_Needs_Analysis_Calculator.Data.Models.Person.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Domain
{
    public enum DisabilityType
    {
        Temporary,
        Student,
        Permanent
    }
    public interface ISpecialNeedsCalculator
    {
        ///// <summary>
        ///// Determine how long the beneficiary will remain a dependent.
        ///// </summary>
        ///// <returns>int Years</returns>
        //public int RemainingDependancy();

        ///// <summary>
        ///// Adds the monthly costs of beneficiary.
        ///// </summary>
        ///// <param name="foodMonthly"></param>
        ///// <param name="entertainmentMonthly"></param>
        ///// <param name="transportationMonthly"></param>
        ///// <param name="medicalCopayMonthly"></param>
        ///// <param name="clothingMonthly"></param>
        ///// <param name="housingCareMonthly"></param>
        ///// <param name="therapyMonthly"></param>
        ///// <param name="otherMonthly"></param>
        ///// <returns>The dependent's monthly expenses</returns>
        //public double CostMonthly(double foodMonthly, double entertainmentMonthly, double transportationMonthly, double medicalCopayMonthly, double clothingMonthly, double housingCareMonthly, double therapyMonthly, double otherMonthly);

        ///// <summary>
        ///// The overall cost of the dependent throughout their life.
        ///// </summary>
        ///// <param name="costMonthly"></param>
        ///// <returns></returns>
        //public double OverallMonetaryCost(double costMonthly);


    }

    public class SpecialNeedsCalculator : ISpecialNeedsCalculator
    {
        public BeneficiaryModel BeneficiaryModel { get; set; }

        // Client information
        public int clientAge { get; set; }
        public int expectedLifespan { get; set; }
        public DisabilityType disabilityType { get; set; }

        // Monthly Expenses
        public double foodMonthly { get; set; }
        public double entertainmentMonthly { get; set; }
        public double transportationMonthly { get; set; }
        public double medicalCopayMonthly { get; set; }
        public double clothingMonthly { get; set; }
        public double housingCareMonthly { get; set; }
        public double therapyMonthly { get; set; }
        public double otherMonthly { get; set; }

        // Eligability 
        public bool qualifySNT { get; set; }

        // Existing beneficiary model
        public bool IsStudent { get; set; }
        public bool IsLegallyBlind { get; set; }
        public bool IsDisabled { get; set; }
        public double Income { get; set; }
        public double SSDI_Monthly { get; set; }
        public double FundRate { get; set; }
        public double Max_ABLE_Holdings { get; set; }

        // Fields which we only need to calculate once
        public double RemainingDependency { get; set; }

        public SpecialNeedsCalculator(BeneficiaryModel beneficiaryModel)
        {

        }

        public int RemainingDependancy() // Store this in a field, so we don't have to always recalculate
        {
            if (disabilityType == DisabilityType.Permanent)
                return expectedLifespan - clientAge;
            else if (IsStudent)
                return Math.Max(24 - clientAge, 0);
            return Math.Max(19 - clientAge, 0);
        }

        public double CostMonthly()
        {
            double costMonthly = foodMonthly + entertainmentMonthly + transportationMonthly + medicalCopayMonthly + clothingMonthly + housingCareMonthly + therapyMonthly + otherMonthly;
            return costMonthly;
        }

        public double OverallMonetaryCost()
        {
            double costTotal = CostMonthly() * 12 * RemainingDependancy();
            return costTotal;
        }

        public bool isUnder65()
        {
            return clientAge < 65;
        }

        public bool SNT_Eligable() // Special Needs Trust
        {
            return isUnder65() && qualifySNT;
        }

        public bool SSI_Eligable() // Supplemental Security Income Payments
        {
            return (isUnder65() || IsLegallyBlind || IsDisabled) && Income < 2000;
        }

        public bool SSDI_Eligable() // Social Security Disability Insurance
        {
            throw new NotImplementedException();
        }

        public double Net_SSDI() 
        {
            return SSDI_Monthly * 12 * RemainingDependancy(); 
        }

        public double Max_ABLE_Contribution()
        {
            return Max_ABLE_Holdings * FundRate * Math.Pow(1 + FundRate, RemainingDependancy()) / Math.Pow(1 + FundRate, RemainingDependancy()) - 1;
        }

    }
}
