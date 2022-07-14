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
        public async Task<bool> CreateUser(UserModel userInfo)
        {
            if (!ModelState.IsValid) return false;
            try
            {
                UserDocument userDocument = new UserDocument();
                userDocument.User = userInfo;
                context.Users.Add(userDocument);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

    }
}
