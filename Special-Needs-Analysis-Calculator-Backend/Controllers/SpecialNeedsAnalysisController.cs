using Microsoft.AspNetCore.Mvc;
using Special_Needs_Analysis_Calculator.Data;
using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.People;

namespace Special_Needs_Analysis_Calculator_Backend.Controllers
{
    // Facade structure 
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
        public async Task<IActionResult> CreateUser(UserModel userModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.CreateUser(userModel);

            if(success) return Ok(userModel);
            else return BadRequest();
        }

        [HttpPost("FindUser")]
        public async Task<UserDocument> FindUser(string Email)
        {
            UserDocument userDocument = await context.FindUser(Email);
            if (userDocument.User.IsActive == false) return null;
            return userDocument;
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserModel userModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.UpdateUser(userModel);

            if (success) return Ok(userModel);
            else return BadRequest();
        }

        // Delete User
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.DeleteUser(email);

            if (success) return Ok();
            else return BadRequest();
        }

        // Add Dependant

        // Login

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard(string sessionId)
        {
            if (!ModelState.IsValid) return BadRequest();
            return NotFound();
        }
    }
}
