using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly MySingletonService _singleton;

        public TestController(MySingletonService singleton)
        {
            _singleton = singleton;
        }

        [HttpGet]
        public Guid Get() => _singleton.GetScopedId();
    }
}
