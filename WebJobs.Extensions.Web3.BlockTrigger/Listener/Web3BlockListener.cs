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
using Microsoft.Extensions.Logging;
using System.Linq;

namespace WebJobs.Extensions.Web3.BlockTrigger.Listener
{
    public class Web3BlockListener : IListener
    {
        private readonly ILogger<Web3BlockListener> _logger;
        private readonly ITriggeredFunctionExecutor _executor;
        private readonly ListenerConfig _config;
        private readonly System.Timers.Timer _timer;
        private readonly IEnumerable<IWeb3> _web3s;

        private BigInteger _lastHeight = 0;
        private BigInteger _cachedHeight = 0;

        private bool _disposed;
        private bool _running;

        public Web3BlockListener(ILoggerFactory loggerFactory, ITriggeredFunctionExecutor executor, ListenerConfig config)
        {
            _logger = loggerFactory.CreateLogger<Web3BlockListener>();
            _executor = executor;
            _config = config;
            _timer = new System.Timers.Timer(12 * 1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += OnTimer;
            _web3s = config.Endpoints.Select(x => new Nethereum.Web3.Web3(x));
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
            if (_running)
                return;
            _running = true;

            _cachedHeight = _lastHeight;

            var (success, foundHeight) = await GetCurrentBlockNumber();
            if (!success)
            {
                _logger.LogTrace($"Failed to fetch current block");
                return;
            }
            if (!IsNewBlockFound(foundHeight))
            {
                _logger.LogTrace($"No new blocks found");
                return;
            }

            if (IsFirstTime)
            {
                _lastHeight = foundHeight - _config.Confirmation;
            }

            BigInteger nextLatest = foundHeight - _config.Confirmation;
            for (var i = _lastHeight + 1; i <= nextLatest ; i++)
            {
                var response = await GetBlockAt(i);
                if (!response.success)
                {
                    _logger.LogWarning($"block number {i} failed to fetch.");
                    // TODO: handle failed rpc request
                }

                var input = new TriggeredFunctionData
                {
                    TriggerValue = response.block
                };
                
                var res = await _executor.TryExecuteAsync(input, CancellationToken.None);
                if (!res.Succeeded)
                {
                    // TODO: handle Function execution failure
                }
            }

            _lastHeight = nextLatest;
            _running = false;
        }

        private async Task<(bool success, BigInteger number)> GetCurrentBlockNumber()
        {
            foreach(var web3 in _web3s)
            {
                var delayStrategy = new DelayStrategy(TimeSpan.FromMilliseconds(100), TimeSpan.FromMinutes(1));
                var response = await TimeOutRetriableJobHandler
                    .ExecuteWithTimeout(delayStrategy,
                        () => web3.Eth.Blocks.GetBlockNumber.SendRequestAsync());
                if (response.success)
                    return (true, response.res.Value);
            }

            return (false, 0);
        }

        private async Task<(bool success, BlockWithTransactions block)> GetBlockAt(BigInteger number)
        {
            var nextHeightHex = new HexBigInteger(number);
            foreach (var web3 in _web3s)
            {
                var delayStrategy = new DelayStrategy(TimeSpan.FromMilliseconds(100), TimeSpan.FromMinutes(1));
                var response = await TimeOutRetriableJobHandler
                    .ExecuteWithTimeout(delayStrategy,
                        () => web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(nextHeightHex));
                if (response.success)
                    return (true, response.res);
            }

            return (false, null);
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
