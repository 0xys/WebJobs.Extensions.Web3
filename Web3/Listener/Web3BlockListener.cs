﻿using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WebJobs.Extensions.Web3.BlockTrigger.Models;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Listener
{
    public class Web3BlockListener : IListener
    {
        private ITriggeredFunctionExecutor _executor;
        private ListenerConfig _config;
        private System.Timers.Timer _timer;
        private int _lastHeight = 0;

        private int _cachedHeight = 0;

        public Web3BlockListener(ITriggeredFunctionExecutor executor, ListenerConfig config)
        {
            _executor = executor;
            _config = config;
            _timer = new System.Timers.Timer(5 * 1000)
            {
                AutoReset = true
            };
            _timer.Elapsed += OnTimer;
        }

        public void Cancel()
        {
            _lastHeight = _cachedHeight;
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
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
        }
    }
}
