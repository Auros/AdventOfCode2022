using Day1;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

// - [ 1.1 ] -

List<Elf> elfs = new();
var inputLines = File.ReadAllLines(Path.Combine(inputDir.FullName, "1.1.txt"));

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
    // an elf assign, we assign it and add it to the collection of elfs.
    if (currentElf is null)
    {
        currentElf = new Elf();
        elfs.Add(currentElf);
    }

    // Parse the calories as an integer.
    int calories = int.Parse(line);

    // Add the calories to the current elf.
    currentElf.AddCalories(calories);
}

// Find the elf with the most calories
Elf? mostCalorieElf = null;
for (int i = 0; i < elfs.Count; i++)
{
    var elf = elfs[i];
    if (mostCalorieElf is null || elf.Calories > mostCalorieElf.Calories)
        mostCalorieElf = elf;
}

// Finally, export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "1.1.txt"),$"""
The amount of calories the elf carrying the most calories:
{mostCalorieElf?.Calories ?? 0}
""");