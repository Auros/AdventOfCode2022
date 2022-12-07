DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

bool runFirst = false;
var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "5.txt"));

int maxStackWidthSize = 4;
var stacksInput = input.Where(i => i.Contains('['));

var stacksInputSeparated = stacksInput.Select(s => s.Chunk(maxStackWidthSize).ToArray()).ToArray();

int numberOfStacks = (int)Math.Ceiling((double)stacksInput.First().Length / maxStackWidthSize);

Stack<char>[] stacks = new Stack<char>[numberOfStacks];
for (int i = 0; i < stacks.Length; i++)
    stacks[i] = new Stack<char>();

var instructionsInput = input.Where(i => i.StartsWith("move")).Select(instructionInput =>
{
    var splits = instructionInput.Split(' ');
    var quantity = int.Parse(splits[1]);
    var start = stacks[int.Parse(splits[3]) - 1];
    var end = stacks[int.Parse(splits[5]) - 1];
    return (quantity, start, end);
});

for (int i = stacksInputSeparated.Length - 1; i >= 0; i--)
{
    var row = stacksInputSeparated[i];
    for (int c = 0; c < row.Length; c++)
    {
        var crate = row[c];
        var id = crate[1];
        if (id == ' ')
            continue;

        stacks[c].Push(id);
    }
}

// - [ 5.1 ] -

if (runFirst)
{
    foreach (var (quantity, start, end) in instructionsInput)
    {
        for (int i = 0; i < quantity; i++)
        {
            var crate = start.Pop();
            end.Push(crate);
        }
    }

    string firstCode = new(stacks.Select(s => s.Pop()).ToArray());

    // Export the output
    File.WriteAllText(Path.Combine(outputDir.FullName, "5.1.txt"), $"""
    Stacking Code:
    {firstCode}
    """);
    return;
}

// - [ 5.2 ] -

foreach (var (quantity, start, end) in instructionsInput)
{
    var moving = start.Take(quantity).ToArray();
    for (int i = 0; i < quantity; i++)
        _ = start.Pop();

    for (int i = moving.Length - 1; i >= 0; i--)
        end.Push(moving[i]);
}

string code = new(stacks.Select(s => s.Pop()).ToArray());

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "5.2.txt"), $"""
    Stacking Code:
    {code}
    """);