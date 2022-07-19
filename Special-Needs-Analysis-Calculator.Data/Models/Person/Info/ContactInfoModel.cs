using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Person.Info
{
    public class ContactInfoModel
    {
        public string Email { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string? SecondaryPhoneNumber { get; set; }       // don't require secondary

        public ContactInfoModel()
        {

        }

        // constructor overide
        public ContactInfoModel(string Email, string PrimaryPhoneNumber)
        {
            this.Email = Email;
            this.PrimaryPhoneNumber = PrimaryPhoneNumber;
        }

        // constructor overide
        public ContactInfoModel(string Email, string PrimaryPhoneNumber, string? SecondaryPhoneNumber)
        {
            this.Email = Email;
            this.PrimaryPhoneNumber = PrimaryPhoneNumber;
            this.SecondaryPhoneNumber = SecondaryPhoneNumber;
        }
        
    }
}
