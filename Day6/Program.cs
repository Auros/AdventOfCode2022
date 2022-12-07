DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllText(Path.Combine(inputDir.FullName, "6.txt"));

char[] buffer = input.Take(4).ToArray();

// - [ 6.1 ] -

int? index = null;
for (int i = 0; i < input.Length - buffer.Length; i++)
{
    if (buffer.Distinct().Count() == buffer.Length)
    {
        index = i + buffer.Length;
        break;
    }

    for (int c = 0; c < buffer.Length; c++)
        buffer[c] = input[i + c + 1];
}

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "6.1.txt"), $"""
First Marker Location:
{index}
""");