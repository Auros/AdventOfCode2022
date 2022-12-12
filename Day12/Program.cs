using System.Runtime.CompilerServices;

DirectoryInfo inputDir = new(Environment.GetEnvironmentVariable("AOC_INPUT_DIR")!);
DirectoryInfo outputDir = new(Environment.GetEnvironmentVariable("AOC_OUTPUT_DIR")!);

var input = File.ReadAllLines(Path.Combine(inputDir.FullName, "12.txt"));
var rowLength = input[0].Length;
var columnLength = input.Length;
var totalMountainSize = rowLength * columnLength;
Span<char> mountain = stackalloc char[totalMountainSize];

int startIndex = -1;
int endIndex = -1;

for (int i = 0; i < mountain.Length; i++)
{
    var row = i / rowLength;
    var current = input[row][i - (row * rowLength)];
    ref var mountainCell = ref mountain[i];
    if (current is 'S')
    {
        startIndex = i;
        mountainCell = 'a';
    }
    else if (current is 'E')
    {
        endIndex = i;
        mountainCell = 'z';
    }
    else
    {
        mountainCell = current;
    }
}

// - [ 12.1 ] -

var minimumPathFromStart = GetMinimumPathValue(startIndex, endIndex, rowLength, columnLength, ref mountain);

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "12.1.txt"), $"""
Minimum Steps To Goal:
{minimumPathFromStart}
""");

// - [ 12.2 ] -

int lowestScore = int.MaxValue;
for (int i = 0; i < mountain.Length; i++)
{
    ref var cell = ref mountain[i];
    if (cell is not 'b')
        continue;

    var score = GetMinimumPathValue(i, endIndex, rowLength, columnLength, ref mountain);
    if (lowestScore > score)
        lowestScore = score;
}
lowestScore++; // Since we base it off the second lowest elevation, we need to add one since it's a guarantee that the path we found is connected to the lowest.

// Export the output
File.WriteAllText(Path.Combine(outputDir.FullName, "12.2.txt"), $"""
Minimum Steps To Goal:
{lowestScore}
""");

static int GetMinimumPathValue(int startIndex, int endIndex, int rowLength, int columnLength, ref Span<char> mountain)
{
    int end = -1;
    Span<int> layer = stackalloc int[rowLength * 4 - 4]; // Maximum layer search depth will be the perimeter of the grid? Probably?
    Span<int> layerBuffer = stackalloc int[layer.Length];
    Span<int> mountainPathValues = stackalloc int[mountain.Length];
    mountainPathValues[startIndex] = -1;
    layer[0] = startIndex;
    int layerSize = 1;
    int depth = 1;

    while (end is -1)
    {
        int bufferPos = 0;
        for (int i = 0; i < layerSize; i++)
        {
            ref var layerElementIndex = ref layer[i];
            ref var layerElement = ref mountain[layerElementIndex];
            var maxIncline = layerElement + 1;

            var currentElementRow = layerElementIndex / rowLength;
            var currentElementCol = layerElementIndex % rowLength;

            // Look left:
            if (currentElementCol is not 0)
            {
                var elementIndex = layerElementIndex - 1;
                ref var element = ref mountain[elementIndex];
                if (maxIncline >= element) // Is this element at a valid depth?
                {
                    ref var elementPathValue = ref mountainPathValues[elementIndex];
                    if (elementPathValue is 0) // Ensure this path has not been visited.
                    {
                        elementPathValue = depth;
                        layerBuffer[bufferPos++] = elementIndex;

                        if (elementIndex == endIndex)
                            end = elementIndex;
                    }
                }
            }

            // Look right:
            if (currentElementCol != rowLength - 1)
            {
                var elementIndex = layerElementIndex + 1;
                ref var element = ref mountain[elementIndex];
                if (maxIncline >= element) // Is this element at a valid depth?
                {
                    ref var elementPathValue = ref mountainPathValues[elementIndex];
                    if (elementPathValue is 0) // Ensure this path has not been visited.
                    {
                        elementPathValue = depth;
                        layerBuffer[bufferPos++] = elementIndex;

                        if (elementIndex == endIndex)
                            end = elementIndex;
                    }
                }
            }

            // Look up:
            if (currentElementRow is not 0)
            {
                var elementIndex = layerElementIndex - rowLength;
                ref var element = ref mountain[elementIndex];
                if (maxIncline >= element) // Is this element at a valid depth?
                {
                    ref var elementPathValue = ref mountainPathValues[elementIndex];
                    if (elementPathValue is 0) // Ensure this path has not been visited.
                    {
                        elementPathValue = depth;
                        layerBuffer[bufferPos++] = elementIndex;

                        if (elementIndex == endIndex)
                            end = elementIndex;
                    }
                }
            }

            // Look down:
            if (currentElementRow != columnLength - 1)
            {
                var elementIndex = layerElementIndex + rowLength;
                ref var element = ref mountain[elementIndex];
                if (maxIncline >= element) // Is this element at a valid depth?
                {
                    ref var elementPathValue = ref mountainPathValues[elementIndex];
                    if (elementPathValue is 0) // Ensure this path has not been visited.
                    {
                        elementPathValue = depth;
                        layerBuffer[bufferPos++] = elementIndex;

                        if (elementIndex == endIndex)
                            end = elementIndex;
                    }
                }
            }
        }
        depth++;
        layerSize = bufferPos;
        for (int i = 0; i < bufferPos; i++)
        {
            ref var localLayer = ref layer[i];
            localLayer = layerBuffer[i];
        }
    }

    return mountainPathValues[end];
}