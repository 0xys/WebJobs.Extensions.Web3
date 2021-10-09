using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebJobs.Extensions.Web3.BlockTrigger.Listener
{
    public static class ParallelJobHandler
    {
        public static async Task<T> RunParallelAndAwaitFirstCompletion<T>(IEnumerable<Task<T>> tasks)
        {
            using(var cts = new CancellationTokenSource())
            {
                var tasksWithCts = tasks.Select(x => Task<T>.Factory.StartNew(() => x.Result, cts.Token));
                var firstCompletedTask = await Task.WhenAny(tasksWithCts);
                var res = await firstCompletedTask;
                cts.Cancel();
                return res;
            }
        }
    }
}
