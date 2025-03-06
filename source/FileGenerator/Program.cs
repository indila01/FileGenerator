using System.Text;

namespace FileGenerator;

public class Program
{
    private static readonly Random random = new Random();
    private const long TARGET_FILE_SIZE = 10 * 1024 * 1024;
    private const string DATA_FOLDER = "data";

    public static void Main(string[] args)
    {
        try
        {
            string outputFile = GenerateUniqueFilePath();
            GenerateFile(outputFile);

            FileInfo fileInfo = new(outputFile);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"File generated successfully!");
            Console.WriteLine($"Location: {outputFile}");
            Console.WriteLine($"Size: {fileInfo.Length / 1024.0 / 1024.0:F2} MB");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
            Environment.Exit(1);
        }
    }

    /// <summary>
    /// Generates a unique file path for a new file in the data directory.
    /// </summary>
    /// <returns>
    /// A string representing the unique file path for the generated file.
    /// </returns>
    private static string GenerateUniqueFilePath()
    {
        string currentDir = Directory.GetCurrentDirectory();

        string dataDirectory = Path.Combine(currentDir, DATA_FOLDER);
        Directory.CreateDirectory(dataDirectory);

        string fileName = $"generated_data_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
        string filePath = Path.Combine(dataDirectory, fileName);

        Console.WriteLine($"Generating file at: {filePath}");
        return filePath;
    }

    private static void GenerateFile(string filePath)
    {
        using StreamWriter writer = new(filePath, false);
        long currentSize = 0;
        bool isFirstItem = true;

        while (currentSize < TARGET_FILE_SIZE)
        {
            string item = GenerateRandomItem();

            if (!isFirstItem)
            {
                writer.Write(",");
                currentSize += 1;
            }

            writer.Write(item);
            currentSize += Encoding.UTF8.GetByteCount(item);
            isFirstItem = false;
        }
    }

    private static string GenerateRandomItem()
    {
        return random.Next(4) switch
        {
            0 => GenerateAlphabeticalString(),
            1 => GenerateRealNumber(),
            2 => GenerateInteger(),
            3 => GenerateAlphanumeric(),
            _ => throw new InvalidOperationException()
        };
    }

    private static string GenerateAlphabeticalString()
    {
        int length = random.Next(5, 15);
        return new string(
            Enumerable.Range(0, length).Select(_ => (char)random.Next('a', 'z' + 1)).ToArray()
        );
    }

    private static string GenerateRealNumber()
    {
        return $"{random.NextDouble() * random.Next(-1000, 1000):F6}";
    }

    private static string GenerateInteger()
    {
        return random.Next(-1000000, 1000000).ToString();
    }

    private static string GenerateAlphanumeric()
    {
        int length = random.Next(5, 15);
        string value = new string(
            Enumerable
                .Range(0, length)
                .Select(_ =>
                    random.Next(2) == 0
                        ? (char)random.Next('a', 'z' + 1)
                        : (char)random.Next('0', '9' + 1)
                )
                .ToArray()
        );

        int spacesBefore = random.Next(0, 11);
        int spacesAfter = random.Next(0, 11);

        return new string(' ', spacesBefore) + value + new string(' ', spacesAfter);
    }
}
