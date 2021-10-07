using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger
{
    public class BlockInfo
    {
        public BigInteger Height { get; set; }
        public string Hash { get; set; }
    }
}
