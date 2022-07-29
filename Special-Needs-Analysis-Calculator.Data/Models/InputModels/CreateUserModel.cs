using System.Globalization;
using System.Text.RegularExpressions;

namespace Special_Needs_Analysis_Calculator.Data.Models.InputModels
{
    public interface ICreateUserModel
    {
        public string CheckInput(CreateUserModel userModel);
        public bool IsValidName(string name);
        public bool IsValidEmail(string email);
    }

    public class CreateUserModel : ICreateUserModel
    {
        public UserModel UserModel { get; set; }
        public string Password { get; set; }

        public string CheckInput(CreateUserModel userModel)
        {
            // name validation
            var Fistname = userModel.UserModel.FirstName;
            var Lastname = userModel.UserModel.LastName;
            if (!IsValidName(Fistname) || !IsValidName(Lastname))               
            {
                return "Invalid name format";
            }

            // email validation
            var Email = userModel.UserModel.Email;
            if (!IsValidEmail(Email))
            {
                return "Invalid email format";
            }
            else return "";
        }

        public bool IsValidName(string Name)
        {
            if (Name == null || Name.Length == 0) return false;
            
            if(Name.Length < 20) return true;
            else return false;
        }

        public bool IsValidEmail(string email) // not my code
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                // Normalise the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                // Examines the domain part of the email and normalises it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();
                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }
            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
