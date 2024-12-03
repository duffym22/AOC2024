using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024.Day03;

class Solution : SolutionBase
{
    public Solution() : base(03, 2024, "") { }

    protected override string SolvePartOne()
    {
        Regex mulEx = new Regex("mul\\([0-9]{1,4}\\,[0-9]{1,4}\\)");
        MatchCollection matchCollection = mulEx.Matches(Input);
        int sumMulInstructions = 0;
        foreach (Match item in matchCollection)
        {
            sumMulInstructions += GetProduct(item.ToString());
        }

        //Attempt 1: 173731097 - CORRECT
        return sumMulInstructions.ToString();
    }

    protected override string SolvePartTwo()
    {
        Regex mulEx = new Regex("(do|don\'t|mul)\\(([0-9]{1,4}\\,[0-9]{1,4})?\\)");
        MatchCollection matchCollection = mulEx.Matches(Input);
        int sumMulInstructions = 0;
        bool mulEnabled = true;

        foreach (Match item in matchCollection)
        {
            string conditional = item.ToString();
            if (conditional.StartsWith("don"))
            {
                mulEnabled = false;
                continue;
            }
            
            if (conditional.StartsWith("do("))
            {
                mulEnabled = true;
                continue;
            }

            if (mulEnabled)
            {
                sumMulInstructions += GetProduct(item.ToString());
            }
        }

        //Attempt 1: 93729253 - CORRECT
        return sumMulInstructions.ToString();
    }

    private int GetProduct(string mulValues)
    {
        Regex valuesOnly = new Regex("[0-9]{1,4}\\,[0-9]{1,4}");
        MatchCollection theDeets = valuesOnly.Matches(mulValues);
        string[] workingString = theDeets.First().ToString().Split(',');
        int.TryParse(workingString.First(), out int val1);
        int.TryParse(workingString.Last(), out int val2);
        return val1 * val2;

    }
}
