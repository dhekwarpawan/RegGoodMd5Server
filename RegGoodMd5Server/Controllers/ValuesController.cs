using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RegGoodMd5Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult ThrowError()
        {
            throw new Exception("This is a test exception");
        }
    }
}
