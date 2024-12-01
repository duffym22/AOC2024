using System.IO.IsolatedStorage;

namespace AdventOfCode.Solutions.Year2024.Day01;

class Solution : SolutionBase
{
    public Solution() : base(01, 2024, "") { }


    protected override string SolvePartOne()
    {
        List<string> ParsedInput = Input.SplitByNewline().ToList();
        List<int> Left = new List<int>();
        List<int> Right = new List<int>();

        foreach (string item in ParsedInput)
        {
            string[] sp = item.Split(" ");
            Left.Add(int.Parse(sp.First()));
            Right.Add(int.Parse(sp.Last()));
        }

        Left.Sort();
        Right.Sort();

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
        List<string> ParsedInput = Input.SplitByNewline().ToList();
        List<int> Left = new List<int>();
        List<int> Right = new List<int>();

        foreach (string item in ParsedInput)
        {
            string[] sp = item.Split(" ");
            Left.Add(int.Parse(sp.First()));
            Right.Add(int.Parse(sp.Last()));
        }

        Left.Sort();
        Right.Sort();
        int similarity = 0;
        for (int i = 0; i < Left.Count; i++)
        {
            similarity += Left[i] * Right.Count(x => x == Left[i]);
        }

        //Attempt 1: 22545250 - CORRECT
        return similarity.ToString();
    }
}
