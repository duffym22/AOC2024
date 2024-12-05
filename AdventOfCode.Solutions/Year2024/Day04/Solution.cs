using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode.Solutions.Year2024.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2024, "") { }


    const int MIN = 0;
    const int MAX = 9;

    protected override string SolvePartOne()
    {
        List<string> wordRows = Input.SplitByNewline().ToList();
        char[][] wordSearch = CreateGrid(wordRows);

        List<Point> points = FindX(wordSearch);
        int instancesFound = 0;

        foreach (Point item in points)
        {
            instancesFound += SearchUp(wordSearch, item);
            instancesFound += SearchUpRight(wordSearch, item);
            instancesFound += SearchRight(wordSearch, item);
            instancesFound += SearchDownRight(wordSearch, item);
            instancesFound += SearchDown(wordSearch, item);
            instancesFound += SearchDownLeft(wordSearch, item);
            instancesFound += SearchLeft(wordSearch, item);
            instancesFound += SearchUpLeft(wordSearch, item);
        }

        //Attempt 1: 987 - Too low
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
                if (grid[y][x] == 'X')
                    points.Add(new Point(x, y));
            }
        }
        return points;
    }

    private int SearchUp(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X;       //Same
        int nextY = coordinate.Y - 1;   //Up
        //Make sure we aren't at the edge
        if (nextY < MIN)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            //We started at a coordinate with an 'X'
            //check if the next characters are as expected
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if(i == 2)
                xmasFound++;
            
            nextY--;
            if (nextY < MIN)
                return xmasFound;
        }

        return xmasFound;
    }

    private int SearchUpRight(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X + 1;   //Right
        int nextY = coordinate.Y - 1;   //Up

        //Make sure we aren't at the edge
        if (nextX > MAX || nextY < MIN)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextY--;
            nextX++;
            if (nextX > MAX || nextY < MIN)
                return xmasFound;
        }

        return xmasFound;
    }

    private int SearchRight(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X + 1;   //Right
        int nextY = coordinate.Y;       //Same

        //Make sure we aren't at the edge
        if (nextX > MAX)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextX++;
            //Make sure we aren't at the edge
            if (nextX > MAX)
                return xmasFound;
        }
        return xmasFound;
    }

    private int SearchDownRight(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X + 1;   //Right
        int nextY = coordinate.Y + 1;   //Down

        //Make sure we aren't at the edge
        if (nextX > MAX || nextY > MAX)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextX++;
            nextY++;
            //Make sure we aren't at the edge
            if (nextX > MAX || nextY > MAX)
                return xmasFound;
        }

        return xmasFound;
    }

    private int SearchDown(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X;       //Same
        int nextY = coordinate.Y + 1;   //Down

        //Make sure we aren't at the edge
        if (nextY > MAX)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextY++;
            //Make sure we aren't at the edge
            if (nextY > MAX)
                return xmasFound;
        }

        return xmasFound;
    }

    private int SearchDownLeft(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X - 1;   //Left
        int nextY = coordinate.Y + 1;   //Down

        //Make sure we aren't at the edge
        if (nextX < MIN || nextY > MAX)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextX--;
            nextY++;
            //Make sure we aren't at the edge
            if (nextX < MIN || nextY > MAX)
                return xmasFound;
        }

        return xmasFound;
    }

    private int SearchLeft(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X - 1;   //Left
        int nextY = coordinate.Y;       //Same

        //Make sure we aren't at the edge
        if (nextX < MIN)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextX--;
            //Make sure we aren't at the edge
            if (nextX < MIN)
                return xmasFound;
        }

        return xmasFound;
    }

    private int SearchUpLeft(char[][] grid, Point coordinate)
    {
        int xmasFound = 0;

        int nextX = coordinate.X - 1;   //Left
        int nextY = coordinate.Y - 1;   //Up

        //Make sure we aren't at the edge
        if (nextX < MIN || nextY < MIN)
            return xmasFound;

        //Iterate 3 times to cover the next 3 characters
        for (int i = 0; i < 3; i++)
        {
            char examineChar = 'X';
            switch (i)
            {
                case 0:
                    examineChar = 'M';
                    break;
                case 1:
                    examineChar = 'A';
                    break;
                case 2:
                    examineChar = 'S';
                    break;
            }

            if (grid[nextY][nextX].Equals(examineChar) == false)
                return xmasFound;
            else if (i == 2)
                xmasFound++;

            nextX--;
            nextY--;
            //Make sure we aren't at the edge
            if (nextX < MIN || nextY < MIN)
                return xmasFound;
        }

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
