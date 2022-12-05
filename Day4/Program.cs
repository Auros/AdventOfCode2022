DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "4.txt"));

// - [ 4.1 ] -

var overlappingPairCount = input.Count(line =>
{
    var rawPairs = line.Split(',');
    var rawFirst = rawPairs[0].Split('-');
    var rawSecond = rawPairs[1].Split('-');

    var firstStart = int.Parse(rawFirst[0]);
    var secondStart = int.Parse(rawSecond[0]);
    var firstEnd = int.Parse(rawFirst[1]);
    var secondEnd = int.Parse(rawSecond[1]);

    var firstSize = firstEnd - firstStart + 1;
    var secondSize = secondEnd - secondStart + 1;

    var firstSectionRange = Enumerable.Range(firstStart, firstSize);
    var secondSectionRange = Enumerable.Range(secondStart, secondSize);

    // Order matters here. When we go to check the contents, we can safety use LINQ
    // without having to run it in both directions by sorting based on which one is larger.
    var larger = firstSize > secondSize ? firstSectionRange : secondSectionRange;
    var smaller = larger == secondSectionRange ? firstSectionRange : secondSectionRange;

    return smaller.All(larger.Contains);
});

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "4.1.txt"), $"""
Overlapping Pair Count:
{overlappingPairCount}
""");