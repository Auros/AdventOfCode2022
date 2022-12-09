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

// - [ 9.2 ] -
visited = new HashSet<(int, int)>();
Span<(int, int)> rope = stackalloc (int, int)[10];

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

    for (int i = 0; i < steps; i++)
    {
        // Move the head
        var (headX, headY) = rope[0];
        var targetLocation = (headX + stepX, headY + stepY);
        var (targetX, targetY) = targetLocation;
        rope[0] = targetLocation;

        for (int c = 1; c < rope.Length; c++)
        {
            var (prevX, prevY) = rope[c - 1];
            var (knotX, knotY) = rope[c];

            var diffX = knotX - prevX;
            var diffY = knotY - prevY;

            // If the current knot is out of range of the previous knot, move it.
            if (diffX > 1 || diffX < -1 || diffY > 1 || diffY < -1)
                rope[c] = (knotX + (diffX is 0 ? 0 : (diffX > 0 ? -1 : 1)), knotY + (diffY is 0 ? 0 : (diffY > 0 ? -1 : 1)));
        }

        var tail = rope[^1];
        visited.Add(tail);
    }
}

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "9.2.txt"), $"""
Unique Places Visited:
{visited.Count}
""");