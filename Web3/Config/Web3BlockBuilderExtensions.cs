using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Config
{
    public static class Web3BlockBuilderExtensions
    {
        public static IWebJobsBuilder AddWeb3BlockTrigger(this IWebJobsBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.AddExtension<Web3BlockExtensionConfigProvider>();
            return builder;
        }
    }
}
