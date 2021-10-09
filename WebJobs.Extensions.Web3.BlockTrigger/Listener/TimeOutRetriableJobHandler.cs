using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Listener
{
    public static class TimeOutRetriableJobHandler
    {
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(15);

        public static async Task<(bool success, T res)> ExecuteWithTimeout<T>(IDelayStrategy delayStrategy, Func<Task<T>> job)
        {
            using (var cts = new CancellationTokenSource())
            {
                Task timeoutTask = Task.Delay(DefaultTimeout, cts.Token);
                Task<T> retriableJobTask = ExecuteWithDelayStrategy<T>(delayStrategy, job, cts.Token);

                Task firstCompletedTask = await Task.WhenAny(timeoutTask, retriableJobTask);

                if (Equals(firstCompletedTask, timeoutTask))
                {
                    cts.Cancel();
                    return (false, default(T));
                }

                var res = await retriableJobTask;
                cts.Cancel();
                return (true, res);
            }
        }

        private static async Task<T> ExecuteWithDelayStrategy<T>(IDelayStrategy delayStrategy, Func<Task<T>> job, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var res = await job();
                    return res;
                }
                catch
                {
                    var delay = delayStrategy.GetNextDelay(false);
                    await Task.Delay(delay);
                }
            }
            return default(T);
        }
    }
}
