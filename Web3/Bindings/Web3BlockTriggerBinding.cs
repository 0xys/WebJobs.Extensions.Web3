using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Listeners;
using Microsoft.Azure.WebJobs.Host.Protocols;
using Microsoft.Azure.WebJobs.Host.Triggers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebJobs.Extensions.Web3.BlockTrigger.Models;
using WebJobs.Extensions.Web3.BlockTrigger.Web3.Listener;

namespace WebJobs.Extensions.Web3.BlockTrigger.Web3.Bindings
{
    public class Web3BlockTriggerBinding : ITriggerBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly IReadOnlyDictionary<string, Type> _bindingContract;

        public Web3BlockTriggerBinding(ParameterInfo parameter)
        {
            _parameter = parameter;
            _bindingContract = CreateBindingDataContract();
        }

        public Type TriggerValueType => typeof(BlockInfo);

        public IReadOnlyDictionary<string, Type> BindingDataContract => _bindingContract;

        public Task<ITriggerData> BindAsync(object value, ValueBindingContext context)
        {
            var triggerValue = value as BlockInfo;
            var valueProvider = new ValueProvider(triggerValue);
            var data = new TriggerData(valueProvider, CreateBindingData(triggerValue));
            return Task.FromResult<ITriggerData>(data);
        }

        public Task<IListener> CreateListenerAsync(ListenerFactoryContext context)
        {
            var attr = _parameter.GetCustomAttribute<Web3BlockTriggerAttribute>(false);

            var config = new ListenerConfig
            {
                Endpoint = attr.Endpoint,
                FromHeight = attr.FromHeight,
                Confirmation = attr.Confirmation
            };
            var listener = new Web3BlockListener(context.Executor, config);
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
            contract.Add("Web3BlockTrigger", typeof(BlockInfo));
            return contract;
        }

        private IReadOnlyDictionary<string, object> CreateBindingData(BlockInfo value)
        {
            var bindingData = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            bindingData.Add("Web3BlockTrigger", value);
            return bindingData;
        }

        private class ValueProvider: IValueProvider
        {
            private readonly object _value;

            public ValueProvider(object value)
            {
                _value = value;
            }

            public Type Type => typeof(BlockInfo);
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
