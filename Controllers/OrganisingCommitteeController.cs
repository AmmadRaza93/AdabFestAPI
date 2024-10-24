using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class OrganisingCommitteeController : ControllerBase
    {
        OrganisingCommitteeRepository _organisingCommitteeRepo;

        public OrganisingCommitteeController()
        {
            _organisingCommitteeRepo = new OrganisingCommitteeRepository();
        }
        [HttpGet]
        [Route("All")]
        public async Task<RspOrganisingCommittee> GetAll()
        {
            var data = _organisingCommitteeRepo.GetAll();
            return await data;
        }

    }
}