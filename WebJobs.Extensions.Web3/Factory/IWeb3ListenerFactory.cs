using System;
using System.Collections.Generic;
using System.Text;
using WebJobs.Extensions.Web3.Listener;

namespace WebJobs.Extensions.Web3.Factory
{
    public interface IWeb3ListenerFactory
    {
        IWeb3Listener GetOrCreateInstance(IWeb3ListenerConfig config);
    }
}
