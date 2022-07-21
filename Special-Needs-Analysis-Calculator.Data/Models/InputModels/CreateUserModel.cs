using Special_Needs_Analysis_Calculator.Data.Models.People;

namespace Special_Needs_Analysis_Calculator.Data.Models.InputModels
{
    public class CreateUserModel
    {
        public UserModel UserModel { get; set; }
        public string Password { get; set; }
    }
}
