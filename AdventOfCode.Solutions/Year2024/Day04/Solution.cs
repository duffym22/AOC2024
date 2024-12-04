using System.Drawing;

namespace AdventOfCode.Solutions.Year2024.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2024, "") { }

    protected override string SolvePartOne()
    {
        List<string> wordRows = Input.SplitByNewline().ToList();
        char[][] wordSearch = CreateGrid(wordRows); 

        List<Point> points = FindX(wordSearch);
        int instancesFound = 0;

        foreach (Point item in points)
        {
            instancesFound += Search(wordSearch, item);
        }

        return instancesFound.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> reports = Input.SplitByNewline().ToList();

        return "";
    }

    private char[][] CreateGrid(List<string> wordRows)
    {
        char[][] wordSearch = new char[wordRows.Count][];
        for (int i = 0; i < wordRows.Count; i++)
        {
            wordSearch[i] = wordRows[i].ToCharArray();
        }
        return wordSearch;
    }

    private List<Point> FindX(char[][] grid)
    {
        List<Point> points = new List<Point>();
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[x][y] == 'X')
                    points.Add(new Point(x,y));
            }
        }
        return points;
    }

    private int Search(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        //Up
        if(coordinate.Y == 0)
        {
            
        }

        //Up-Right

        //Right

        //Down-Right

        //Down

        //Down-Left

        //Left

        //Up-Left

        return xmasFound;
    }

    private int SearchForward(string row)
    {
        int found = 0, idx = -1;
        idx = row.IndexOf('X');
        if (idx == -1) { return found; }

        if (idx + 3 < row.Length && row.Substring(idx, 4) == "XMAS")
            found++;
        int nextIdx = idx + 1;
        int nextLen = row.Length - nextIdx;
        if (nextLen >= 4)
        {
            string sub = row.Substring(nextIdx, nextLen);
            found += SearchForward(sub);
        }

        return found;
    }

    private int SearchBackward(string row)
    {
        int found = 0, idx = -1;
        idx = row.IndexOf('S');
        if (idx == -1) { return found; }

        if (idx + 3 < row.Length && row.Substring(idx, 4) == "SAMX")
            found++;
        int nextIdx = idx + 1;
        int nextLen = row.Length - nextIdx;
        if (nextLen >= 4)
        {
            string sub = row.Substring(nextIdx, nextLen);
            found += SearchBackward(sub);
        }

        return found;
    }

}
