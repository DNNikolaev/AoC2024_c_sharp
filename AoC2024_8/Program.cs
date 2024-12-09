var streamReader = new StreamReader(@"..\..\..\input");

var lines = streamReader.ReadToEnd().Split("\r\n");

bool IsInField(int x, int y) => x >= 0 && x < lines.Length &&
    y >= 0 && y < lines[0].Length;

var antinodesCoordsStageTwo = new List<(int x, int y)>();
var coordsBySymbol = new Dictionary<char, List<(int x, int y)>>();
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        var currChar = lines[i][j];
        if (currChar == '.') continue;
        antinodesCoordsStageTwo.Add((i, j));
        if (!coordsBySymbol.ContainsKey(currChar))
        {
            var coordList = new List<(int x, int y)>();
            coordList.Add((i, j));
            coordsBySymbol.Add(currChar, coordList);
        }
        else
        {
            var coordList = coordsBySymbol[currChar];
            coordList.Add((i, j));
        }
    }
}

var antinodesCoords = new List<(int x, int y)>();

foreach(KeyValuePair<char, List<(int x, int y)>> entry in coordsBySymbol)
{
    Console.WriteLine($"{entry.Key}: {entry.Value.Count}");
    var coordsArray = entry.Value.ToArray();
    for (int i = 0; i < coordsArray.Length - 1; i++)
    {
        for (int j = i + 1; j < coordsArray.Length; j++)
        {
            var xDiff = coordsArray[j].x - coordsArray[i].x;
            var yDiff = coordsArray[j].y - coordsArray[i].y;
            var firstAntinodeCoords = (x: coordsArray[j].x + xDiff, y: coordsArray[j].y + yDiff);
            var secondAntinodeCoords = (x: coordsArray[i].x - xDiff, y: coordsArray[i].y - yDiff);
            if (IsInField(firstAntinodeCoords.x, firstAntinodeCoords.y))
            {
                antinodesCoords.Add(firstAntinodeCoords);
                antinodesCoordsStageTwo.Add(firstAntinodeCoords);
                firstAntinodeCoords = (x: firstAntinodeCoords.x + xDiff, y: firstAntinodeCoords.y + yDiff);
                while (IsInField(firstAntinodeCoords.x, firstAntinodeCoords.y))
                {
                    antinodesCoordsStageTwo.Add(firstAntinodeCoords);
                    firstAntinodeCoords = (x: firstAntinodeCoords.x + xDiff, y: firstAntinodeCoords.y + yDiff);
                }
            }

            if (IsInField(secondAntinodeCoords.x, secondAntinodeCoords.y))
            {
                antinodesCoords.Add(secondAntinodeCoords);
                antinodesCoordsStageTwo.Add(secondAntinodeCoords);
                secondAntinodeCoords = (x: secondAntinodeCoords.x - xDiff, y: secondAntinodeCoords.y - yDiff);
                while (IsInField(secondAntinodeCoords.x, secondAntinodeCoords.y))
                {
                    antinodesCoordsStageTwo.Add(secondAntinodeCoords);
                    secondAntinodeCoords = (x: secondAntinodeCoords.x - xDiff, y: secondAntinodeCoords.y - yDiff);
                }
            }
        }
    }
}

Console.WriteLine(antinodesCoords.Distinct().Count());
Console.WriteLine(antinodesCoordsStageTwo.Distinct().Count());
