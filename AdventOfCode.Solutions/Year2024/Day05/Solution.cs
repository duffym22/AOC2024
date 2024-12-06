


using System.Data;

namespace AdventOfCode.Solutions.Year2024.Day05;

class Solution : SolutionBase
{
    public Solution() : base(05, 2024, "") { }

    protected override string SolvePartOne()
    {
        List<string> lines = Input.SplitByNewline().ToList();
        List<List<int>> rules = new List<List<int>>();
        List<List<int>> pages = new List<List<int>>();
        List<int> validUpdatePagesMiddleNumbers = new();
        ParseInput(lines, out rules, out pages);

        for (int i = 0; i < pages.Count; i++)
        {
            //Get all applicable rules AND determine if the update page follows all of them
            Dictionary<List<int>, bool> applicableRules;
            GetTheRulesAndApplyThem(rules, pages[i], out applicableRules);

            //Evaluate if all rules were met, if so,
            //get the middle number of the page
            //add it to the list which will be summed up later
            if (EvaluateAllRulesMet(applicableRules))
            {
                validUpdatePagesMiddleNumbers.Add(GetMiddlePageNumber(pages[i]));
            }
        }

        //Attempt 1: 3608 - CORRECT
        return validUpdatePagesMiddleNumbers.Sum(x => x).ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> lines = Input.SplitByNewline().ToList();
        List<List<int>> rules = new List<List<int>>();
        List<List<int>> pages = new List<List<int>>();
        List<int> validUpdatePagesMiddleNumbers = new();
        ParseInput(lines, out rules, out pages);

        for (int i = 0; i < pages.Count; i++)
        {
            //Get all applicable rules AND determine if the update page follows all of them
            Dictionary<List<int>, bool> applicableRules;
            GetTheRulesAndApplyThem(rules, pages[i], out applicableRules);

            //Take ONLY the rules that were not followed
            if (EvaluateAllRulesMet(applicableRules) == false)
            {
                Dictionary<List<int>, bool> invalidRules = GetInvalidRules(applicableRules);
                List<int> newPage = pages[i];
                while (invalidRules.Count() > 0)
                {
                    newPage = ReorderThePage(newPage, invalidRules.Keys.First());
                    GetTheRulesAndApplyThem(rules, newPage, out applicableRules);
                    invalidRules = GetInvalidRules(applicableRules);
                }
                validUpdatePagesMiddleNumbers.Add(GetMiddlePageNumber(newPage));
            }
        }

        //Attempt 1: 4922 - CORRECT
        return validUpdatePagesMiddleNumbers.Sum(x => x).ToString();
    }

    private void ParseInput(List<string> lines, out List<List<int>> rules, out List<List<int>> pages)
    {
        int lastIndex = 0;
        rules = new();
        pages = new();

        //Parse the rules
        for (int i = 0; i < lines.Count; i++)
        {
            //When the line contains commas, break out to parse them separately
            if (lines[i].Contains(","))
            {
                lastIndex = i;
                break;
            }

            string[] strings = lines[i].Split(new char[] { '|' });
            rules.Add(new List<int> { int.Parse(strings.First()), int.Parse(strings.Last()) });
        }

        //Parse the updated page numbers
        for (int j = lastIndex; j < lines.Count; j++)
        {
            string[] strings = lines[j].Split(',');
            List<int> nums = strings.ToList().Select(x => int.Parse(x)).ToList();
            pages.Add(nums);
        }
    }

    private bool DoesRuleApply(List<int> rule, List<int> pages)
    {
        //Rule applies if the update page contains BOTH numbers in the rule set
        return pages.Where(x => x == rule.First()).Count() > 0 && pages.Where(x => x == rule.Last()).Count() > 0;
    }

    private bool DoesPageFollowRule(List<int> rule, List<int> pages)
    {
        List<int> p = pages.ToList();
        //Does index of first number come before index of the second number
        return p.IndexOf(rule.First()) < p.IndexOf(rule.Last());
    }

    private void ApplyTheRules(List<int> pages, Dictionary<List<int>, bool> applicableRules)
    {
        //If the rule is followed, set the dictionary value to true - we'll count the number of TRUE rules later
        var keys = applicableRules.Keys.ToArray();
        for (int j = 0; j < keys.Length; j++)
        {
            if (DoesPageFollowRule(keys[j], pages))
                applicableRules[keys[j]] = true;
        }
    }

    private void GetApplicableRules(List<List<int>> rules, List<int> pages, out Dictionary<List<int>, bool> applicableRules)
    {
        //Rule applies if the update page contains BOTH numbers in the rule set
        applicableRules = new();
        foreach (List<int> rule in rules)
        {
            if (DoesRuleApply(rule, pages))
                applicableRules.Add(rule, false);
        }
    }

    private void GetTheRulesAndApplyThem(List<List<int>> rules, List<int> pages, out Dictionary<List<int>, bool> applicableRules)
    {
        applicableRules = new();
        foreach (List<int> rule in rules)
        {
            //Only add applicable rules to the list, even if the pages don't follow them
            //This is necessary for Part 2 because we need to work on the rules that were not followed (FALSE)
            if (DoesRuleApply(rule, pages))
            {
                applicableRules.Add(rule, DoesPageFollowRule(rule, pages));
            }
        }
    }

    private bool EvaluateAllRulesMet(Dictionary<List<int>, bool> applicableRules)
    {
        //Determine if the update page followed all of the applicable rules
        //  >> Count of all the rules in the dictionary is equal the count of rules set to TRUE
        //if so, then add it to the list of valid 
        return applicableRules.Where(x => x.Value == true).Count() == applicableRules.Count();
    }

    private Dictionary<List<int>, bool> GetInvalidRules(Dictionary<List<int>, bool> applicableRules)
    {
        return applicableRules.Where(x => x.Value == false).ToDictionary();
    }

    private List<int> ReorderThePage(List<int> arrPageCurr, List<int> firstInvalidRule)
    {
        List<int> pageCurrent = arrPageCurr.ToList();
        List<int> pageModified = arrPageCurr.Select(x => x).ToList();   //deep-copy the list

        for (int i = 0; i < arrPageCurr.Count; i++)
        {
            //Get the index of where the page must come before
            if (i == pageCurrent.IndexOf(firstInvalidRule.Last()))
            {
                pageModified.Remove(firstInvalidRule.First());
                pageModified.Insert(i, firstInvalidRule.First());
            }
        }
        return pageModified;

    }

    private int GetMiddlePageNumber(List<int> update)
    {
        //Cut it in half - if its not a whole number it will automatically round up
        return update[(update.Count / 2)];
    }
}