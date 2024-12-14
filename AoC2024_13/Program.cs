using System.Text.RegularExpressions;

var streamReader = new StreamReader(@"..\..\..\input");
var getButtonIncrementsRegexp = new Regex(@".*X\+(\d+), Y\+(\d+)");
var getMachineTargetsRegexp = new Regex(@".*X=(\d+), Y=(\d+)");

var machinesDescriptions = streamReader.ReadToEnd().Split("\r\n\r\n");
long tokenCount = 0;
long tokenCountStageTwo = 0;
foreach (var machineDescriptionString in machinesDescriptions)
{
    var machineDescription = machineDescriptionString.Split("\r\n");
    var buttonAInfo = getButtonIncrementsRegexp.Match(machineDescription[0]);
    var buttonBInfo = getButtonIncrementsRegexp.Match(machineDescription[1]);
    var targetInfo = getMachineTargetsRegexp.Match(machineDescription[2]);
    var buttonA = (x: int.Parse(buttonAInfo.Groups[1].ToString()), y: int.Parse(buttonAInfo.Groups[2].ToString()));
    var buttonB = (x: int.Parse(buttonBInfo.Groups[1].ToString()), y: int.Parse(buttonBInfo.Groups[2].ToString()));
    var target = (x: long.Parse(targetInfo.Groups[1].ToString()), y: long.Parse(targetInfo.Groups[2].ToString()));
    
    var biggerX = buttonA;
    var smallerX = buttonB;
    var costBiggerX = 3;
    var costSmallerX = 1;
    if (buttonB.x > buttonA.x)
    {
        biggerX = buttonB;
        smallerX = buttonA;
        costBiggerX = 1;
        costSmallerX = 3;
    }
    var possibleSolutions = new List<(long countBigger, long countSmaller)>();
    var reminderX = target.x;
    var reminderY = target.y;
    var countBigger = 0;
    if (target.x > (buttonA.x * 100 + buttonB.x * 100) || target.y > (buttonA.y * 100 + buttonB.y * 100)) continue;
    while (reminderX > biggerX.x)
    {
        reminderX -= biggerX.x;
        reminderY -= biggerX.y;
        countBigger++;
        var countSmallerX = reminderX / smallerX.x;
        var countSmallerY = reminderY / smallerX.y;
        if (countSmallerX > 100 || countSmallerY > 100 ) continue;
        if (reminderX % smallerX.x == 0 && reminderY % smallerX.y == 0 && countSmallerX == countSmallerY)
        {
            possibleSolutions.Add((countBigger, countSmallerX));
            // Console.WriteLine($"{buttonA} {buttonB} {target}: bigger: {countBigger}, cost: {countBigger * costBiggerX} smaller: {countSmallerX}, cost: {countSmallerX * costSmallerX} ");
        }
    }
    
    var smallestTokenCount = possibleSolutions.Count > 0 ? possibleSolutions
        .Select(s => s.countBigger * costBiggerX + s.countSmaller * costSmallerX)
        .Min() : 0;
    tokenCount += smallestTokenCount;
    
    target.x += 10000000000000;
    target.y += 10000000000000;
    var aClicks = (double)(target.x * buttonB.y - buttonB.x * target.y) / (buttonA.x * buttonB.y - buttonB.x * buttonA.y);
    var bClicks = (double)(target.y * buttonA.x - buttonA.y * target.x) / (buttonA.x * buttonB.y - buttonB.x * buttonA.y);
    if (aClicks > 0 && Math.Abs(aClicks % 1) <= (Double.Epsilon * 100) && bClicks > 0 && Math.Abs(bClicks % 1) <= (Double.Epsilon * 100))
    {
        tokenCountStageTwo += ((long)aClicks * 3 + (long)bClicks);
    }
}

Console.WriteLine(tokenCount);
Console.WriteLine(tokenCountStageTwo);
