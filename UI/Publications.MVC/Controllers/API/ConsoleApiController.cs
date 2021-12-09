using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Publications.MVC.Controllers.API
{
    [ApiController, Route("api/console")]
    public class ConsoleApiController : ControllerBase
    {
        [HttpGet("clear")]
        public void Clear() => Console.Clear();

        [HttpGet("WriteLine/{Message}")]
        public void WriteLine(string Message) => Console.WriteLine(Message);

        [HttpGet("throw/{Message}")]
        public void Throw(string Message) => throw new ApplicationException(Message);
    }
}
