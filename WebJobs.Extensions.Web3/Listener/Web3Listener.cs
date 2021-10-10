using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WebJobs.Extensions.Web3.Listener
{
    public class Web3Listener : IWeb3Listener
    {
        private readonly ILogger<Web3Listener> _logger;
        private readonly IEnumerable<IWeb3ListenerClient> _clientRegistry;
        private readonly IEnumerable<BlockWithTransactions> _cache;
        private readonly System.Timers.Timer _timer;
        private readonly IWeb3ListenerConfig _config;

        private bool _disposed;
        private bool _started;

        private const int DefaultTimeInterval = 5000;

        public Web3Listener(ILoggerFactory loggerFactory, IWeb3ListenerConfig config)
        {
            _logger = loggerFactory.CreateLogger<Web3Listener>();
            _config = config;
            _timer = new System.Timers.Timer(DefaultTimeInterval)
            {
                AutoReset = true
            };
            _timer.Elapsed += OnTimer;
        }

        public Task TryRegisterClient(IWeb3ListenerClient client)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TryStartAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();

            if (_started)
                return Task.FromResult(false);

            _started = true;
            _timer.Start();
            
            return Task.FromResult(true);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            _started = false;
            return Task.FromResult(true);
        }
        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _timer.Dispose();
                _disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {

        }
    }
}
