using Microsoft.AspNetCore.Mvc;
using Special_Needs_Analysis_Calculator.Data;
using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.People;

namespace Special_Needs_Analysis_Calculator_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialNeedsAnalysisController : Controller
    {
        private readonly SpecialNeedsAnalysisDbContext context;
        private readonly UserDB user;

        public SpecialNeedsAnalysisController(SpecialNeedsAnalysisDbContext context)
        {
            this.context = context;
            this.user = new UserDB(context);
        }

        [HttpGet]
        public string Index()
        {
            return "From Special Needs Analysis Controller Index.";
        }

        [HttpPost("AddUser")]
        public async Task<bool> AddUser(UserModel userInfo)
        {
            return await user.AddUser(userInfo);
        }

        [HttpPost("FindUser")]
        public async Task<UserDocument> FindUser(int Id)
        {
            return await user.FindUser(Id);
        }
    }
}
