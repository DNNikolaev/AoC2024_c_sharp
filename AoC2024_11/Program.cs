var streamReader = new StreamReader(@"..\..\..\input");

var initialArrangement = streamReader.ReadToEnd();
var initialArrangementStageTwo = initialArrangement;

var cache = new Dictionary<string, long>();

Console.WriteLine(initialArrangement.Split(" ").Select(v => Blink(v, 25)).Sum());
Console.WriteLine(initialArrangementStageTwo.Split(" ").Select(v => Blink(v, 75)).Sum());
return;


long Blink(string input, int depth)
{
    var key = $"{input};{depth}";
    var newDepth = depth - 1;
    if (cache.TryGetValue(key, out var result)) return result;
    if (depth == 0)
    {
        result = 1;
    }
    else if (input == "0") 
    {
        result = Blink("1", newDepth);
    }
    else if (input.Length % 2 == 1)
    {
        result = Blink((Int64.Parse(input) * 2024).ToString(), newDepth);
    }
    else
    {
        var middlePoint = input.Length / 2;
        var delimitedInput = input.Insert(middlePoint, " ");
        var values = delimitedInput.Split(" ").Select(v => Int64.Parse(v).ToString()).ToArray();
        result = Blink(values[0], newDepth) + Blink(values[1], newDepth);
    }

    cache.Add(key, result);
    return result; 
}
