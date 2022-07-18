using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    
    public class EverythingNeededForSpecialNeedsTrustee
    {
        // Contact Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string StateOfResidence { get; set; }

        // Conditional Status
        public bool IsConditionPermanent { get; set; }
        public bool IsLegallyBlind { get; set; }
        public bool IsAbleGroceryShop { get; set; }
        public bool IsAbleDrive { get; set; }

        // Eligability
        public bool AllowedSocialSecurityDisabilityInsurance { get; set; }
        public bool AllowedSupplimentalSecurityIncome { get; set; }
        public bool AllowedMedicaid { get; set; }
        public bool IsStudent { get; set; }

        // Estimated Expenses
        public double MonthlyExpenses { get; set; }
    }
}
