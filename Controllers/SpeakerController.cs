using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class SpeakerController : ControllerBase
    {
        SpeakerRepository _speakerRepo;

        public SpeakerController()
        {
            _speakerRepo = new SpeakerRepository();
        }
        [HttpGet]
        [Route("All")]
        public async Task<RspSpeaker> GetAll()
        {
            var data = _speakerRepo.GetAllSpeakers();
            return await data;
        }

    }
}