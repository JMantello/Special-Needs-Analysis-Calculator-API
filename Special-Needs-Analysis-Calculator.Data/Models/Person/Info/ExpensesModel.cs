using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person.Info
{
    public class ExpensesModel
    {
        public int CostOfHousing { get; set; }
        public int CostOfFood { get; set; }
        public int CostOfUtilities { get; set; }
        public int CostOfTransportation { get; set; }
        public int CostOfMedicalCoPay { get; set; }
        public int CostOfEntertainment { get; set; }
        public int CostOfConditionCare { get; set; }
        public int CostOther { get; set; }

        public ExpensesModel (int CostOfHousing, int CostOfFood, int CostOfUtilities, int CostOfTransportation, int CostOfMedicalCoPay, int CostOfEntertainment, int CostOfConditionCare, int CostOther)
        {
            this.CostOfHousing = CostOfHousing;
            this.CostOfFood = CostOfFood;
            this.CostOfUtilities = CostOfUtilities;
            this.CostOfTransportation = CostOfTransportation;
            this.CostOfMedicalCoPay = CostOfMedicalCoPay;
            this.CostOfEntertainment = CostOfEntertainment;
            this.CostOfConditionCare = CostOfConditionCare;
            this.CostOther = CostOther;
        }
    }
}
