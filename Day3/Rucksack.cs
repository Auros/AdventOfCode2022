using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day3;

internal class Rucksack
{
	public IReadOnlyList<char> FirstBag { get; }

	public IReadOnlyList<char> SecondBag { get; }

	public Rucksack(string content)
	{
		var chunks = content.Chunk(content.Length / 2);
		FirstBag = chunks.First();
		SecondBag = chunks.Last();
	}
}