using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models
{
    public class UserModel
    {
        private string Username { get; set; }
        private string Password { get; set; }
        private ContactInfoModel ContactInfo { get; set; }

        public UserModel (string username, string password, ContactInfoModel contactInfo)
        {
            Username = username;
            Password = password;
            ContactInfo = contactInfo;
        }
    }
}
