﻿using System.Threading.Tasks;

namespace Dotnet.Services.Pay.Payments.Wechatpay.Configs {
    /// <summary>
    /// 微信支付配置提供器
    /// </summary>
    public interface IWechatpayConfigProvider {
        /// <summary>
        /// 获取配置
        /// </summary>
        Task<WechatpayConfig> GetConfigAsync();
    }
}
