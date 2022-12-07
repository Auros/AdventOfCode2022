DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllText(Path.Combine(inputDir.FullName, "6.txt"));


int? GetMarker(char[] buffer)
{
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
    return index;
}

// - [ 6.1 ] -

char[] buffer = input.Take(4).ToArray();
int? index = GetMarker(buffer);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "6.1.txt"), $"""
First Packet Marker Location:
{index}
""");

// - [ 6.2 ] -

buffer = input.Take(14).ToArray();
index = GetMarker(buffer);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "6.2.txt"), $"""
First Message Marker Location:
{index}
""");