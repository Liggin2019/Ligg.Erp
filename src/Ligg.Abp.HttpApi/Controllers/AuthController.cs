
using Ligg.Abp.Application;
using Ligg.Base.DataModel.ServiceResult;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using static Ligg.Abp.Domain.Shared.Consts;

namespace Ligg.Abp.HttpApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_com)]
    public class AuthController : AbpController
    {
        private readonly IUserService _usrService;

        public AuthController(IUserService usrService)
        {
            _usrService = usrService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ServiceResult> Logon(string userCode,string userPass)
        {
            return await _usrService.Logon(userCode,userPass);
        }



    }
}