using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using System;
using WebJobs.Extensions.Web3.BlockTrigger;
using WebJobs.Extensions.Web3.BlockTrigger.Config;

[assembly: WebJobsStartup(typeof(Web3BlockTriggerStartup), "Timers, Files and Warmup")]
namespace WebJobs.Extensions.Web3.BlockTrigger
{
    public class Web3BlockTriggerStartup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.AddWeb3BlockTrigger();
        }
    }
}
