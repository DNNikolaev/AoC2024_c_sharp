static bool isValid(IEnumerable<int> report)
{
    return report.All(diff => Math.Abs(diff) > 0 && Math.Abs(diff) <= 3) &&
           (report.All(int.IsPositive) || report.All(int.IsNegative));
}

static IEnumerable<int> getReportDiffs(IEnumerable<int> report)
{
    return report.Select((reportValue, index) => index == 0 ? 0 : reportValue - report.ToArray()[index - 1])
        .TakeLast(report.Count() - 1);
}

static bool IsAnyPermutationValid(IEnumerable<int> report)
{
    return report.Select((value, index) =>
    {
        var permutation = report.ToList();
        permutation.RemoveAt(index);
        return isValid(getReportDiffs(permutation));
    }).Any(value => value);
}

var streamReader = new StreamReader(@"..\..\..\input");

var reports = streamReader.ReadToEnd().Split("\r\n");
var reportsValues = reports
    .Select(reportData => reportData
        .Split(" ")
        .Select(int.Parse));

var validReports = reportsValues    
    .Select(getReportDiffs).Count(isValid);
    
    Console.WriteLine(validReports);

var validReportsStageTwo = reportsValues.Where(report => isValid(getReportDiffs(report)) || IsAnyPermutationValid(report) );
Console.WriteLine(validReportsStageTwo.Count());