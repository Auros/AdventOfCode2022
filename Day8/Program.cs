DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "8.txt"));
var width = input[0].Length;
var height = input.Length;
int endWidth = width - 1;
int endHeight = height - 1;

void ScanForVisibleTrees(int x, int y, int xIncrement, int yIncrement, int limit, HashSet<Coordinate> collector)
{
    var maximumTree = -1;
    while (x >= 0 && y >= 0 && x < limit && y < limit && maximumTree is not 57 /* 57 is 9, the tallest tree possible. No point in looking further beyond. */)
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

int ScanUntilViewBlocked(int x, int y, int xIncrement, int yIncrement, int limit, int maximum)
{
    x += xIncrement;
    y += yIncrement;
    int viewDistance = 0;

    while (x >= 0 && y >= 0 && x < limit && y < limit)
    {
        var currentTree = input![x][y];

        viewDistance++;
        if (currentTree >= maximum)
            break;    

        x += xIncrement;
        y += yIncrement;
    }
    return viewDistance;
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

// - [ 8.2 ] -
int bestScore = 0;

int CalculateScenicScore(int x, int y)
{
    var value = input[x][y];
    int score = ScanUntilViewBlocked(x, y, 1, 0, height, value);
    score *= ScanUntilViewBlocked(x, y, -1, 0, height, value);
    score *= ScanUntilViewBlocked(x, y, 0, 1, width, value);
    score *= ScanUntilViewBlocked(x, y, 0, -1, width, value);
    return score;
}

for (int i = 0; i < width; i++)
{
    for (int c = 0; c < height; c++)
    {
        if (i is 3 && c is 2)
            _ = true;

        var score = CalculateScenicScore(i, c);
        if (score > bestScore)
            bestScore = score;
    }
}

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "8.2.txt"), $"""
Best Scenic Score Available:
{bestScore}
""");


readonly record struct Coordinate(int X, int Y);