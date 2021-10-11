using System.Numerics;

namespace WebJobs.Extensions.Web3.Blockchain
{
    public interface IBlockchainCache<TBlock>
    {
        BigInteger CurrentHeight { get; }
        BigInteger AppendNewBlock(TBlock block);
        bool TryGetBlockAt(BigInteger height, out TBlock block);
    }
}
