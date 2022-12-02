using Day1;

bool useLINQ = true;
DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

// - [ 1.1 ] -

List<Elf> elves = new();
var inputLines = File.ReadAllLines(Path.Combine(inputDir.FullName, "1.txt"));

Elf? currentElf = null;
foreach (var line in inputLines)
{
    // Once we reach the end of a grouping of calories,
    // unassign the current elf. The ending is represented
    // as an empty line.
    if (line == string.Empty)
    {
        currentElf = null;
        continue;
    }

    // If we have a calorie value (this line isn't empty) and there isn't
    // an elf assign, we assign it and add it to the collection of elves.
    if (currentElf is null)
    {
        currentElf = new Elf();
        elves.Add(currentElf);
    }

    // Parse the calories as an integer.
    int calories = int.Parse(line);

    // Add the calories to the current elf.
    currentElf.AddCalories(calories);
}

// Find the elf with the most calories
Elf? mostCaloricElf = null;
if (useLINQ)
{
    mostCaloricElf = elves.OrderByDescending(elf => elf.Calories).FirstOrDefault();
}
else
{
    for (int i = 0; i < elves.Count; i++)
    {
        var elf = elves[i];
        if (mostCaloricElf is null || elf.Calories > mostCaloricElf.Calories)
            mostCaloricElf = elf;
    }
}

// Or, with LINQ

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "1.1.txt"), $"""
The amount of calories the elf carrying the most calories:
{mostCaloricElf?.Calories ?? 0}
""");

// - [ 1.2 ] -

const int topElvesCount = 3;
int topCalorieCount = default;

if (useLINQ)
{
    topCalorieCount = elves.OrderByDescending(elf => elf.Calories).Take(topElvesCount).Sum(elf => elf.Calories);
}
else
{
    // Sort the elves from most amount of calories to least.
    elves.Sort((a, b) => b.Calories - a.Calories);

    // Add the top X elves calorie counts. 
    for (int i = 0; i < topElvesCount || i >= elves.Count; i++)
        topCalorieCount += elves[i].Calories;
}

// or, with LINQ

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "1.2.txt"), $"""
The amount of calories the top {topElvesCount} elves combined are carrying is:
{topCalorieCount}
""");