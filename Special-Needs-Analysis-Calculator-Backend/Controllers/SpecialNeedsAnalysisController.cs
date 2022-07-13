using Microsoft.AspNetCore.Mvc;
using Special_Needs_Analysis_Calculator.Data;

namespace Special_Needs_Analysis_Calculator_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialNeedsAnalysisController : Controller
    {
        private readonly SpecialNeedsAnalysisDbContext context;

        public SpecialNeedsAnalysisController(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public string Index()
        {
            return "From Special Needs Analysis Controller Index.";
        }

        [HttpPost("CreateUser")]
        public bool CreateUser(UserModel input)
        {
            if (!ModelState.IsValid) return false;

            UserModel newUser = new UserModel();
            newUser.FirstName = input.FirstName;
            newUser.LastName = input.LastName;
            newUser.Email = input.Email;
            
            context.Users.Add(newUser);
            context.SaveChanges();

            return true;
        }

    }
}
