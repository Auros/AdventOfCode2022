using Day2;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

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

var rounds = File.ReadAllLines(Path.Combine(inputDir.FullName, "2.txt")).Select(static line =>
{
    var parts = line.Split(' ');
    var first = ConvertToRPS(parts[0]);
    var second = ConvertToRPS(parts[1]);
    return new
    {
        First = first,
        Second = second
    };
});

static int PointValueForChoice(RPS rps)
{
    return rps switch
    {
        RPS.Rock => 1,
        RPS.Paper => 2,
        RPS.Scissors => 3,
        _ => throw new NotImplementedException(),
    };
}

static int GetMatchResultValue(RPS first, RPS second)
{
    return first == second ? 3 : (first, second) switch
    {
        (RPS.Rock, RPS.Paper) or (RPS.Paper, RPS.Scissors) or (RPS.Scissors, RPS.Rock) => 6,
        _ => 0
    };
}

// - [ 2.1 ] -

var totalSum = rounds.Select(static round =>
{
    var first = round.First;
    var second = round.Second;

    return PointValueForChoice(second) + GetMatchResultValue(first, second);
}).Sum();

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "2.1.txt"), $"""
My total value for all the RPS games:
{totalSum}
""");

// - [ 2.2 ] -

var riggedScore = rounds.Select(static round =>
{
    var first = round.First;
    var result = (Result)round.Second;

    var move = result is Result.Draw ? first : (first, result) switch
    {
        (RPS.Rock, Result.Lose) or (RPS.Paper, Result.Win) => RPS.Scissors,
        (RPS.Paper, Result.Lose) or (RPS.Scissors, Result.Win) => RPS.Rock,
        (RPS.Scissors, Result.Lose) or (RPS.Rock, Result.Win) => RPS.Paper,
        _ => throw new NotImplementedException(),
    };

    return PointValueForChoice(move) + GetMatchResultValue(first, move);
}).Sum();

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "2.2.txt"), $"""
My total value for stanged RPS games:
{riggedScore}
""");