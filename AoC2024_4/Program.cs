using System.Text;
using System.Text.RegularExpressions;

var streamReader = new StreamReader(@"..\..\..\input");

var horizontalLines = streamReader.ReadToEnd().Split("\r\n").ToArray();
string[] verticalLines = [];
for (int i = 0; i < horizontalLines[0].Length; i++)
{
    verticalLines = verticalLines.Append(string.Join("",horizontalLines.Select(l => l[i].ToString()).ToArray())).ToArray();
}

var currentColumn = horizontalLines[0].Length - 1;
var currentRow = 0;

string[] diagonalLUToRDLines = [];
while (currentRow < horizontalLines.Length)
{
    var rowIndex = currentRow;
    var columnIndex = currentColumn;
    var line = string.Concat(horizontalLines[rowIndex][columnIndex]);
    
    while (rowIndex < horizontalLines.Length - 1 && columnIndex < horizontalLines[0].Length - 1)
    {
        rowIndex++;
        columnIndex++;
        line += horizontalLines[rowIndex][columnIndex];
    }
    
    diagonalLUToRDLines = diagonalLUToRDLines.Append(line).ToArray();
    if (currentColumn > 0) currentColumn--;
    else currentRow++;
}

currentColumn = 0;
currentRow = 0;

string[] diagonalLDToRULines = [];
while (currentColumn < horizontalLines[0].Length)
{
    var rowIndex = currentRow;
    var columnIndex = currentColumn;
    var line = string.Concat(horizontalLines[rowIndex][columnIndex]);
    
    while (rowIndex > 0 && columnIndex < horizontalLines[0].Length - 1)
    {
        rowIndex--;
        columnIndex++;
        line += horizontalLines[rowIndex][columnIndex];
    }
    
    diagonalLDToRULines = diagonalLDToRULines.Append(line).ToArray();
    if (currentRow < horizontalLines.Length - 1) currentRow++;
    else currentColumn++;
}

List<string> allLines = horizontalLines.ToList();
allLines.AddRange(verticalLines);
allLines.AddRange(diagonalLUToRDLines);
allLines.AddRange(diagonalLDToRULines);

var regexFirstStage = new Regex("XMAS");
var regexFirstStageReverse = new Regex("SAMX");

Console.WriteLine(allLines.Select(l => regexFirstStage.Matches(l).Count + regexFirstStageReverse.Matches(l).Count).Sum());

List<string> validXSymbolsList = new List<string>()
{
    "SSMM",
    "MMSS",
    "SMSM",
    "MSMS",
};

var count = 0;
for (int i = 1; i < horizontalLines.Length - 1; i++)
{
    for (int j = 1; j < horizontalLines[0].Length - 1; j++)
    {
        var symbol = horizontalLines[i][j];
        if (symbol == 'A')
        {
            var sb = new StringBuilder();
            sb.Append(horizontalLines[i-1][j-1]);
            sb.Append(horizontalLines[i-1][j+1]);
            sb.Append(horizontalLines[i + 1][j - 1]);
            sb.Append(horizontalLines[i + 1][j + 1]);
            if (validXSymbolsList.Contains(sb.ToString())) count++;
        }
    }
}


Console.WriteLine(count);