using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class PartnerController : ControllerBase
    {
        PartnerRepository _partnerRepo;

        public PartnerController()
        {
            _partnerRepo = new PartnerRepository();
        }
        [HttpGet]
        [Route("All")]
        public async Task<RspPartner> GetALL()
        {
            var data = _partnerRepo.GetAllPartner();
            return await data;
        }

    }
}