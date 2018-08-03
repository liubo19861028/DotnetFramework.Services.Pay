using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Dotnet.Services.Pay.Payments.Alipay.Abstractions;
using Dotnet.Services.Pay.Payments.Alipay.Configs;
using Dotnet.Services.Pay.Payments.Alipay.Parameters;
using Dotnet.Services.Pay.Payments.Alipay.Parameters.Requests;
using Dotnet.Services.Pay.Payments.Alipay.Services.Base;
using Dotnet.Services.Pay.Payments.Core;
using Dotnet.Utility;

namespace Dotnet.Services.Pay.Payments.Alipay.Services {
    /// <summary>
    /// 支付宝电脑网站支付服务
    /// </summary>
    public class AlipayPagePayService : AlipayServiceBase, IAlipayPagePayService {
        /// <summary>
        /// 初始化支付宝电脑网站支付服务
        /// </summary>
        /// <param name="provider">支付宝配置提供器</param>
        public AlipayPagePayService( IAlipayConfigProvider provider ) : base( provider ) {
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="param">支付参数</param>
        public override async Task<PayResult> PayAsync( PayParam param ) {
            var config = await ConfigProvider.GetConfigAsync();
            Validate( config, param );
            var builder = new AlipayParameterBuilder( config );
            Config( builder, param );
            var form = GetForm( builder );
            WriteLog( config, builder, form );
            return new PayResult { Result = form };
        }

        /// <summary>
        /// 获取表单
        /// </summary>
        private string GetForm( AlipayParameterBuilder builder ) {
            FormBuilder formBuilder = new FormBuilder();
            formBuilder.AddParam( builder );
            return formBuilder.ToString();
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">电脑网站支付参数</param>
        public async Task<string> PayAsync( AlipayPagePayRequest request ) {
            var result = await PayAsync( request.ToParam() );
            return result.Result;
        }

        /// <summary>
        /// 跳转到支付宝收银台
        /// </summary>
        /// <param name="request">电脑网站支付参数</param>
        public async Task RedirectAsync( AlipayPagePayRequest request ) {
            var result = await PayAsync( request );
            var response = WebUtil.Response;
            response.ContentType = "text/html; charset=utf-8";
            await response.WriteAsync( result );
        }

        /// <summary>
        /// 获取请求方法
        /// </summary>
        protected override string GetMethod() {
            return "alipay.trade.page.pay";
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        protected override PayWay GetPayWay() {
            return PayWay.AlipayPagePay;
        }
    }
}