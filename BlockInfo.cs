using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger
{
    public class BlockInfo
    {
        public int Height { get; set; }
        public string Hash { get; set; }
    }
}
