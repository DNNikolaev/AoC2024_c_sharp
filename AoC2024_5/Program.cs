var streamReader = new StreamReader(@"..\..\..\input");

var lines = streamReader.ReadToEnd().Split("\r\n").ToList();
var splitIndex = lines.IndexOf("");
var rules = lines.Slice(0, splitIndex);
var updates = lines.Slice(splitIndex + 1, lines.Count - rules.Count - 1).Select(line => line.Split(',')).ToArray();

bool IsValidUpdate(string[] update)
{
    for (int i = update.Length - 1; i > 0; i--)
    {
        for (int j = i - 1; j >= 0; j--)
        {
            // Console.WriteLine($"i: {i}, j:{j}, {update[i]}|{update[j]} rule index: {rules.IndexOf($"{update[i]}|{update[j]}")}");
            if (rules.IndexOf($"{update[i]}|{update[j]}") > -1) return false;   
        }
        
    }

    return true;
}

var validUpdates = updates.Where(IsValidUpdate);
var sum = validUpdates.Sum(update =>  int.Parse(update[(int)Math.Floor(update.Length / 2.0)]));
Console.WriteLine(sum);

string[] MakeValid(string[] invalidUpdate)
{
    string[] validUpdate = new string[invalidUpdate.Length];
    var validUpdateV2 = new List<string>();
    for (int i = invalidUpdate.Length - 1; i >= 0; i--)
    {
        var initialIndex = invalidUpdate.Length - 1;
        for (int j = invalidUpdate.Length - 1; j >= 0; j--)
        {
            if (rules.IndexOf($"{invalidUpdate[i]}|{invalidUpdate[j]}") > -1) initialIndex--;
        }

        while (!string.IsNullOrEmpty(validUpdate[initialIndex]))
        {
            initialIndex--;
        }        
        validUpdate[initialIndex] = invalidUpdate[i];
    }
    return validUpdate;
}

var invalidUpdates = updates.Where(u => !IsValidUpdate(u));
var sumStageTwo = invalidUpdates.Select(MakeValid).Sum(update => int.Parse(update[(int)Math.Floor(update.Length / 2.0)]));
Console.WriteLine(sumStageTwo);