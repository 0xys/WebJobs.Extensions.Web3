using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WebJobs.Extensions.Web3.BlockTrigger.Bindings;

namespace WebJobs.Extensions.Web3.BlockTrigger.Config
{
    public class Web3BlockExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public Web3BlockExtensionConfigProvider(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var provider = new Web3BlockTriggerAttributeBindingProvider(_configuration, _loggerFactory);
            context.AddBindingRule<Web3BlockTriggerAttribute>()
                .BindToTrigger(provider);
        }
    }
}
