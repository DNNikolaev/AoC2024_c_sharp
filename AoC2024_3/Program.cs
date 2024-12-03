using System.Text.RegularExpressions;

var streamReader = new StreamReader(@"..\..\..\input");

var input = streamReader.ReadToEnd().Replace("\r\n", "");
var stageOneRegex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
var matches = stageOneRegex.Matches(input);
Console.WriteLine(matches.Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value)).Sum());

var stageTwoEnabledMatches = Regex.Matches(input, @"^(.*?)(don't\(\))|do\(\)(.*?)(don't\(\))|do\(\)(.*?)$");
Console.WriteLine(stageTwoEnabledMatches
    .Select(m => stageOneRegex.Matches(m.Groups[0].Value)
        .Select(m => int.Parse(m.Groups[1].Value) * int.Parse(m.Groups[2].Value))
        .Sum())
    .Sum()
);
