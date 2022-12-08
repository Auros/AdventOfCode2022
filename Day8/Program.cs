DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "8.txt"));
var width = input[0].Length;
var height = input.Length;
int endWidth = width - 1;
int endHeight = height - 1;

void ScanForVisibleTrees(int x, int y, int xIncrement, int yIncrement, int maximum, HashSet<Coordinate> collector)
{
    var maximumTree = -1;
    while (x >= 0 && y >= 0 && x < maximum && y < maximum && maximumTree is not 57 /* 57 is 9. If the tallest tree we've seen is the tallest possible, no point in looking further beyond. */)
    {
        var currentTree = input[x][y];

        if (currentTree > maximumTree)
        {
            collector.Add(new Coordinate(x, y));
            maximumTree = currentTree;
        }

        x += xIncrement;
        y += yIncrement;
    }
}

// - [ 8.1 ] -
HashSet<Coordinate> visibleCoordinates = new();

for (int i = 0; i < width; i++)
{
    ScanForVisibleTrees(0, i, 1, 0, height, visibleCoordinates);
    ScanForVisibleTrees(endWidth, i, -1, 0, height, visibleCoordinates);
}

for (int i = 0; i < height; i++)
{
    ScanForVisibleTrees(i, 0, 0, 1, width, visibleCoordinates);
    ScanForVisibleTrees(i, endHeight, 0, -1, width, visibleCoordinates);
}

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "8.1.txt"), $"""
Total number of unique visible trees:
{visibleCoordinates.Count}
""");

readonly record struct Coordinate(int X, int Y);