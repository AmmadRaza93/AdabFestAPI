using AdabFest_API.Models;
using AdabFest_API.Repositories;
using Microsoft.AspNetCore.Mvc;
 

namespace AdabFest_API.Controllers
{     
    [ApiController]
    [Route("[Controller]")]
    public class SettingController : ControllerBase
    {
        SettingRepository _settingRepo;

        public SettingController()
        {
            _settingRepo = new SettingRepository();
        }
        [HttpGet]
        [Route("All")]
        public async Task<RspSetting> GetSettings()
        {
            var data = _settingRepo.GetAllSettings();
            return await data;
        }

    }
}