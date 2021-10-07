using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.Configuration;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Bindings
{
    public class Web3BlockTriggerAttributeBindingProvider : ITriggerBindingProvider
    {
        private readonly IConfiguration _configuration;

        public Web3BlockTriggerAttributeBindingProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<ITriggerBinding> TryCreateAsync(TriggerBindingProviderContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var attribute = context.Parameter.GetCustomAttribute<Web3BlockTriggerAttribute>(false);
            if (attribute == null)
                return Task.FromResult<ITriggerBinding>(null);

            if(context.Parameter.ParameterType != typeof(BlockWithTransactions))
                throw new InvalidOperationException(string.Format("Can't bind {0} to type '{1}'",
                    nameof(Web3BlockTriggerAttribute), context.Parameter.ParameterType));

            var binding = new Web3BlockTriggerBinding(context.Parameter, _configuration);
            return Task.FromResult<ITriggerBinding>(binding);
        }
    }
}
