var streamReader = new StreamReader(@"..\..\..\input");

var diskMap = streamReader.ReadToEnd();
// Console.WriteLine(diskMap);
var diskRepresentation = new List<string>();
var fileId = 0;
for (int i = 0; i < diskMap.Length; i++)
{
        var valueToFill = i % 2 == 0 ? (fileId++).ToString() : ".";
        for (int j = 0; j < int.Parse(diskMap[i].ToString()); j++)
        {
                diskRepresentation.Add(valueToFill);                
        }
}

// Console.WriteLine(string.Join("",diskRepresentation));
var diskRepresentationStageTwo = diskRepresentation.ToList();
var lastCharPosition = diskRepresentation.Count - 1;
for (int i = 0; i < diskRepresentation.Count; i++)
{
        if (diskRepresentation[i] != ".") continue;
        for (int j = lastCharPosition; j > i; j--)
        {
                if (diskRepresentation[j] == ".") continue;
                diskRepresentation[i] = diskRepresentation[j];
                diskRepresentation[j] = ".";
                lastCharPosition = j - 1;
                break;
        }
        if (i >= lastCharPosition) break;
}
// Console.WriteLine(string.Join("",diskRepresentation));
// Console.WriteLine();
// Console.WriteLine(string.Join("",diskRepresentationStageTwo));
var diskMapToChange = diskMap.Select(c => int.Parse(c.ToString())).ToArray();
var modifiedDiskMapToChange = diskMapToChange.ToList();
for (int i = modifiedDiskMapToChange.Count - 1; i >= 0; i -= 2)
{
        var fileSize = modifiedDiskMapToChange[i];
        if (fileSize == 0) continue;
        
        for (int j = 1; j < i; j += 2)
        {
                var emptySpaceSize = modifiedDiskMapToChange[j];
                        
                // Console.WriteLine($"{i}: {fileSize}; {j}: {emptySpaceSize}");
                if (fileSize > emptySpaceSize) continue;
                
                var endIndex = diskRepresentationStageTwo.Count - 1;
                for (int k = modifiedDiskMapToChange.Count - 1; k > i; k--) endIndex -= modifiedDiskMapToChange[k];
                var valueToTransfer = diskRepresentationStageTwo[endIndex];
                for (int l = endIndex; l > endIndex - fileSize; l--) diskRepresentationStageTwo[l] = ".";
                // Console.WriteLine($"{string.Join("",diskRepresentationStageTwo)}");
                
                var startIndex = 0;
                for (int k = 0; k < j; k++) startIndex += modifiedDiskMapToChange[k];
                for (int l = startIndex; l < startIndex + fileSize; l++) diskRepresentationStageTwo[l] = valueToTransfer;
                // Console.WriteLine($"{string.Join("",diskRepresentationStageTwo)}");
                
                
                
                modifiedDiskMapToChange.Insert(j, 0);
                modifiedDiskMapToChange.Insert(j+1, fileSize);
                modifiedDiskMapToChange[j+2] -= fileSize;
                i += 2;
                modifiedDiskMapToChange[i] = 0;
                modifiedDiskMapToChange[i-1] += fileSize;
                // Console.WriteLine($"{string.Join("",modifiedDiskMapToChange)}");
                
                break;
        }
}

Int64 sum = 0;
for (int i = 0; i < diskRepresentation.Count; i++)
{
        if (diskRepresentation[i] == ".") continue;
        sum += i * int.Parse(diskRepresentation[i]);
}
Console.WriteLine(sum);

Int64 sumStageTwo = 0;
for (int i = 0; i < diskRepresentationStageTwo.Count; i++)
{
        if (diskRepresentationStageTwo[i] == ".") continue;
        sumStageTwo += i * int.Parse(diskRepresentationStageTwo[i]);
}
Console.WriteLine(sumStageTwo);