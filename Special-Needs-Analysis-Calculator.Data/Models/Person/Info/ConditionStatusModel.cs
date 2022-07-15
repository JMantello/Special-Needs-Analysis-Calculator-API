using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person.Info
{
    public class ConditionStatusModel
    {
        public bool IsConditionPermanent { get; set; }
        public bool IsLegallyBlind { get; set; }
        public bool IsAbleGroceryShop { get; set; }
        public bool IsAbleDrive { get; set; }

        public ConditionStatusModel (bool isConditionPermanent, bool isLegallyBlind, bool isAbleGroceryShop, bool isAbleDrive)
        {
            this.IsConditionPermanent = isConditionPermanent;
            this.IsLegallyBlind = isLegallyBlind;
            this.IsAbleGroceryShop = isAbleGroceryShop;
            this.IsAbleDrive = isAbleDrive;
        }
    }
}
