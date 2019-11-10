using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestingAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = nameof(SchemeTwoHandler))]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }
    }
}
