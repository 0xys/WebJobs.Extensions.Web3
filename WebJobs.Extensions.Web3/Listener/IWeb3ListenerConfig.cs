using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WebJobs.Extensions.Web3.Listener
{
    public interface IWeb3ListenerConfig
    {
        IEnumerable<string> Endpoints { get; }
        BigInteger NetworkId { get; }
    }
}
