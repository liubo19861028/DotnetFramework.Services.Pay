using System.Threading.Tasks;
using Dotnet.Services.Pay.Payments.Alipay.Parameters.Requests;
using Dotnet.Services.Pay.Payments.Core;

namespace Dotnet.Services.Pay.Payments.Alipay.Abstractions {
    /// <summary>
    /// 支付宝条码支付服务
    /// </summary>
    public interface IAlipayBarcodePayService {
        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">条码支付参数</param>
        Task<PayResult> PayAsync( AlipayBarcodePayRequest request );
    }
}