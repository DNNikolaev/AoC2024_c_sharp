var streamReader = new StreamReader(@"..\..\..\input");

var lines = streamReader.ReadToEnd().Split("\r\n");
var visited = new List<string>();
var columnsPadding = lines[0].Length.ToString().Length;
var rowPadding = lines.Length.ToString().Length;

var sum = 0;
var sumStageTwo = 0;
for (int i = 0; i < lines.Length; i++)
{
    for (int j = 0; j < lines[i].Length; j++)
    {
        if (visited.Contains($"{i},{j}")) continue;
        var data = CalculateCost(i, j, lines[i][j]);
        sum += data.area * data.perimeter;
        data.sides.Sort();
        sumStageTwo += data.area * GetUniqueSidesCount(data.sides);
    }
}

Console.WriteLine(sum);
Console.WriteLine(sumStageTwo);
return sum;

(int area, int perimeter, List<string> sides) CalculateCost(int x, int y, char currentLabel)
{
    var result = (area: 0, perimeter: 0, sides: new List<string>());
    if (visited.Contains($"{x},{y}")) return result;
    visited.Add($"{x},{y}");
    var leftwardsResults = (area: 0, perimeter: 0, sides: new List<string>());
    if (y - 1 < 0 || lines[x][y - 1] != currentLabel)
    {
        result.perimeter += 1;
        result.sides.Add($"l{y.ToString().PadLeft(columnsPadding, '0')},{x.ToString().PadLeft(rowPadding, '0')}");
    }
    else
    {
        leftwardsResults = CalculateCost(x, y - 1, currentLabel);
    }
    var downwardsResults = (area: 0, perimeter: 0, sides: new List<string>());
    if (x + 1 >= lines.Length || lines[x + 1][y] != currentLabel)
    {
        result.perimeter += 1;
        result.sides.Add($"d{x.ToString().PadLeft(rowPadding, '0')},{y.ToString().PadLeft(columnsPadding, '0')}");
    }
    else
    {
        downwardsResults = CalculateCost(x + 1, y, currentLabel);
    }
    var rightwardsResults = (area: 0, perimeter: 0, sides: new List<string>());
    if (y + 1 >= lines[x].Length  || lines[x][y + 1] != currentLabel)
    {
        result.perimeter += 1;
        result.sides.Add($"r{y.ToString().PadLeft(columnsPadding, '0')},{x.ToString().PadLeft(rowPadding, '0')}");
    }
    else
    {
        rightwardsResults = CalculateCost(x, y + 1, currentLabel);
    }
    var upwardsResults = (area: 0, perimeter: 0, sides: new List<string>());
    if (x - 1 < 0  || lines[x - 1][y] != currentLabel)
    {
        result.perimeter += 1;
        result.sides.Add($"u{x.ToString().PadLeft(rowPadding, '0')},{y.ToString().PadLeft(columnsPadding, '0')}");
    }
    else
    {
        upwardsResults = CalculateCost(x - 1, y, currentLabel);
    }

    result.area = 1 + leftwardsResults.area + downwardsResults.area + rightwardsResults.area + upwardsResults.area;
    result.perimeter = result.perimeter + leftwardsResults.perimeter + downwardsResults.perimeter + rightwardsResults.perimeter + upwardsResults.perimeter;
    result.sides.AddRange(leftwardsResults.sides);
    result.sides.AddRange(downwardsResults.sides);
    result.sides.AddRange(rightwardsResults.sides);
    result.sides.AddRange(upwardsResults.sides);
    // Console.WriteLine($"{currentLabel}, {x}, {y}: {result}");
    return result;
}

int GetUniqueSidesCount(List<string> sides)
{
    var count = 1;
    var prevElement = sides[0];
    for (int i = 1; i < sides.Count; i++)
    {
        var prevParts = prevElement.Split(",");
        var currParts = sides[i].Split(",");
        prevElement = sides[i];
        if (currParts[0] == prevParts[0] && int.Parse(currParts[1]) - int.Parse(prevParts[1]) == 1) continue;
        count++;
    }
    
    return count;
}