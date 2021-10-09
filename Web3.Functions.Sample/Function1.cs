using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using WebJobs.Extensions.Web3.BlockTrigger;

namespace Web3.Functions.Sample
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([Web3BlockTrigger("%EthereumEndpoint%")] BlockWithTransactions block, ILogger log)
        {
            log.LogInformation($"Block[{block.Number}, {block.BlockHash}], tx: {block.Transactions.Length}");
        }
    }
}
