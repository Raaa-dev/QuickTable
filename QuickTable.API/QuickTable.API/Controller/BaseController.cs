using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace QuickTable.API.Controller
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class BaseController : ControllerBase
    {
    }
}
