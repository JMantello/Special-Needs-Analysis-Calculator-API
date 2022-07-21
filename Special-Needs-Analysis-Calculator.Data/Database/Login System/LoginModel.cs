using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Special_Needs_Analysis_Calculator.Data.Database.Login_System
{

    public static class LoginModel
    {
        /*public string Email { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; set; }*/

        public static string PasswordHash(string password)
        {
            string secretKey = password;
            string salt = "123";        // should be random eventually
            System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create();
            byte[] preHash = System.Text.Encoding.UTF32.GetBytes(secretKey + salt);
            byte[] hash = sha.ComputeHash(preHash);
            string stringy =  System.Convert.ToBase64String(hash);
            return stringy;
        }

        public static async Task<bool> CheckCredentials(string Password, string Email)
        {
            var UserHash = PasswordHash(Password);
            
            var DBHash = "28lDM6Z4ttS6kO0wByJPCETmHWbX9HZj3rgQxneBsss="; //get hash from db and store here

            if (UserHash == DBHash) return true;
            else return false;
        }
    }
}
