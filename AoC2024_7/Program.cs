var streamReader = new StreamReader(@"..\..\..\input");

var lines = streamReader.ReadToEnd().Split("\r\n");

string ConvertToBaseThree(int number)
{
    var stepResult = number;
    var result = (stepResult % 3).ToString();
    while (stepResult / 3 > 0)
    {
        stepResult /= 3;
        result = stepResult % 3 + result;
    }
    return result;
}
Int64 sum = 0;
Int64 sumStageTwo = 0;
foreach (var equation in lines)
{
    var partsOfEquation = equation.Split(": ");
    var result = Int64.Parse(partsOfEquation[0]);
    var numberUsed = partsOfEquation[1].Split(" ").Select(Int64.Parse).ToArray();
    var numberOfCombinations = (Int64)Math.Pow(2, numberUsed.Length - 1);
    for (int i = 0; i < numberOfCombinations; i++)
    {
        var bitRepresentation = Convert.ToString(i, 2).PadLeft(numberUsed.Length - 1, '0');
        var possibleResult = numberUsed[0];
        for (int j = 0; j < bitRepresentation.Length; j++)
        {
            switch (bitRepresentation[j])
            {
                case '0':
                    possibleResult += numberUsed[j+1];
                    break;
                case '1':
                    possibleResult *= numberUsed[j+1];
                    break;
            }
        }

        if (result == possibleResult)
        {
            sum += possibleResult;
            break;
        }
    }
    
    var numberOfCombinationsStageTwo = (Int64)Math.Pow(3, numberUsed.Length - 1);
    for (int i = 0; i < numberOfCombinationsStageTwo; i++)
    {
        var bitRepresentation = ConvertToBaseThree(i).PadLeft(numberUsed.Length - 1, '0');
        var possibleResult = numberUsed[0];
        for (int j = 0; j < bitRepresentation.Length; j++)
        {
            switch (bitRepresentation[j])
            {
                case '0':
                    possibleResult += numberUsed[j+1];
                    break;
                case '1':
                    possibleResult *= numberUsed[j+1];
                    break;
                case '2':
                    possibleResult = Int64.Parse(possibleResult.ToString() + numberUsed[j+1]);
                    break;
            }
        }

        if (result == possibleResult)
        {
            sumStageTwo += possibleResult;
            break;
        }
    }
}

Console.WriteLine(sum);
Console.WriteLine(sumStageTwo);