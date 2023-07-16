using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Fuse8_ByteMinds.SummerSchool.Domain;

namespace Fuse8_ByteMinds.SummerSchool.Benchmarks
{
    [MemoryDiagnoser(displayGenColumns: true)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class AccountProcessorBenchmark
    {
        private readonly AccountProcessor _accountProcessor = new();

        [Benchmark(Baseline = true)]
        [ArgumentsSource(nameof(CreateSampleData))]
        public decimal CalculateBenchmark(BankAccount bankAccount)
            => _accountProcessor.Calculate(bankAccount);

        [Benchmark]
        [ArgumentsSource(nameof(CreateSampleData))]
        public decimal CalculatePerformedBenchmark(ref BankAccount bankAccount)
            => _accountProcessor.CalculatePerformed(ref bankAccount);

        // Сравнение показало медианное ускорение работы метода на 90% (0.09 против 1.0). При этом использование управляемой памяти не осуществлялось.
        // Судя по dotPeek операций боксинга не производится(в методе Calculate они точно были при использовании CalculateOperation3),
        // инстансов создания BankOperation не наблюдается.
        public IEnumerable<BankAccount> CreateSampleData()
        {
            yield return new BankAccount
            {
                LastOperation = new() { OperationInfo0 = 1, OperationInfo1 = 2, OperationInfo2 = 3, TotalAmount = 4},
                PreviousOperation = new() { OperationInfo0 = 5, OperationInfo1 = 6, OperationInfo2 = 7, TotalAmount = 8 },
            };

            yield return new BankAccount
            {
                LastOperation = new() { OperationInfo0 = 1000, OperationInfo1 = 2000, OperationInfo2 = 3000, TotalAmount = 4000 },
                PreviousOperation = new() { OperationInfo0 = 5000, OperationInfo1 = 6000, OperationInfo2 = 7000, TotalAmount = 8000 },
            };
        }
    }
}
