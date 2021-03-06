﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dotnet.Parameters;
using Dotnet.Validations;
using Dotnet.Extensions;
using Dotnet.Services.Pay.Payments.Wechatpay.Configs;
using Dotnet.Services.Pay.Payments.Wechatpay.Signatures;
using Dotnet.Utility;
using Dotnet.Logging;
using Dotnet.Dependency;

namespace Dotnet.Services.Pay.Payments.Wechatpay.Results {
    /// <summary>
    /// 微信支付结果
    /// </summary>
    public class WechatpayResult {
        /// <summary>
        /// 配置提供器
        /// </summary>
        private readonly IWechatpayConfigProvider _configProvider;
        /// <summary>
        /// 响应结果
        /// </summary>
        private readonly ParameterBuilder _builder;
        /// <summary>
        /// 签名
        /// </summary>
        private string _sign;
        /// <summary>
        /// 微信支付原始响应
        /// </summary>
        public string Raw { get; }

        public ILogger Logger { get; set; }

        /// <summary>
        /// 初始化微信支付结果
        /// </summary>
        /// <param name="configProvider">配置提供器</param>
        /// <param name="response">xml响应消息</param>
        public WechatpayResult( IWechatpayConfigProvider configProvider, string response ) {
            configProvider.CheckNull( nameof( configProvider ) );
            _configProvider = configProvider;
            Raw = response;
            _builder = new ParameterBuilder();
            Resolve( response );
             Logger = IocManager.GetContainer().Resolve<ILoggerFactory>().Create(DotnetConsts.LoggerName);
        }

        /// <summary>
        /// 解析响应
        /// </summary>
        private void Resolve( string response ) {
            if( response.IsEmpty() )
                return;
            var elements = XmlUtil.ToElements( response );
            elements.ForEach( node => {
                if( node.Name == WechatpayConst.Sign ) {
                    _sign = node.Value;
                    return;
                }
                _builder.Add( node.Name.LocalName, node.Value );
            } );
            WriteLog();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        protected void WriteLog() {

            Logger.Error(GetType().FullName + " 微信支付返回:"
              + "请求参数:" + GetParams()
              + "原始响应: " + Raw
              );
        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="name">xml节点名称</param>
        public string GetParam( string name ) {
            return _builder.GetValue( name ).SafeString();
        }

        /// <summary>
        /// 获取返回状态码
        /// </summary>
        public string GetReturnCode() {
            return GetParam( WechatpayConst.ReturnCode );
        }

        /// <summary>
        /// 获取业务结果代码
        /// </summary>
        public string GetResultCode() {
            return GetParam( WechatpayConst.ResultCode );
        }

        /// <summary>
        /// 获取返回消息
        /// </summary>
        public string GetReturnMessage() {
            return GetParam( WechatpayConst.ReturnMessage );
        }

        /// <summary>
        /// 获取应用标识
        /// </summary>
        public string GetAppId() {
            return GetParam( WechatpayConst.AppId );
        }

        /// <summary>
        /// 获取商户号
        /// </summary>
        public string GetMerchantId() {
            return GetParam( WechatpayConst.MerchantId );
        }

        /// <summary>
        /// 获取随机字符串
        /// </summary>
        public string GetNonce() {
            return GetParam( "nonce_str" );
        }

        /// <summary>
        /// 获取预支付标识
        /// </summary>
        public string GetPrepayId() {
            return GetParam( "prepay_id" );
        }

        /// <summary>
        /// 获取交易类型
        /// </summary>
        public string GetTradeType() {
            return GetParam( WechatpayConst.TradeType );
        }

        /// <summary>
        /// 获取错误码
        /// </summary>
        public string GetErrorCode() {
            return GetParam( WechatpayConst.ErrorCode );
        }

        /// <summary>
        /// 获取错误码和描述
        /// </summary>
        public string GetErrorCodeDescription() {
            return GetParam( WechatpayConst.ErrorCodeDescription );
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        public string GetSign() {
            return _sign;
        }

        /// <summary>
        /// 获取参数列表
        /// </summary>
        public IDictionary<string, string> GetParams() {
            var builder = new ParameterBuilder( _builder );
            builder.Add( WechatpayConst.Sign, _sign );
            return builder.GetDictionary().ToDictionary( t => t.Key, t => t.Value.SafeString() );
        }

        /// <summary>
        /// 验证
        /// </summary>
        public async Task<ValidationResultCollection> ValidateAsync() {
            if( GetReturnCode() != WechatpayConst.Success || GetResultCode() != WechatpayConst.Success )
                return new ValidationResultCollection( GetErrorCodeDescription() );
            var isValid = await VerifySign();
            if( isValid == false )
                return new ValidationResultCollection( "签名失败" );
            return ValidationResultCollection.Success;
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        public async Task<bool> VerifySign() {
            var config = await _configProvider.GetConfigAsync();
            return SignManagerFactory.Create( config, _builder ).Verify( GetSign() );
        }
    }
}
