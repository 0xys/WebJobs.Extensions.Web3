using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using WebJobs.Extensions.Web3.BlockTrigger.Web3.Bindings;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Config
{
    public class Web3BlockExtensionConfigProvider : IExtensionConfigProvider
    {
        private readonly IConfiguration _configuration;

        public Web3BlockExtensionConfigProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var provider = new Web3BlockTriggerAttributeBindingProvider(_configuration);
            context.AddBindingRule<Web3BlockTriggerAttribute>()
                .BindToTrigger(provider);
        }
    }
}
