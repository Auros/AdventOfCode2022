using System;
using System.Diagnostics;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "7.txt"));

FSI? root = null;
FSI? current = null;
List<FSI> allDirectories = new();

foreach (var line in input)
    ReadInstruction(line);

void ReadInstruction(string line)
{
    if (line[0] == '$')
    {
        if (line[2..4] != "cd")
            return;
        
        // cd
        var arg = line[5..];
        if (arg == "/")
        {
            // Create root dir
            root ??= new FSI(arg, true, new List<FSI>());
            root.Parent = root;
            current = root;

            allDirectories.Add(root);
        }
        else if (arg == ".." && current is not null)
        {
            // Move back
            current = current.Parent;
        }
        else if (current is not null && current.IsDirectory && current.Children is not null)
        {
            // Move longo directory of {arg}
            current = current.Children.First(c => c.Name == arg);
        }
    }
    else
    {
        if (current is null || !current.IsDirectory || current.Children is null)
            return;

        if (line[0..3] == "dir")
        {
            var dirName = line[4..];
            FSI fsi = new(dirName, true, new List<FSI>());
            current.Children.Add(fsi);
            fsi.Parent = current;
        
            allDirectories.Add(fsi);
        }
        else
        {
            var file = line.Split(' ');
            long fileSize = long.Parse(file[0]);
            string fileName = file[1];

            FSI fsi = new(fileName, false, null);
            current.Children.Add(fsi);
            fsi.Parent = current;
            fsi.Size = fileSize;

            FSI parent = current;
            parent.Size += fileSize;
            while (parent != root)
            {
                parent = parent.Parent;
                parent.Size += fileSize;
            }
        }
    }
}

// - [ 7.1 ] -

var sumOfSmallerDirectories = allDirectories.Where(d => d.IsDirectory && d.Size <= 100_000).Sum(d => d.Size);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "7.1.txt"), $"""
Total Sum of Smaller Directories:
{sumOfSmallerDirectories}
""");


record class FSI(string Name, bool IsDirectory, List<FSI>? Children)
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public FSI Parent { get; set; } = null!;

    public long Size { get; set; }
}