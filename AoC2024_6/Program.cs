var streamReader = new StreamReader(@"..\..\..\input");

var fullText = streamReader.ReadToEnd();
var withoutWrap = fullText.Replace("\r\n", "");
var startingSymbol = withoutWrap.ToList().Distinct().Where(c => c != '.' && c != '#').First();
var lines = fullText.Split("\r\n");
int startingRow, currentRow, startingColumn, currentColumn;
startingRow = currentRow = withoutWrap.IndexOf(startingSymbol) / lines.Length;
startingColumn = currentColumn = withoutWrap.IndexOf(startingSymbol) % lines.Length;

var nextMoveDiffs = new int[4,2] {
    {-1,0},
    {0, 1},
    {1, 0},
    {0, -1}
};

int directionIteration = 0;
switch (startingSymbol)
{
    case '^':
        directionIteration = 0;
        break;
    case '>':
        directionIteration = 1;
        break;
    case 'v':
        directionIteration = 2;
        break;
    case '<':
        directionIteration = 3;
        break;
}

var visitedCoords = new List<string>();
while (currentRow < lines.Length && currentRow >= 0 && currentColumn < lines[0].Length && currentColumn >= 0)
{
    
    visitedCoords.Add(currentRow + "," + currentColumn);
    var nextRow = currentRow + nextMoveDiffs[directionIteration % 4, 0];
    var nextColumn = currentColumn + nextMoveDiffs[directionIteration % 4, 1];
    if (nextRow < lines.Length && nextRow >= 0 && nextColumn < lines[0].Length && nextColumn >= 0 && lines[nextRow][nextColumn] == '#')
    {
        directionIteration++;
    }
    currentRow += nextMoveDiffs[directionIteration % 4, 0];
    currentColumn += nextMoveDiffs[directionIteration % 4, 1];
}

var distinctVisitedCoords = visitedCoords.Distinct();
Console.WriteLine(distinctVisitedCoords.Count());

var distinctVisitedCoordsWithoutStarting = distinctVisitedCoords.Skip(1);
var loops = 0;

foreach (var distinctCoords in distinctVisitedCoordsWithoutStarting)
{
    switch (startingSymbol)
    {
        case '^':
            directionIteration = 0;
            break;
        case '>':
            directionIteration = 1;
            break;
        case 'v':
            directionIteration = 2;
            break;
        case '<':
            directionIteration = 3;
            break;
    }
    currentRow = startingRow;
    currentColumn = startingColumn;
    string[] newLines = new string[lines.Length];
    lines.CopyTo(newLines, 0);
    var coords = distinctCoords.Split(',').Select(int.Parse).ToArray();
    var line = newLines[coords[0]].ToCharArray();
    line[coords[1]] = 'O';
    newLines[coords[0]] = string.Join("",line);
    var visitedCoordsHash = new HashSet<string>();
    var loop = false;
    
    while (currentRow < newLines.Length && currentRow >= 0 && currentColumn < newLines[0].Length && currentColumn >= 0)
    {
        
        if (!visitedCoordsHash.Add($"{currentRow},{currentColumn},{directionIteration % 4}"))
        {
            loops++;
            break;
        }
        
        var nextRow = currentRow + nextMoveDiffs[directionIteration % 4, 0];
        var nextColumn = currentColumn + nextMoveDiffs[directionIteration % 4, 1];
        while (nextRow < newLines.Length && nextRow >= 0 &&
            nextColumn < newLines[0].Length && nextColumn >= 0 && 
            (newLines[nextRow][nextColumn] == '#' || newLines[nextRow][nextColumn] == 'O'))
        {
            directionIteration++;
            nextRow = currentRow + nextMoveDiffs[directionIteration % 4, 0];
            nextColumn = currentColumn + nextMoveDiffs[directionIteration % 4, 1];

        }
        currentRow += nextMoveDiffs[directionIteration % 4, 0];
        currentColumn += nextMoveDiffs[directionIteration % 4, 1];
    }
}

Console.WriteLine(loops);