using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebJobs.Extensions.Web3.BlockTrigger.Models;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Listener
{
    public class Web3BlockListener : IListener
    {
        private ITriggeredFunctionExecutor _executor;
        private ListenerConfig _config;
        private System.Timers.Timer _timer;
        private int _lastHeight = 0;

        public Web3BlockListener(ITriggeredFunctionExecutor executor, ListenerConfig attribute)
        {
            _executor = executor;
            _config = attribute;
            _timer = new System.Timers.Timer(5 * 1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += OnTimer;
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
