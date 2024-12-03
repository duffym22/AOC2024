namespace AdventOfCode.Solutions.Year2024.Day02;

class Solution : SolutionBase
{
    public Solution() : base(02, 2024, "") { }
    //public Solution() : base(02, 2024, "", true) { }

    protected override string SolvePartOne()
    {
        List<string> reports = Input.SplitByNewline().ToList();
        int safeReportCount = 0;

        for (int i = 0; i < reports.Count; i++)
        {
            List<int> levels = reports[i].Split(' ').Select(int.Parse).ToList();
            int firstBadIndex = IsReportGood(levels);
            if (firstBadIndex == -1)
                safeReportCount++;
        }

        //Attempt 1: 536 - Too high
        //Attempt 2: 236 - CORRECT
        return safeReportCount.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> reports = Input.SplitByNewline().ToList();
        int 
            safeReportCountNormal = 0;

        for (int i = 0; i < reports.Count; i++)
        {
            List<int> levels = reports[i].Split(' ').Select(int.Parse).ToList();
            int firstBadIndex = IsReportGood(levels);

            List<int> temp = levels.Select(x => x).Reverse().ToList();
            int secndBadIndex = IsReportGood(temp);
            if (firstBadIndex == -1)
            {
                safeReportCountNormal++;
                continue;
            }

            //Input line 25: [12] 10 11 13 15 18 20 - remove 12 and this becomes valid

            //Remove first identified index and check
            levels.RemoveAt(firstBadIndex);
            if (IsReportGood(levels) == -1)
            {
                safeReportCountNormal++;
                continue;
            }
        }

        //Attempt 1: 439 - Too high
        //Attempt 2: 301 - Too low
        //Attempt 3: 303 - Too low
        return safeReportCountNormal.ToString();
    }

    private int IsReportGood(List<int> levels)
    {
        int lastDirection = 99;
        int badThingHappenedAtIndex = -1;

        for (int j = 0; j < levels.Count - 1; j++)
        {
            int diff = levels[j] - levels[j + 1];

            //Bad - numbers must change by at least 1 and at most 3
            if (diff == 0 || Math.Abs(diff) < 1 || Math.Abs(diff) > 3)
            {
                badThingHappenedAtIndex = j + 1;
                break;
            }

            //First time through
            if (lastDirection == 99)
            {
                lastDirection = diff < 0 ? -1 : 1;
                continue;
            }

            //Bad - switched direction
            if (diff < 0 && lastDirection == 1 || diff > 0 && lastDirection == -1)
            {
                badThingHappenedAtIndex = j + 1;
                break;
            }
        }

        return badThingHappenedAtIndex;
    }
}
