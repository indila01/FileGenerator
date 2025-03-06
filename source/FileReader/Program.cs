using System.Text;

namespace FileReader;

public class Program
{
    private const string DATA_FOLDER = "data";
    private const string OUTPUT_FOLDER = "output";

    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("File Reader Starting...");
            string filePath = GetInputFile();
            ProcessFile(filePath);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }

    private static string GetInputFile()
    {
        string baseDirectory = Directory.GetCurrentDirectory();
        string dataDirectory = Path.Combine(baseDirectory, DATA_FOLDER);

        if (!Directory.Exists(dataDirectory))
        {
            throw new DirectoryNotFoundException($"Data directory not found at: {dataDirectory}");
        }

        var directory = new DirectoryInfo(dataDirectory);
        var latestFile = directory
            .GetFiles("generated_data_*.txt")
            .OrderByDescending(f => f.LastWriteTime)
            .FirstOrDefault();

        return latestFile?.FullName
            ?? throw new FileNotFoundException("No data files found in the data directory.");
    }

    private static void ProcessFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {filePath}");
        }

        string outputDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, OUTPUT_FOLDER);
        Directory.CreateDirectory(outputDirectory);

        string outputFile = Path.Combine(
            outputDirectory,
            $"analysis_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt"
        );

        using var reader = new StreamReader(filePath);
        using var writer = new StreamWriter(outputFile);

        string content = reader.ReadToEnd();
        var items = content.Split(',');

        WriteHeader(writer, filePath, items.Length);

        foreach (var item in items)
        {
            if (string.IsNullOrEmpty(item))
                continue;
            ProcessItem(writer, item.Trim());
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Analysis complete. Output saved to: {outputFile}");
        Console.ResetColor();
    }

    private static void WriteHeader(StreamWriter writer, string filePath, int itemCount)
    {
        var headerBuilder = new StringBuilder();
        headerBuilder.AppendLine($"Processing file: {Path.GetFileName(filePath)}");
        headerBuilder.AppendLine($"Total items found: {itemCount}");
        headerBuilder.AppendLine($"Analysis Date: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        headerBuilder.AppendLine("=====================================================");
        var header = headerBuilder.ToString();

        writer.WriteLine(header);
        Console.Write(header);
    }

    private static void ProcessItem(StreamWriter writer, string item)
    {
        var type = DetermineType(item);
        var output = $"Item: {item}\nType: {type}\n\n";

        writer.Write(output);
        Console.Write(output);
    }

    private static string DetermineType(string item)
    {
        if (string.IsNullOrWhiteSpace(item))
            return "Empty";

        if (int.TryParse(item, out _))
            return "Integer";

        if (double.TryParse(item, out _))
            return "Real Number";

        if (item.All(char.IsLetter))
            return "Alphabetical String";

        if (item.Any(char.IsLetter) && item.Any(char.IsDigit))
            return "Alphanumeric";

        return "Unknown";
    }
}
