using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Listener
{
    public interface IDelayStrategy
    {
        TimeSpan GetNextDelay(bool executionSuccess);
    }
}
