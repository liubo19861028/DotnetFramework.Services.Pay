using Dotnet.Services.Base;
using Dotnet.Services.Pay.Payments.Wechatpay.Configs;
using Dotnet.Services.Pay.Payments.Wechatpay.Parameters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet.Services.Pay.Api
{
    [Route("WechatpayApi")]
    public class WechatpayApiController : BaseApiController
    {
        [HttpPost]
        [Route("ParameterBuilder")]
        public IActionResult ParameterBuilder()
        {
            WechatpayParameterBuilder _builder = new WechatpayParameterBuilder(new WechatpayConfig());

            return new JsonResult(_builder);
        }


       /* [HttpPost]
        [Route("VerifyMobile")]
        public IActionResult ParameterBuilder()
        {
            _builder = new WechatpayParameterBuilder(new WechatpayConfig());

            return Ok(_builder);
        }*/

    }
}
