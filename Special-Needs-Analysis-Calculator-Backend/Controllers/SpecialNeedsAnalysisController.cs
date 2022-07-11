using Microsoft.AspNetCore.Mvc;

namespace Special_Needs_Analysis_Calculator_Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpecialNeedsAnalysisController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return "From Special Needs Analysis Controller Index.";
        }
    }
}
