namespace AdventOfCode.Solutions.Year2024.Day04;

class Solution : SolutionBase
{
    public Solution() : base(04, 2024, "") { }

    protected override string SolvePartOne()
    {
        List<string> wordRows = Input.SplitByNewline().ToList();

        char[][] wordSearch = new char[wordRows.Count][];

        for (int i = 0; i < wordRows.Count; i++)
        {
            wordSearch[i] = wordRows[i].ToCharArray();
        }

        int instancesFound = 0;
        foreach (char[] item in wordSearch)
        {
            string r = item.JoinAsStrings();
            instancesFound += SearchForward(r);
            instancesFound += SearchBackward(r);
        }

        char[][] rotatedWordSearch = RotateWordSearch(wordSearch);



        return instancesFound.ToString();
    }

    protected override string SolvePartTwo()
    {
        List<string> reports = Input.SplitByNewline().ToList();

        return "";
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

    private char[][] RotateWordSearch(char[][] ws)
    {
        char[][] rotated = new char[ws.Length][];
        string[] assembledRows = new string[ws.Length];
        for (int j = ws.Length - 1; j > 0; j--)
        {
            for (int i = 0; i < ws.Length; i++)
            {
                assembledRows[i] = assembledRows[i] += ws[i][j];
            }
        }
        return rotated;
    }

}
