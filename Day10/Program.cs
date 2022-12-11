DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "10.txt"));

int reader = 0;
int current = 1;
Span<int> register = stackalloc int[input.Length * 2]; // Maximum possible size since each instruction can use at most two cycles. 

foreach (var line in input)
{
    if (line[0] is 'n')
    {
        // noop

        // Consume one cycle and don't do anything
        register[reader++] = current;
    }
    else
    {
        // addx
        bool negative = line[5] == '-';
        bool doubleDigit = line.Length == 8 && negative || line.Length == 7 && !negative;
        var value = line[^1] - '0';
        if (doubleDigit)
            value += (line[^2] - '0') * 10;
        
        if (negative)
            value *= -1;

        // Consume two cycles and assign the incremented register on the second.
        register[reader++] = current;
        register[reader++] = current += value;
    }
}

// - [ 10.1 ] -

int sum = 0;
for (int i = 19; i < 220; i += 40)
    sum += register[i - 1] * (i + 1); // This doesn't feel right...

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "10.1.txt"), $"""
Sum of every 40th cycle:
{sum}
""");

// - [ 10.2 ] -

int currentRow = 0;
int currentCrt = 0;
Span<char> crt = stackalloc char[247]; // :tf:
crt[currentCrt++] = register[0] == 1 || register[0] == 0 ? '#' : '.';
for (int i = 1; i < reader; i++)
{
    if (i % 40 == 0)
    {
        crt[currentCrt++] = '\n';
        currentRow += 40;
    }

    var value = register[i - 1] + currentRow;
    crt[currentCrt++] = (value == i || value + 1 == i || value - 1 == i) ? '#' : '.' ;
}

var output = new string(crt);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "10.2.txt"), output);