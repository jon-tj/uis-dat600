static void Run(Sorter sorter, List<int> original, List<float> output, List<float> comparisons)
{
    var copy = new List<int>(original);
    sorter.Sort(copy);
    output.Add(sorter.Milliseconds);
    comparisons.Add(sorter.Comparisons);
}

static void SaveTableCsv(Dictionary<string, List<float>> results, string filePath)
{
    using var writer = new StreamWriter(filePath);

    // Header
    writer.WriteLine("N,Insertion,Merge,Heap,Quick");

    int n = 2;
    int count = results["Insertion"].Count;

    for (int i = 0; i < count; i++)
    {
        writer.WriteLine(
            $"{n}," +
            $"{results["Insertion"][i]}," +
            $"{results["Merge"][i]}," +
            $"{results["Heap"][i]}," +
            $"{results["Quick"][i]}"
        );
        n++;
    }

    Console.WriteLine($"CSV saved to {Path.GetFullPath(filePath)}");
}

var rand = new Random();

var insertion = new InsertionSorter();
var merge = new MergeSorter();
var heap = new HeapSorter();
var quick = new QuickSorter();

var resultsTime = new Dictionary<string, List<float>>()
        {
            {"Insertion", new List<float>()},
            {"Merge", new List<float>()},
            {"Heap", new List<float>()},
            {"Quick", new List<float>()}
        };

var resultsComparisons = new Dictionary<string, List<float>>()
        {
            {"Insertion", new List<float>()},
            {"Merge", new List<float>()},
            {"Heap", new List<float>()},
            {"Quick", new List<float>()}
        };

for (int n = 2; n < 200; n++)
{
    var data = Enumerable.Range(0, n)
                         .Select(_ => rand.Next(0, 100))
                         .ToList();

    Run(insertion, data, resultsTime["Insertion"], resultsComparisons["Insertion"]);
    Run(merge, data, resultsTime["Merge"], resultsComparisons["Merge"]);
    Run(heap, data, resultsTime["Heap"], resultsComparisons["Heap"]);

    data.Sort(); // worst-case scenario when using pivot as first element

    Run(quick, data, resultsTime["Quick"], resultsComparisons["Quick"]);
}

Console.WriteLine("Done!");
SaveTableCsv(resultsTime, "sorting_microseconds.csv");
SaveTableCsv(resultsComparisons, "sorting_comparisons.csv");
Console.WriteLine("Results written to CSV files.");