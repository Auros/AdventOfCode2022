namespace Day3;

internal class ElfGroup
{
    public IReadOnlyList<char> Elf1 { get; }
    public IReadOnlyList<char> Elf2 { get; }
    public IReadOnlyList<char> Elf3 { get; }

    public ElfGroup(IEnumerable<string> elfContent)
    {
        var elves = elfContent.ToArray();
        Elf1 = elves[0].ToList();
        Elf2 = elves[1].ToList();
        Elf3 = elves[2].ToList();
    }
}