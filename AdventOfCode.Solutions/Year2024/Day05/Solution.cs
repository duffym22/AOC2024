namespace AdventOfCode.Solutions.Year2024.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2024, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByNewline().ToList();
        List<int[]> rules = new List<int[]>();
        List<int[]> pages = new List<int[]>();
        int lastIndex = 0;

        //Parse the rules
        for (int i = 0; i < lines.Count; i++)
        {
            //When the line contains commas, break out to parse them separately
            if (lines[i].Contains(","))
            { lastIndex = i; break; }

            string[] strings = lines[i].Split(new char[] { '|' });
            rules.Add(new int[] { int.Parse(strings.First()), int.Parse(strings.Last()) });
        }

        //Parse the updated page numbers
        for (int j = lastIndex; j < lines.Count; j++)
        {
            string[] strings = lines[j].Split(',');
            int[] nums = strings.ToList().Select(x => int.Parse(x)).ToArray();
            pages.Add(nums);
        }

        List<int[]> applicableRules = new List<int[]>();
        for (int i = 0; i < pages.Count; i++)
        {
            //Get all applicable rules
            foreach (int[] rule in rules)
            {
                if(DoesRuleApply(rule, pages[i]))
                    applicableRules.Add(rule);
            }

            //Determine if the update follows all the applicable rules


        }



        return "";
    }

    protected override string SolvePartTwo()
    {
        return "";
    }

    private bool DoesRuleApply(int[] rule, int[] pages)
    {
        int firstRule = rule.First();
        int secondRule = rule.Last();

        bool first = false;
        bool second = false;

        foreach (int item in pages)
        {
            if (!first && item.Equals(firstRule))
            {
                first = true;
            }

            if (!second && item.Equals(secondRule))
            {
                second = true;
            }
        }

        return first && second;
    }
}