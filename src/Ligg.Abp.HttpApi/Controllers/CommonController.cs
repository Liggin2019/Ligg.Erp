
using Ligg.Abp.Application;
using Ligg.Abp.Application.Contracts;
using Ligg.Base.DataModel.ServiceResult;
using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using static Ligg.Abp.Domain.Shared.Consts;

namespace Ligg.Abp.HttpApi.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = Grouping.GroupName_com)]
    public class CommonController : AbpController
    {
        private readonly IUserService _usrService;
        private readonly ITransactionService _transactionService;

        public CommonController(IUserService usrService, ITransactionService transactionService)
        {
            _usrService = usrService;
            _transactionService = transactionService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ServiceResultT<IEnumerable<TransactionListTreeDto>>> GetLmn(string param)
        {
            return await _transactionService.GetLeftMenuItems(param);
        }



    }
}