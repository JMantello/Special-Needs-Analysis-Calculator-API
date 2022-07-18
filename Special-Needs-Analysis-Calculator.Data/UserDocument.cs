using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public class UserDocument
    {
        public int Id { get; set; }
        public UserModel User { get; set; }

        public UserDocument(UserModel userModel)
        {
            User = userModel;
        }

        public UserDocument(string firstName, string lastName, string email)
        {
            User = new UserModel()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };
        }
    }
}
