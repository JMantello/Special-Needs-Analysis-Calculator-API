using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public class UserDocument
    {
        public string Email { get; set; }
        public UserModel User { get; set; }

        public UserDocument(UserModel userModel)
        {
            Email = userModel.Email;
            User = userModel;
        }

        public UserDocument(string firstName, string lastName, string email)
        {
            Email = email;

            User = new UserModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }
    }
}
