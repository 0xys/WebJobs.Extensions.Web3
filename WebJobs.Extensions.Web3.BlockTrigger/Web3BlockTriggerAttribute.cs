using Microsoft.Azure.WebJobs.Description;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3
{

    [AttributeUsage(AttributeTargets.Parameter)]
    [Binding]
    public class Web3BlockTriggerAttribute: Attribute
    {
        public string Endpoint { get; set; }
        public int FromHeight { get; set; }
        public int Confirmation { get; set; }

        public Web3BlockTriggerAttribute(string endpoint, int fromHeight, int confirmation)
        {
            Endpoint = endpoint;
            FromHeight = fromHeight;
            Confirmation = confirmation;
        }

        public Web3BlockTriggerAttribute(string endpoint, string fromHeight, int confirmation)
        {
            Endpoint = endpoint;
            if (int.TryParse(fromHeight, out int from))
            {
                FromHeight = from;
            }
            else
            {
                FromHeight = -1;
            }
            Confirmation = confirmation;
        }

        public Web3BlockTriggerAttribute(string endpoint) : this(endpoint, -1, 12) { }
        public Web3BlockTriggerAttribute(string endpoint, string fromHeight) : this(endpoint, fromHeight, 12) { }
        public Web3BlockTriggerAttribute(string endpoint, int confirmation) : this(endpoint, "latest", confirmation) { }
    }
}
