using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Configuration;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;

namespace SiteP.Services
{
    public class CustomSecurityTokenService : SecurityTokenService
    {
        private readonly SigningCredentials signingCreds;
        private readonly EncryptingCredentials encryptingCreds;

        public CustomSecurityTokenService(SecurityTokenServiceConfiguration config)
            : base(config)
        {
            this.signingCreds = new X509SigningCredentials(
                CertificateUtil.GetCertificate(StoreName.Root, StoreLocation.LocalMachine, WebConfigurationManager.AppSettings[Common.SigningCertificateName]));

            if (!string.IsNullOrWhiteSpace(WebConfigurationManager.AppSettings[Common.EncryptingCertificateName]))
            {
                this.encryptingCreds = new X509EncryptingCredentials(
                    CertificateUtil.GetCertificate(StoreName.Root, StoreLocation.LocalMachine, WebConfigurationManager.AppSettings[Common.EncryptingCertificateName]));
            }
        }

        /// <summary>
        /// 此方法返回要发布的令牌内容。内容由一组ClaimsIdentity实例来表示，每一个实例对应了一个要发布的令牌。当前Windows Identity Foundation只支持单个令牌发布，因此返回的集合必须总是只包含单个实例。
        /// </summary>
        /// <param name="principal">调用方的principal</param>
        /// <param name="request">进入的 RST,我们这里不用它</param>
        /// <param name="scope">由之前通过GetScope方法返回的范围</param>
        /// <returns></returns>
        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            //返回一个默认声明集，里面了包含自己想要的声明
            //这里你可以通过ClaimsPrincipal来验证用户，并通过它来返回正确的声明。
            string identityName = principal.Identity.Name;
            string[] temp = identityName.Split('|');
            ClaimsIdentity outgoingIdentity = new ClaimsIdentity();
            outgoingIdentity.AddClaim(new Claim(ClaimTypes.Email, temp[0]));
            outgoingIdentity.AddClaim(new Claim(ClaimTypes.DateOfBirth, temp[1]));
            outgoingIdentity.AddClaim(new Claim(ClaimTypes.Name, temp[2]));
            SingleSignOnManager.RegisterRP(scope.AppliesToAddress);
            return outgoingIdentity;
        }

        /// <summary>
        /// 此方法返回用于令牌发布请求的配置。配置由Scope类表示。在这里，我们只发布令牌到一个由encryptingCreds字段表示的RP标识        /// </summary>
        /// <param name="principal"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            // 使用request的AppliesTo属性和RP标识来创建Scope
            Scope scope = new Scope(request.AppliesTo.Uri.AbsoluteUri, this.signingCreds);

            if (Uri.IsWellFormedUriString(request.ReplyTo, UriKind.Absolute))
            {
                if (request.AppliesTo.Uri.Host != new Uri(request.ReplyTo).Host)
                    scope.ReplyToAddress = request.AppliesTo.Uri.AbsoluteUri;
                else
                    scope.ReplyToAddress = request.ReplyTo;
            }
            else
            {
                Uri resultUri = null;
                if (Uri.TryCreate(request.AppliesTo.Uri, request.ReplyTo, out resultUri))
                    scope.ReplyToAddress = resultUri.AbsoluteUri;
                else
                    scope.ReplyToAddress = request.AppliesTo.Uri.ToString();
            }
            if (this.encryptingCreds != null)
            {
                // 如果STS对应多个RP，要选择证书指定到请求令牌的RP，然后再用 encryptingCreds 
                scope.EncryptingCredentials = this.encryptingCreds;
            }
            else
                scope.TokenEncryptionRequired = false;
            return scope;
        }
    }

}