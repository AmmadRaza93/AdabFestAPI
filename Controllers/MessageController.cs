using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class MessageController : ControllerBase
    {
        MessageRepository _msgRepo;

        public MessageController()
        {
            _msgRepo = new MessageRepository();
        }
        [HttpGet]
        [Route("All")]
        public async Task<RspMsg> GetAll()
        {
            var data = _msgRepo.GetAllMsgs();
            return await data;
        }

    }
}