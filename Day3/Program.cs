using Day3;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var rucksacks = File.ReadAllLines(Path.Combine(inputDir.FullName, "3.txt"))
    .Select(content => new Rucksack(content));

var priorityLookup = Enumerable.Range(97, 26).Union(Enumerable.Range(65, 26)).ToList();

int GetPriority(char item) => item is char.MinValue ? 0 : (priorityLookup.IndexOf(item) + 1);

// - [ 3.1 ] -

var sum = rucksacks.Sum(r => GetPriority(r.FirstBag.Intersect(r.SecondBag).FirstOrDefault()));

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "3.1.txt"), $"""
Sum of the item priorities:
{sum}
""");