using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class OrganizerController : ControllerBase
    {
        OrganizerRepository _organizerRepo;

        public OrganizerController()
        {
            _organizerRepo = new OrganizerRepository();
        }
        //[HttpGet]
        //[Route("All")]
        //public async Task<RspOrganizer> GetALL()
        //{
        //    var data = _organizerRepo.GetAllOrganizer();
        //    return await data;
        //}

    }
}