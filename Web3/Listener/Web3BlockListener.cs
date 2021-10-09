using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebJobs.Extensions.Web3.BlockTrigger.Models;
using Nethereum.Web3;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Listener
{
    public class Web3BlockListener : IListener
    {
        private ITriggeredFunctionExecutor _executor;
        private ListenerConfig _config;
        private System.Timers.Timer _timer;

        private readonly IWeb3 _web3;
        private BigInteger _lastHeight = 0;
        private BigInteger _cachedHeight = 0;

        private bool _disposed;

        public Web3BlockListener(ITriggeredFunctionExecutor executor, ListenerConfig config)
        {
            _executor = executor;
            _config = config;
            _timer = new System.Timers.Timer(12 * 1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += OnTimer;

            _web3 = new Nethereum.Web3.Web3(config.Endpoint);
        }

        public void Cancel()
        {
            ThrowIfDisposed();
            _lastHeight = _cachedHeight;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _timer.Dispose();
                _disposed = true;
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            _timer.Start();
            return Task.FromResult(true);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer.Stop();
            return Task.FromResult(true);
        }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {
            _cachedHeight = _lastHeight;

            BigInteger foundHeight = (await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync()).Value;
            if (!IsNewBlockFound(foundHeight))
                return;

            if(IsFirstTime)
            {
                _lastHeight = foundHeight - _config.Confirmation;
            }

            BigInteger nextLatest = foundHeight - _config.Confirmation;
            for (var i = _lastHeight + 1; i <= nextLatest ; i++)
            {
                var nextHeightHex = new HexBigInteger(i);
                var delayStrategy = new DelayStrategy(TimeSpan.FromMilliseconds(100), TimeSpan.FromMinutes(1));
                var response = await TimeOutRetriableJobHandler
                    .ExecuteWithTimeout<BlockWithTransactions>(delayStrategy, () => _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(nextHeightHex));
                if (!response.success)
                {
                    // TODO: handle failed rpc request
                }

                var input = new TriggeredFunctionData
                {
                    TriggerValue = response.res
                };
                
                var res = await _executor.TryExecuteAsync(input, CancellationToken.None);
                if (!res.Succeeded)
                {
                    // TODO: handle Function execution failure
                }
            }

            _lastHeight = nextLatest;
        }

        private bool IsNewBlockFound(BigInteger foundHeight) => foundHeight > _lastHeight + _config.Confirmation;

        private bool IsFirstTime => _lastHeight == 0;

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
