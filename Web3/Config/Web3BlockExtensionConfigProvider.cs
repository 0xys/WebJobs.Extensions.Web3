using Microsoft.Azure.WebJobs.Host.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Config
{
    public class Web3BlockExtensionConfigProvider : IExtensionConfigProvider
    {
        public void Initialize(ExtensionConfigContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");


        }
    }
}
