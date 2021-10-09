# Web3.BlockTrigger
 Custom Trigger Binding for the use of Microsoft Azure Functions, triggered by Ethereum block event.

## Usage
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
