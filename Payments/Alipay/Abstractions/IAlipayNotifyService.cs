using Dotnet.Services.Pay.Payments.Core;

namespace Dotnet.Services.Pay.Payments.Alipay.Abstractions {
    /// <summary>
    /// 支付宝通知服务
    /// </summary>
    public interface IAlipayNotifyService : INotifyService {
        /// <summary>
        /// 买家支付宝用户号
        /// </summary>
        string BuyerId { get; }
    }
}
