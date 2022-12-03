
using Day2;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var inputLines = File.ReadAllLines(Path.Combine(inputDir.FullName, "2.txt"));

// - [ 2.1 ] -
static RPS ConvertToRPS(string input)
{
    return input switch
    {
        "A" or "X" => RPS.Rock,
        "B" or "Y" => RPS.Paper,
        "C" or "Z" => RPS.Scissors,
        _ => throw new NotImplementedException(),
    };
}

var totalSum = inputLines.Select(static line =>
{
    var parts = line.Split(' ');
    var first = ConvertToRPS(parts[0]);
    var second = ConvertToRPS(parts[1]);
    return second switch
    {
        RPS.Rock => 1,
        RPS.Paper => 2,
        RPS.Scissors => 3,
        _ => throw new NotImplementedException(),
    } + first switch
    {
        RPS.Rock => second switch
        {
            RPS.Rock => 3,
            RPS.Paper => 6,
            RPS.Scissors => 0,
            _ => throw new NotImplementedException(),
        },
        RPS.Paper => second switch
        {
            RPS.Rock => 0,
            RPS.Paper => 3,
            RPS.Scissors => 6,
            _ => throw new NotImplementedException(),
        },
        RPS.Scissors => second switch
        {
            RPS.Rock => 6,
            RPS.Paper => 0,
            RPS.Scissors => 3,
            _ => throw new NotImplementedException(),
        },
        _ => throw new NotImplementedException(),
    };

}).Sum();

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "2.1.txt"), $"""
My total value for all the RPS games:
{totalSum}
""");