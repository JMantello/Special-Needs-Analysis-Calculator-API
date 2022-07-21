using Microsoft.AspNetCore.Mvc;
using Special_Needs_Analysis_Calculator.Data;
using Special_Needs_Analysis_Calculator.Data.Database;
using Special_Needs_Analysis_Calculator.Data.Models.InputModels;
using Special_Needs_Analysis_Calculator.Data.Models.Login;
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
        public async Task<IActionResult> CreateUser(CreateUserModel createUserModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.CreateUser(createUserModel);

            if(success) return Ok(createUserModel);
            else return BadRequest();
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserModel updateUserModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.UpdateUser(updateUserModel);

            if (success) return Ok(updateUserModel);
            else return BadRequest();
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string sessionToken)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.DeleteUser(sessionToken);

            if (success) return Ok();
            else return BadRequest();
        }

        [HttpPost("AddBeneficiary")]
        public async Task<IActionResult> AddBeneficiary(AddBeneficiaryModel addBeneficiaryModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            bool success = await context.AddBeneficiary(addBeneficiaryModel);

            if (success) return Ok();
            else return BadRequest();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin loginRequest)
        {
            if (!ModelState.IsValid) return BadRequest();

            string? sessionId = await context.Login(loginRequest);

            if (sessionId == null) return Unauthorized();
            else return Ok(sessionId);
        }

        [HttpGet("Dashboard")]
        public async Task<IActionResult> Dashboard(string sessionId)
        {
            if (!ModelState.IsValid) return BadRequest();
            return NotFound();
        }
    }
}
