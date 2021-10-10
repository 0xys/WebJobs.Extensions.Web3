using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.Listener
{
    public interface IWeb3ListenerClient
    {
        string Id { get; }
        Task OnNewBlock(BlockWithTransactions block);
    }
}
