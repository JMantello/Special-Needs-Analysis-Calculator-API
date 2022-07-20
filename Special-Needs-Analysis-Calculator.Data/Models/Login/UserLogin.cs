using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Models.Login
{
    public class UserLogin
    {
        [Key]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
