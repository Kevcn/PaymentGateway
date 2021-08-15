using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PaymentGateway.Services;

namespace Benchmarks
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class CardServiceBenchmarkets
    {
        private const string cardNumber = "1111222233334444";
        private static readonly CardService CardService = new CardService();

        [Benchmark]
        public void MaskByRemoveInsert()
        {
            CardService.MaskCardNumber(cardNumber);
        }
        
        [Benchmark]
        public void MaskByReplace()
        {
            CardService.MaskByReplace(cardNumber);
        }
    }
}