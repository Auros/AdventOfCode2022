DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "4.txt"));

static SectionWorkPair GetSectionWorkPairs(string line)
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

    var larger = firstSize > secondSize ? firstSectionRange : secondSectionRange;
    var smaller = larger == secondSectionRange ? firstSectionRange : secondSectionRange;

    return new SectionWorkPair(larger, smaller);
}

// - [ 4.1 ] -

var overlappingPairCount = input.Count(line =>
{
    var (larger, smaller) = GetSectionWorkPairs(line);
    return smaller.All(larger.Contains);
});

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "4.1.txt"), $"""
Overlapping Pair Count:
{overlappingPairCount}
""");

// - [ 4.2 ] -

var overlappingPairRangeCount = input.Count(line =>
{
    var (larger, smaller) = GetSectionWorkPairs(line);
    return smaller.Any(larger.Contains);
});

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "4.2.txt"), $"""
Overlapping Pair Range Count:
{overlappingPairRangeCount}
""");

readonly record struct SectionWorkPair(IEnumerable<int> Larger, IEnumerable<int> Smaller);