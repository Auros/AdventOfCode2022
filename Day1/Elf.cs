namespace Day1;

internal class Elf
{
    public int Calories { get; private set; }

    public void AddCalories(int calories) => Calories += calories;
}