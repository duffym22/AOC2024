using System.Diagnostics;
using System.IO.IsolatedStorage;

namespace AdventOfCode.Solutions.Year2024.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2024, "") { }


    protected override string SolvePartOne()
    {
        ParseAndSort(out List<int> Left, out List<int> Right);

        int totalDistance = 0;
        for (int i = 0; i < Left.Count; i++)
        {
            int diff = Math.Abs(Left[i] - Right[i]);
            totalDistance += diff;
        }

        //Attempt 1: 1222801 - CORRECT
        return totalDistance.ToString();
    }

    protected override string SolvePartTwo()
    {
        ParseAndSort(out List<int> Left, out List<int> Right);
        int similarity = 0;
        // Original - executes in 42.2 ms
        //for (int i = 0; i < Left.Count; i++)
        //{
        //    similarity += Left[i] * Right.Count(x => x == Left[i]);
        //}

        // Executes in 1.2 ms
        Dictionary<int, int> keyValuePairs = new();
        foreach (int i in Right)
        {
            if (keyValuePairs.ContainsKey(i))
                keyValuePairs[i] += 1;
            else
                keyValuePairs.Add(i, 1);
        }

        foreach (int item in Left)
        {
            if(keyValuePairs.ContainsKey(item))
                similarity += item * keyValuePairs[item];
        }

        //Attempt 1: 22545250 - CORRECT
        return similarity.ToString();
    }

    private void ParseAndSort(out List<int> Left, out List<int> Right)
    {
        List<string> ParsedInput = Input.SplitByNewline().ToList();
        Left = new List<int>();
        Right = new List<int>();

        foreach (string item in ParsedInput)
        {
            string[] sp = item.Split(" ");
            Left.Add(int.Parse(sp.First()));
            Right.Add(int.Parse(sp.Last()));
        }

        Left.Sort();
        Right.Sort();
    }
}
