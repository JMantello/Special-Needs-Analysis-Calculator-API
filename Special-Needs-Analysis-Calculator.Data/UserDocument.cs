﻿using Special_Needs_Analysis_Calculator.Data.Models.People;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data
{
    public class UserDocument
    {
        [Key]
        public string Email { get; set; }
        public UserModel User { get; set; }
    }
}
