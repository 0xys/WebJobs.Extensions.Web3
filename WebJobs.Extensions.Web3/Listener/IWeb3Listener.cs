using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.Listener
{
    public interface IWeb3Listener: IDisposable
    {
        Task OnNewBlock(BlockWithTransactions block);
        Task<bool> TryStartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void Cancel();
    }
}
