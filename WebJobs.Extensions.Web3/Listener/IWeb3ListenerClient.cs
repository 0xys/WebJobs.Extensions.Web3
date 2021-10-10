using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.Listener
{
    public interface IWeb3ListenerClient
    {
        string Id { get; }
    }
}
