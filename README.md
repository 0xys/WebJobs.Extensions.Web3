# Web3 Extension for Azure Functions
 This extension provides trigger bindings in Microsoft [Azure Functions](https://docs.microsoft.com/en-us/azure/azure-functions/functions-overview), allowing you to interact with web3-compatible blockchain event frictionlessly, powered by [Web3 Provider](https://nethereum.readthedocs.io/en/latest/introduction/web3/) of [Nethereum](https://nethereum.readthedocs.io/en/latest/) project. Currently, following trigger is actively developed.

- **BlockTrigger**: triggered by ethereum block event.

## Usage
### BlockTrigger
```cs

// Set endpoint URL inline
[FunctionName("Function1")]
public static async Task Run([Web3BlockTrigger("https://mainnet.infura.io/v3/<YOUR_PROJECT_ID>")] BlockWithTransactions block, ILogger log)
{
    log.LogInformation($"Block[{block.Number}, {block.BlockHash}], tx: {block.Transactions.Length}");
}

// Set endpoint from config file
[FunctionName("Function2")]
public static async Task Run([Web3BlockTrigger("%EthereumEndpoint%")] BlockWithTransactions block, ILogger log)
{
    log.LogInformation($"Block[{block.Number}, {block.BlockHash}], tx: {block.Transactions.Length}");
}

// Set required number of blocks to confirm. default to 12 if unset.
[FunctionName("Function3")]
public static async Task Run([Web3BlockTrigger("%EthereumEndpoint%", 30)] BlockWithTransactions block, ILogger log)
{
    log.LogInformation($"Block[{block.Number}, {block.BlockHash}], tx: {block.Transactions.Length}");
}
```
