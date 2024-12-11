var streamReader = new StreamReader(@"..\..\..\input");

var map = streamReader.ReadToEnd().Split("\r\n");
Dictionary<string, List<string>> reachablePeaksByStartingPoint = new Dictionary<string, List<string>>();

for (int i = 0; i < map.Length; i++)
{
    for (int j = 0; j < map[i].Length; j++)
    {
        if (map[i][j] != '0') continue;
        reachablePeaksByStartingPoint.Add($"{i},{j}", GetReachablePeaks(i, j, 0).ToList());
    }
}

Console.WriteLine(reachablePeaksByStartingPoint.Select(e => e.Value.Distinct().ToList().Count).Sum());
Console.WriteLine(reachablePeaksByStartingPoint.Select(e => e.Value.Count).Sum());

return;
    
List<string> GetReachablePeaks(int x, int y, int currentHeight)
{
    if (currentHeight == 9) return [$"{x},{y}"];
    List<string> result = [];
    if (x - 1 >= 0 && Char.GetNumericValue(map[x - 1][y]) == currentHeight + 1) result.AddRange(GetReachablePeaks(x-1,y,currentHeight+1));
    if (x + 1 < map.Length && Char.GetNumericValue(map[x + 1][y]) == currentHeight + 1) result.AddRange(GetReachablePeaks(x+1,y,currentHeight+1));
    if (y - 1 >= 0 && Char.GetNumericValue(map[x][y - 1]) == currentHeight + 1) result.AddRange(GetReachablePeaks(x,y-1,currentHeight+1));
    if (y + 1 < map[x].Length && Char.GetNumericValue(map[x][y + 1]) == currentHeight + 1) result.AddRange(GetReachablePeaks(x,y+1,currentHeight+1));

    return result;
}


