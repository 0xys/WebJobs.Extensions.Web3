using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Models
{
    public class ListenerConfig
    {
        public IEnumerable<string> Endpoints { get; set; }
        public int FromHeight { get; set; }
        public int Confirmation { get; set; }
    }
}
