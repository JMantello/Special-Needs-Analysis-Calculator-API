using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public class DocumentAttribute : Attribute
    {
        public int Id { get; set; }
        public DocumentAttribute()
        {
        }
    }

    [Document]
    public class UserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
