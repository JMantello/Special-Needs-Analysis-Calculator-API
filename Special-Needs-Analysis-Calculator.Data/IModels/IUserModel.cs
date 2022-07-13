using Special_Needs_Analysis_Calculator.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.IModels
{
    public interface IUserModel
    {
        public void CreateUser(string Username, string Password, ContactInfoModel ContactInfo);
        public void UpdateUserInfo(string username, string argument, string info);
        public void DeleteUser(string Username);
    }
}
