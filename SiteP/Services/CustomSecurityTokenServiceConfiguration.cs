using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace SiteP.Services
{
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private static readonly object syncRoot = new object();
        private const string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";

        public CustomSecurityTokenServiceConfiguration()
            : base(WebConfigurationManager.AppSettings[Common.IssuerName])
        {
            this.SecurityTokenService = typeof(CustomSecurityTokenService);
        }

        public static CustomSecurityTokenServiceConfiguration Current
        {
            get
            {
                HttpApplicationState app = HttpContext.Current.Application;
                CustomSecurityTokenServiceConfiguration config = app.Get(CustomSecurityTokenServiceConfigurationKey) as CustomSecurityTokenServiceConfiguration;
                if (config != null)
                    return config;
                lock (syncRoot)
                {
                    config = app.Get(CustomSecurityTokenServiceConfigurationKey) as CustomSecurityTokenServiceConfiguration;
                    if (config == null)
                    {
                        config = new CustomSecurityTokenServiceConfiguration();
                        app.Add(CustomSecurityTokenServiceConfigurationKey, config);
                    }

                    return config;
                }
            }
        }
    }

}