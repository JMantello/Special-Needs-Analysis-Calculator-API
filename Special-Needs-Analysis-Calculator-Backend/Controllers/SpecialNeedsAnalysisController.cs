using Microsoft.AspNetCore.Mvc;
using Special_Needs_Analysis_Calculator.Data;

namespace Special_Needs_Analysis_Calculator_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialNeedsAnalysisController : Controller
    {
        private readonly IDatabaseCrud context;


        public SpecialNeedsAnalysisController(IDatabaseCrud context)
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
            return await context.CreateUser(userInfo);
        }

    }
}
