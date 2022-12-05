using Day3;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "3.txt"));
var priorityLookup = Enumerable.Range(97, 26).Union(Enumerable.Range(65, 26)).ToList();

int GetPriority(char item) => item is char.MinValue ? 0 : (priorityLookup.IndexOf(item) + 1);

// - [ 3.1 ] -

var rucksacks = input.Select(content => new Rucksack(content));
var rucksackUniqueSum = rucksacks.Sum(r => GetPriority(r.FirstBag.Intersect(r.SecondBag).FirstOrDefault()));

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "3.1.txt"), $"""
Sum of the item priorities:
{rucksackUniqueSum}
""");

// - [ 3.2 ] -

var elfGroups = input.Chunk(3).Select(contents => new ElfGroup(contents));
var elfGroupUniqueSum = elfGroups.Sum(g => GetPriority(g.Elf1.Intersect(g.Elf2).Intersect(g.Elf3).First()));

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "3.2.txt"), $"""
Sum of the elf group priorities:
{elfGroupUniqueSum}
""");