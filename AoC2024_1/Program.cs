var streamReader = new StreamReader(@"..\..\..\input");

var lines = streamReader.ReadToEnd().Split("\r\n");
var leftList = lines.Select(l => int.Parse(l.Split(' ')[0])).ToList();
var rightList = lines.Select(l => int.Parse(l.Split(' ')[^1])).ToList();

Console.WriteLine(leftList.Sum() - rightList.Sum());

leftList.Sort();
rightList.Sort();

var sum = leftList.Select((l, i) => Math.Abs(l - rightList[i])).Sum();

Console.WriteLine($"Left: {leftList[0]}; Right: {rightList[0]} ");
Console.WriteLine($"Sum: {sum}");

var stageTwoSum = leftList.Select(l => rightList.FindAll(r => r == l).Count * l).Sum();

Console.WriteLine($"Stage Two Sum: {stageTwoSum}");
