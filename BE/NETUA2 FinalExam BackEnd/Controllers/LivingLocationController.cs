using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NETUA2_FinalExam_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class LivingLocationController : ControllerBase
    {
        private readonly ILogger<LivingLocationController> _logger;
    }
}
