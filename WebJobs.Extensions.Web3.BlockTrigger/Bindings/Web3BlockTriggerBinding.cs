using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nethereum.RPC.Eth.DTOs;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebJobs.Extensions.Web3.BlockTrigger.Models;
using WebJobs.Extensions.Web3.BlockTrigger.Listener;

namespace WebJobs.Extensions.Web3.BlockTrigger.Bindings
{
    public class Web3BlockTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly IReadOnlyDictionary<string, Type> _bindingContract;

        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _loggerFactory;

        public Web3BlockTriggerBinding(ParameterInfo parameter, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            _parameter = parameter;
            _bindingContract = CreateBindingDataContract();
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        public Type TriggerValueType => typeof(BlockWithTransactions);

        public IReadOnlyDictionary<string, Type> BindingDataContract => _bindingContract;

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var triggerValue = value as BlockWithTransactions;
            var valueProvider = new ValueProvider(triggerValue);
            var data = new TriggerData(valueProvider, CreateBindingData(triggerValue));
            return Task.FromResult<ITriggerData>(data);
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            var attr = _parameter.GetCustomAttribute<Web3BlockTriggerAttribute>(false);

            var config = new ListenerConfig
            {
                Endpoints = new string[] { Resolve(attr.Endpoint) },
                FromHeight = attr.FromHeight,
                Confirmation = attr.Confirmation
            };
            var listener = new Web3BlockListener(_loggerFactory, context.Executor, config);
            return Task.FromResult<IListener>(listener);
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new Web3BlockParameterDescriptor
            {
                Name = _parameter.Name,
                DisplayHints = new ParameterDisplayHints
                {
                    Prompt = "Web3Block",
                    Description = "Web3Block Trigger fired",
                    DefaultValue = "Web3Block"
                }
            };
        }

        private IReadOnlyDictionary<string, Type> CreateBindingDataContract()
        {
            var contract = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            contract.Add("Web3BlockTrigger", typeof(BlockWithTransactions));
            return contract;
        }

        private IReadOnlyDictionary<string, object> CreateBindingData(BlockWithTransactions value)
        {
            var bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            bindingData.Add("Web3BlockTrigger", value);
            return bindingData;
        }

        private string Resolve(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("Cannot resolve null string");

            if(name.StartsWith("%") && name.EndsWith("%"))
            {
                string key = name.Substring(1, name.Length - 2);
                return _configuration.GetValue<string>(key);
            }
            return name;
        }

        private class ValueProvider: IValueProvider
        {
            private readonly object _value;

            public ValueProvider(object value)
            {
                _value = value;
            }

            public Type Type => typeof(BlockWithTransactions);
            public Task<object> GetValueAsync()
            {
                return Task.FromResult(_value);
            }

            public string ToInvokeString()
            {
                return DateTime.Now.ToString("o");
            }
        }

        private class Web3BlockParameterDescriptor : TriggerParameterDescriptor
        {
            public override string GetTriggerReason(IDictionary<string, string> arguments)
            {
                return string.Format("Web3BlockTimer fired at {0}", DateTime.Now.ToString("o"));
            }
        }
    }
}
