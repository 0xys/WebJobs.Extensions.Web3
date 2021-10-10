using Microsoft.Azure.WebJobs.Host.Executors;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.Listener
{
    public class Web3BlockListenerClient : IWeb3ListenerClient
    {
        private readonly string _id;
        private readonly IWeb3ListenerClientConfig _config;
        private readonly ITriggeredFunctionExecutor _executor;

        public string Id => _id;
        public IWeb3ListenerClientConfig Config => _config;

        public Web3BlockListenerClient(string id, IWeb3ListenerClientConfig config, ITriggeredFunctionExecutor executor)
        {
            _id = id;
            _config = config;
            _executor = executor;
        }

        public async Task OnNewBlock(BlockWithTransactions block)
        {
            var input = new TriggeredFunctionData
            {
                TriggerValue = block
            };

            var res = await _executor.TryExecuteAsync(input, CancellationToken.None);
            if (!res.Succeeded)
            {
                // TODO: handle Function execution failure
            }
        }
    }
}
