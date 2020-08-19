using Ligg.Abp.Application;
using Ligg.Base.DataModel.ServiceResult;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using static Ligg.Abp.Domain.Shared.Consts;

namespace Ligg.Abp.HttpApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_com)]
    public class UsrController : AbpController
    {
        private readonly IUserService _usrService;

        public UsrController(IUserService usrService)
        {
            _usrService = usrService;
        }


        [HttpGet]
        [Route("[action]")]
        public async Task<ServiceResultT<string>> GetPrf()
        {
            return null;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<string> PutPrf([FromBody] string  content)
        {
            return "";
        }



    }
}