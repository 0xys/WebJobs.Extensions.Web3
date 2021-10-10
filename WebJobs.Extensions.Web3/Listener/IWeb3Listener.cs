using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.Listener
{
    public interface IWeb3Listener: IDisposable
    {
        Task TryRegisterClient(IWeb3ListenerClient client);
        Task<bool> TryStartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void Cancel();
    }
}
