using System.Text.RegularExpressions;

var streamReader = new StreamReader(@"..\..\..\input");

var robotData = streamReader.ReadToEnd().Split("\r\n");
var parserRegexp = new Regex(@"p=(.*) v=(.*)");
var suspicionRegexp = new Regex(@"\.(1+)\.");

var wide = 101;
var wideCenter = wide / 2;
var tall = 103;
var tallCenter = tall / 2;
var seconds = 100;
var leftTopCount = 0;
var leftBottomCount = 0;
var rightTopCount = 0;
var rightBottomCount = 0;
var pictureStrings = new string[tall];
for (int i = 0; i < tall; i++)
{
    pictureStrings[i] = new string('.', wide);
}

foreach (var dataString in robotData)
{
    var parsedString = parserRegexp.Match(dataString).Groups;
    // Console.WriteLine($"{parsedString[1]};{parsedString[2]}");
    var positions = parsedString[1].ToString().Split(',').Select(int.Parse).ToArray();
    var filledPosition = pictureStrings[positions[1]].ToCharArray();
    filledPosition[positions[0]] = '1';
    pictureStrings[positions[1]] = string.Join("", filledPosition);
    var velocities = parsedString[2].ToString().Split(',').Select(int.Parse).ToArray();
    var endX = (wide + (positions[0] + velocities[0] * seconds) % wide) % wide;
    var endY = (tall + (positions[1] + velocities[1] * seconds) % tall) % tall;
    if (endX != wideCenter && endY != tallCenter)
    {
        if (endX < wideCenter && endY < tallCenter) leftTopCount++;
        if (endX < wideCenter && endY > tallCenter) leftBottomCount++;
        if (endX > wideCenter && endY < tallCenter) rightTopCount++;
        if (endX > wideCenter && endY > tallCenter) rightBottomCount++;
    }
}

Console.WriteLine($"{leftTopCount} {rightTopCount} {leftBottomCount} {rightBottomCount} : {leftTopCount * leftBottomCount * rightTopCount * rightBottomCount}");
var streamWriter = new StreamWriter(@"..\..\..\map");

for (int j = 1; j < 10000; j++)
{
    for (int i = 0; i < tall; i++)
    {
        pictureStrings[i] = new string('.', wide);
    }

    foreach (var dataString in robotData)
    {
        var parsedString = parserRegexp.Match(dataString).Groups;
        var positions = parsedString[1].ToString().Split(',').Select(int.Parse).ToArray();
        var velocities = parsedString[2].ToString().Split(',').Select(int.Parse).ToArray();
        var endX = (wide + (positions[0] + velocities[0] * j) % wide) % wide;
        var endY = (tall + (positions[1] + velocities[1] * j) % tall) % tall;
        var filledPosition = pictureStrings[endY].ToCharArray();
        filledPosition[endX] = '1';
        pictureStrings[endY] = string.Join("", filledPosition);
    }

    if (suspicionRegexp.Matches(string.Join("", pictureStrings))
            .Select(m => m.Groups.Values.Select(v => v.Length).Max()).Max() > 10)
    {
        streamWriter.WriteLine(j);
        for (int i = 0; i < pictureStrings.Length; i++)
        {
            streamWriter.WriteLine(pictureStrings[i]);
        }

        streamWriter.WriteLine();
        streamWriter.WriteLine();
    }
}

streamWriter.Close();