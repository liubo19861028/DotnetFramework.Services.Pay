using System;
using Dotnet.Parameters;
using Dotnet.Services.Pay.Payments.Wechatpay.Configs;
using Dotnet.Services.Pay.Payments.Wechatpay.Enums;
using Dotnet.Signatures;
using Dotnet.Utility;

namespace Dotnet.Services.Pay.Payments.Wechatpay.Signatures {
    /// <summary>
    /// 微信支付签名工厂
    /// </summary>
    public class SignManagerFactory {
        /// <summary>
        /// 创建签名服务
        /// </summary>
        /// <param name="config">微信支付配置</param>
        /// <param name="builder">参数生成器</param>
        public static ISignManager Create( WechatpayConfig config, ParameterBuilder builder ) {
            if( config.SignType == WechatpaySignType.Md5 )
                return new Md5SignManager( new SignKey( config.PrivateKey ), builder );
           // if( config.SignType == WechatpaySignType.HmacSha256 )
           //     return new HmacSha256SignManager( new SignKey( config.PrivateKey ), builder );
            throw new NotImplementedException( $"未实现签名算法:{EnumUtil.GetEnumDescription( config.SignType)}" );
        }
    }
}
