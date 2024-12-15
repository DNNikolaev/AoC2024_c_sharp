using System.Drawing;
using System.Text;

var streamReader = new StreamReader(@"..\..\..\input");

var allInfo = streamReader.ReadToEnd().Split("\r\n\r\n");
var map = allInfo[0].Split("\r\n").Select(s => s.ToCharArray()).ToArray();
var map2 = allInfo[0].Split("\r\n").Select(s => s.ToCharArray()).ToArray();
var movements = allInfo[1].Replace("\r\n", "");
var mapString = allInfo[0].Replace("\r\n", "");
var currentPosition = new Point()
{
    X = mapString.IndexOf('@') % map[0].Length,
    Y = mapString.IndexOf('@') / map[0].Length
};
var positionDiff = new Point(0, 0);
var positionsToPush = new List<Point>();
var positionsToPushVertically = new List<Point>();

for (int i = 0; i < movements.Length; i++)
{
    positionDiff = GetPositionChange(movements[i]);
    if (CanMove(map)) Move(map);
}

var mapStageTwoList = new List<string>();
foreach (var mapStringA in map2)
{
    var sb = new StringBuilder();
    foreach (var symbol in mapStringA)
    {
        if (symbol == 'O') sb.Append('[').Append(']');
        else if (symbol == '@') sb.Append('@').Append('.');
        else sb.Append(symbol).Append(symbol);
    }
    mapStageTwoList.Add(sb.ToString());
}

var mapStageTwo = mapStageTwoList.Select(s => s.ToCharArray()).ToArray();
mapString = string.Join("", mapStageTwo.Select(s => string.Join("", s)));

currentPosition = new Point()
{
    X = mapString.IndexOf('@') % mapStageTwo[0].Length,
    Y = mapString.IndexOf('@') / mapStageTwo[0].Length
};

for (int i = 0; i < movements.Length; i++)
{
    positionDiff = GetPositionChange(movements[i]);
    if ((movements[i] == '<' || movements[i] == '>') && CanMove(mapStageTwo)) 
    {
        Move(mapStageTwo);
    }

    if ((movements[i] == '^' || movements[i] == 'v') && CanMoveVertically(currentPosition))
    {
        positionsToPushVertically = positionsToPushVertically.Distinct().ToList();
        positionsToPushVertically.Sort((pointA, pointB) =>
        {
            if (pointA.Y == pointB.Y) return 0;
            return !(pointA.Y > pointB.Y ^ movements[i] == '^') ? -1 : 1;
        });
        MoveVertically();
        positionsToPushVertically.Clear();
    }
}

Console.WriteLine(CalculateGPS());
Console.WriteLine(CalculateGPSStageTwo());
return;

void Move(char[][] mapToTraverse)
{
    if (positionsToPush.Count > 0)
    {
        for (int i = positionsToPush.Count - 1; i >= 0 ; i--)
        {
            mapToTraverse[positionsToPush[i].Y + positionDiff.Y][positionsToPush[i].X + positionDiff.X] =
                mapToTraverse[positionsToPush[i].Y][positionsToPush[i].X];
        }

        positionsToPush.Clear();
    }

    mapToTraverse[currentPosition.Y][currentPosition.X] = '.';
    currentPosition.X += positionDiff.X;
    currentPosition.Y += positionDiff.Y;
    mapToTraverse[currentPosition.Y][currentPosition.X] = '@';
}

bool CanMove(char[][] mapToTraverse)
{
    var nextPosition = new Point(currentPosition.X + positionDiff.X, currentPosition.Y + positionDiff.Y);
    var nextPositionSymbol = mapToTraverse[nextPosition.Y][nextPosition.X];
    
    if (nextPositionSymbol == '.') return true;

    while (nextPositionSymbol != '.')
    {
        if (nextPositionSymbol == '#')
        {
            positionsToPush.Clear();
            return false;
        }
        positionsToPush.Add(new Point(nextPosition.X, nextPosition.Y));
        nextPosition.X += positionDiff.X;
        nextPosition.Y += positionDiff.Y;
        nextPositionSymbol = mapToTraverse[nextPosition.Y][nextPosition.X];
    }
    
    return true;
}

void MoveVertically()
{
    if (positionsToPushVertically.Count > 0)
    {
        for (int i = positionsToPushVertically.Count - 1; i >= 0 ; i--)
        {
            mapStageTwo[positionsToPushVertically[i].Y + positionDiff.Y][positionsToPushVertically[i].X + positionDiff.X] =
                mapStageTwo[positionsToPushVertically[i].Y][positionsToPushVertically[i].X];
            mapStageTwo[positionsToPushVertically[i].Y][positionsToPushVertically[i].X] = '.';
        }
    }

    mapStageTwo[currentPosition.Y][currentPosition.X] = '.';
    currentPosition.X += positionDiff.X;
    currentPosition.Y += positionDiff.Y;
    mapStageTwo[currentPosition.Y][currentPosition.X] = '@';
}

bool CanMoveVertically(Point startingPosition)
{
    var nextPosition = new Point(startingPosition.X + positionDiff.X, startingPosition.Y + positionDiff.Y);
    var nextPositionSymbol = mapStageTwo[nextPosition.Y][nextPosition.X];
    
    while (nextPositionSymbol != '.')
    {
        if (nextPositionSymbol == '#')
        {
            positionsToPushVertically.Clear();
            return false;
        }
        positionsToPushVertically.Add(nextPosition);
        var secondPositionToCheck = nextPosition with
        {
            X = nextPosition.X + 1
        };
        if (nextPositionSymbol == ']') secondPositionToCheck = nextPosition with
        {
            X = nextPosition.X - 1
        };

        positionsToPushVertically.Add(secondPositionToCheck);
        return CanMoveVertically(nextPosition) && CanMoveVertically(secondPositionToCheck);
    }
    
    return true;
}

Point GetPositionChange(char direction)
{
    switch (direction)
    {
        case '^':
            return new Point(0, -1);
        case '<':
            return new Point(-1, 0);
        case '>':
            return new Point(1, 0);
        default:
            return new Point(0, 1);
    }
}

int CalculateGPS()
{
    var sum = 0;
    for (int i = 1; i < map.Length - 1; i++)
    {
        for (int j = 1; j < map[i].Length - 1; j++)
        {
            if (map[i][j] == 'O') sum += i * 100 + j;
        }
    }

    return sum;
} 

int CalculateGPSStageTwo()
{
    var sum = 0;
    for (int i = 1; i < mapStageTwo.Length - 1; i++)
    {
        for (int j = 1; j < mapStageTwo[i].Length - 1; j++)
        {
            if (mapStageTwo[i][j] == '[') sum += i * 100 + j;
        }
    }

    return sum;
} 