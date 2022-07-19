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
        public async Task<bool> CreateUser(UserModel userModel)
        {
            if (!ModelState.IsValid) return false;
            return await context.CreateUser(userModel);
        }

        [HttpPost("FindUser")]
        public async Task<UserDocument> FindUser(string Email)
        {
<<<<<<< HEAD
            return await context.FindUser(Email);
=======
            if (!ModelState.IsValid) return BadRequest();
            return NotFound();
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard(string sessionId)
        {
            if (!ModelState.IsValid) return BadRequest();
            return NotFound();


>>>>>>> 73d4a67 (Update Controller)
        }
    }
}
