using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class BannerController : ControllerBase
    {
        BannerRepository _bannerRepo;
        
        public BannerController()
        {
            _bannerRepo = new BannerRepository();
            
        }
        [HttpGet]
        [Route("ALL")]
        public async Task<RspBanner> AllBanner()
        {
            var data = _bannerRepo.GetAllBanner();
            return await data;
        }
        
    }
} 