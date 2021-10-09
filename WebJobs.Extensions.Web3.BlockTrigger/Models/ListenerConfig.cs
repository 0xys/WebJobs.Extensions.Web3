using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Models
{
    public class ListenerConfig
    {
        public string Endpoint { get; set; }
        public int FromHeight { get; set; }
        public int Confirmation { get; set; }
    }
}
