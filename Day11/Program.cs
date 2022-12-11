DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

const int testOffet = 21;
const int trueLocation = 29;
const int falseLocation = 30;
const int operationLocation = 25;
const int startingItemsOffset = 18;
const int operationOperatorLocation = 23;

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "11.txt"));

Span<Monkey> monkeys = stackalloc Monkey[(input.Length + 1) / 7];
var holdingSum = CalculateHoldingSum(input, ref monkeys);
Span<long> items = stackalloc long[holdingSum];
Span<int> itemHolders = stackalloc int[holdingSum];

// - [ 11.1 ] -

ParseInput(input, ref monkeys, ref items, ref itemHolders);
var monkeyBusinessPart1 = CalculateMonkeyBusiness(20, 3, ref monkeys, ref items, ref itemHolders);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "11.1.txt"), $"""
Monkey Business:
{monkeyBusinessPart1}
""");

// - [ 11.2 ] -

monkeys = stackalloc Monkey[(input.Length + 1) / 7];
holdingSum = CalculateHoldingSum(input, ref monkeys);
items = stackalloc long[holdingSum];
itemHolders = stackalloc int[holdingSum];
ParseInput(input, ref monkeys, ref items, ref itemHolders);
var monkeyBusinessPart2 = CalculateMonkeyBusiness(10_000, 1, ref monkeys, ref items, ref itemHolders);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "11.2.txt"), $"""
Monkey Business:
{monkeyBusinessPart2}
""");

static int FastParseDoubleDigits(char tens, char ones) => FastParseSingleDigit(ones) + (FastParseSingleDigit(tens) * 10);
static int FastParseSingleDigit(char ones) => ones - '0';
static int FastParseDigits(ref ReadOnlySpan<char> inputChar)
{
    bool doubleDigit = inputChar.Length == 2;
    if (doubleDigit)
        return FastParseDoubleDigits(inputChar[0], inputChar[1]);
    return FastParseSingleDigit(inputChar[0]);
}
static int CalculateHoldingSum(string[] input, ref Span<Monkey> monkeys)
{
    int holdingSum = 0;
    int currentMonkey = 0;
    for (int i = 1; i < input.Length; i += 7)
    {
        var holding = (input[i].Length - startingItemsOffset + 2) / 4; // Calculates how many starting items there are.
        monkeys[currentMonkey++].Holding = holding;
        holdingSum += holding;
    }
    return holdingSum;
}
static void ParseInput(string[] input, ref Span<Monkey> monkeys, ref Span<long> items, ref Span<int> itemHolders)
{
    int nextItem = 0;
    for (int i = 0; i < monkeys.Length; i++)
    {
        var offset = i * 7;
        var startingItemsLine = input[++offset];
        var operationLine = input[++offset];
        var testLine = input[++offset];
        var trueLine = input[++offset];
        var falseLine = input[++offset];

        for (int c = startingItemsOffset; c < startingItemsLine.Length; c += 4)
        {
            var item = FastParseDoubleDigits(startingItemsLine[c], startingItemsLine[c + 1]);
            itemHolders[nextItem] = i;
            items[nextItem++] = item;
        }

        var op = operationLine[operationOperatorLocation];
        var opValueRaw = operationLine[operationLocation];
        var opValue = 0;
        if (opValueRaw is not 'o')
        {
            var opSpan = operationLine.AsSpan(operationLocation);
            opValue = FastParseDigits(ref opSpan);
        }
        monkeys[i].Operation = op == '*' ? opValue * -1 : opValue;

        var testSpan = testLine.AsSpan(testOffet);
        monkeys[i].Test = FastParseDigits(ref testSpan);

        monkeys[i].True = FastParseSingleDigit(trueLine[trueLocation]);
        monkeys[i].False = FastParseSingleDigit(falseLine[falseLocation]);
    }
}
static long CalculateMonkeyBusiness(int rounds, int calmingEffect, ref Span<Monkey> monkeys, ref Span<long> items, ref Span<int> itemHolders)
{
    int lcm = 1;
    for (int i = 0; i < monkeys.Length; i++)
        lcm *= monkeys[i].Test;

    for (int i = 0; i < rounds; i++)
    {
        for (int c = 0; c < monkeys.Length; c++)
        {
            var monkey = monkeys[c];
            if (monkey.Holding is 0) // Monkey isn't holding anything? Skip it's turn.
                continue;

            for (int q = 0; q < items.Length && monkey.Holding is not 0; q++)
            {
                // If the item we're looking for is not 
                if (itemHolders[q] != c)
                    continue;

                // Calculate the new worry level
                var item = items[q];

                //var lcm = Lcm(item, monkey.Test);

                item = monkey.Calculate(item) / calmingEffect % lcm;

                // Perform test
                var targetMonkeyIndex = item % monkey.Test is 0 ? monkey.True : monkey.False;
                monkeys[targetMonkeyIndex].Holding++;
                itemHolders[q] = targetMonkeyIndex;
                monkey.Holding--;
                monkey.Inspections++;
                monkeys[c] = monkey;
                items[q] = item;
            }
        }
    }

    int first = 0;
    int second = 0;

    for (int i = 0; i < monkeys.Length; i++)
    {
        var inspections = monkeys[i].Inspections;
        if (inspections > first)
        {
            second = first;
            first = inspections;
        }
        else if (inspections > second)
            second = inspections;
    }

    return (long)first * second;
}

struct Monkey
{
    public int Holding;

    public int Operation;

    public int Test;

    public int True;

    public int False;

    public int Inspections;

    public long Calculate(long old)
    {
        return Operation switch
        {
            0 => old * old,
            > 0 => old + Operation,
            _ => old * Operation * -1
        };
    }
}