using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using Fuse8_ByteMinds.SummerSchool.Benchmarks;

BenchmarkRunner.Run<AccountProcessorBenchmark>();
BenchmarkRunner.Run<StringInternBenchmark>();

[MemoryDiagnoser(displayGenColumns: true)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class StringInternBenchmark
{
    private readonly List<string> _words = new();
    public StringInternBenchmark()
    {
       foreach (var word in File.ReadLines(@".\SpellingDictionaries\ru_RU.dic"))
           _words.Add(string.Intern(word));
    }

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(SampleData))]
    public bool WordIsExists(string word)
        => _words.Any(item => word.Equals(item, StringComparison.Ordinal));

    [Benchmark]
    [ArgumentsSource(nameof(SampleData))]
    public bool WordIsExistsIntern(string word)
    {
        var internedWord = string.Intern(word);
        return _words.Any(item => ReferenceEquals(internedWord, item));
    }

    //в целом метод WordIsExistsIntern быстрее примерно на 20%, хотя наблюдались и более медленные результаты для слова в начале словаря (но при этом разница минимальна).
    //Разница в миллисекундах собрание словаря указанного размера (146к строк) с интернированием строк и без него составляет примерно 60-90 миллисекунд (по Stopwatch).
    //Средний выигрыш в поиске слова в интернированном словаре составил 0.5 миллисекунды (выигрыш по времени наступает при проверке в среднем более 150 слов)
    //Выигрыша памяти, судя по данным бенчмарка, нет.
    //Эти факторы нужно учитывать для выбора решения в зависимости от логики поставленной задачи, возможно использование интернированного словаря даст выгоду потом.
    public IEnumerable<string> SampleData()
    {
        yield return new StringBuilder("Чил").Append("и").ToString();
        yield return new StringBuilder("Юа").Append("нь").ToString();
        yield return new StringBuilder("абар").Append("банел").ToString();
        yield return new StringBuilder("повест").Append("ка").ToString();
        yield return "Эри";
        yield return "югославский";
        yield return ">fym";
        yield return "повывезете";
    }
}
