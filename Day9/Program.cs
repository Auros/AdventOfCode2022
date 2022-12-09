DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "9.txt"));

// - [ 9.1 ] -

HashSet<(int, int)> visited = new();
(int, int) currentHeadLocation = (0, 0);
(int, int) currentTailLocation = (0, 0);
foreach (var line in input)
{
    var (stepX, stepY) = line[0] switch
    {
        'U' => (0, 1),
        'D' => (0, -1),
        'L' => (-1, 0),
        'R' => (1, 0),
        _ => throw new NotImplementedException(),
    };

    var steps = int.Parse(line[2..]);

    visited.Add(currentTailLocation);
    for (int i = 0; i < steps; i++)
    {
        var (headX, headY) = currentHeadLocation;
        var (tailX, tailY) = currentTailLocation;
        var previousHeadLocation = currentHeadLocation;
        currentHeadLocation = (headX + stepX, headY + stepY);
        (headX, headY) = currentHeadLocation;

        var diffX = headX - tailX;
        var diffY = headY - tailY;

        // If the tail is out of range of the head, move it.
        if (diffX > 1 || diffX < -1 || diffY > 1 || diffY < -1)
        {
            currentTailLocation = previousHeadLocation;
            visited.Add(currentTailLocation);
        }
    }
}

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "9.1.txt"), $"""
Unique Places Visited:
{visited.Count}
""");